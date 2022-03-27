using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGatoController : MonoBehaviour, ITargetCombat
{
    // Máquina de estados del autómata
    public enum HellGatoState
    {
        Inactive,
        ChasePlayer,
        WalkInTransformRight,
        Turn
    }

    [SerializeField] float health = 1;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] HellGatoState hellGatoState = HellGatoState.Inactive;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] LayerChecker groundChecker;
    [SerializeField] LayerChecker blockChecker;
    [SerializeField] LayerChecker rangeVision;

    [SerializeField] private float speed = 1;

    private Rigidbody2D _rigidbody;

    private bool active;

    private void Awake()
    {
        hellGatoState = HellGatoState.Inactive;
        animatorController.Pause();

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (active)
        {
            //print(active);
            if (hellGatoState == HellGatoState.WalkInTransformRight)
            {
                //print("Entro en Walk");
                WalkInTransformRight();
            }

            if (hellGatoState == HellGatoState.ChasePlayer)
            {
                ChasePlayer();
            }
        }
    }

    private void ChasePlayer()
    {
        if ((!groundChecker.isTouching || blockChecker.isTouching) && rangeVision.isTouching)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;
            //_rigidbody.velocity = Vector2.zero;
            // Aquí iría una animacion de Idle... pero no tengo xd
        }
        else
        {
            MoveTo(HeroController.SharedInstance.transform.position);
        }

        if (!rangeVision.isTouching)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;
        }
    }

    private void MoveTo(Vector2 targetPosition)
    {
        var targetDirection = targetPosition - (Vector2)this.transform.position;
        Vector2 currentPosition = this.transform.position;

        this.transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);

        //_rigidbody.velocity = new Vector2(targetDirection.normalized.x * speed, _rigidbody.velocity.y);

        if (targetDirection.normalized.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void WalkInTransformRight()
    {
        animatorController.Play(AnimationID.Run);

        _rigidbody.velocity = new Vector2(this.transform.right.x * speed, _rigidbody.velocity.y);

        // si el groundchecker no está tocando el suelo, significa que ya no hay plataforma y que
        // el gameObject debe girar 180°
        if ((!groundChecker.isTouching || blockChecker.isTouching) && !rangeVision.isTouching)
        {
            Turn();
            return;
        }
        else if ((!groundChecker.isTouching || blockChecker.isTouching) && rangeVision.isTouching)
        {
            _rigidbody.velocity = Vector2.zero;

            if(this.transform.right == Vector3.right)
            {
                if (HeroController.SharedInstance.transform.position.x < this.transform.position.x)
                {
                    Turn();
                }
            }
            else
            {
                if (HeroController.SharedInstance.transform.position.x > this.transform.position.x)
                {
                    Turn();
                }
            }
            return;
        }

        if (rangeVision.isTouching) 
        {
            hellGatoState = HellGatoState.ChasePlayer;
        }
    }

    private void Turn()
    {
        // si está viendo a la derecha
        if (this.transform.right== Vector3.right)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = true;
            animatorController.Unpause();
        }

        if (collision.gameObject.CompareTag("MainCamera") && hellGatoState == HellGatoState.Inactive)
        {
            hellGatoState = HellGatoState.WalkInTransformRight;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = false;
            _rigidbody.velocity = Vector2.zero;
            animatorController.Pause();
        }
    }

    void ITargetCombat.TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, 100);
        damageFeedbackEffect.PlayDamageEffect();

        if (health <= 0)
        {
            if (destructionPrefab)
            {
                Instantiate(destructionPrefab, (Vector2)this.transform.position + Vector2.up, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
