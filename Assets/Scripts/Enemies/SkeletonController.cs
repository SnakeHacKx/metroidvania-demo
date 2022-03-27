using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour, ITargetCombat
{
    // Máquina de estados del autómata
    public enum SkeletonState
    {
        Inactive,
        Rise,
        WalkInTransformRight,
        Turn
    }

    [SerializeField] float health = 1;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] SkeletonState skeletonState = SkeletonState.Inactive;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] LayerChecker groundChecker;
    [SerializeField] LayerChecker blockChecker;

    [SerializeField] private float speed = 1;

    private Rigidbody2D _rigidbody;

    private bool active;
    private bool isExecutingState = false;

    private void Awake()
    {
        skeletonState = SkeletonState.Inactive;
        animatorController.Pause();

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (active)
        {
            if (skeletonState == SkeletonState.Rise && !isExecutingState)
            {
                Rise();
                isExecutingState = true;
            }

            if(skeletonState == SkeletonState.WalkInTransformRight)
            {
                WalkInTransformRight();
            }
        }
    }

    private void WalkInTransformRight()
    {
        animatorController.Play(AnimationID.Walk);

        //_rigidbody.velocity = transform.right * speed;
        _rigidbody.velocity = new Vector2(transform.right.x * speed, _rigidbody.velocity.y);

        // si el groundchecker no está tocando el suelo, significa que ya no hay plataforma y que
        // el gameObject debe girar 180°
        if (!groundChecker.isTouching || blockChecker.isTouching)
        {
            Turn();
        }
    }

    private void Turn()
    {
        // si está viendo a la derecha
        if(this.transform.right == Vector3.right)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Rise()
    {
        StartCoroutine(_Rise());
    }

    IEnumerator _Rise()
    {
        //yield return new WaitForSeconds(0.2f);

        animatorController.Play(AnimationID.Rise);

        yield return new WaitForSeconds(0.55f);

        skeletonState = SkeletonState.WalkInTransformRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = true;
            animatorController.Unpause();
        }

        if (collision.gameObject.CompareTag("MainCamera") && skeletonState == SkeletonState.Inactive)
        {
            skeletonState = SkeletonState.Rise;
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

        if(health <= 0)
        {
            if (destructionPrefab)
            {
                Instantiate(destructionPrefab, (Vector2) this.transform.position + Vector2.up, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
