using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour, ITargetCombat
{

    // Máquina de estados del autómata
    public enum GhostState
    {
        Inactive,
        Patrol,
        ChasePlayer
    }

    [SerializeField] WaypointsManager waypointsManager;
    [SerializeField] float health = 2;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [SerializeField] GhostState ghostState = GhostState.Inactive;
    [SerializeField] GameObject destructionPrefab;

    [SerializeField] LayerChecker rangeVision;

    [SerializeField] private float speed = 1;

    private Rigidbody2D _rigidbody;

    private bool active;
    // private bool isExecutingState = false;

    private Vector2 currentWaypoint;

    private QuestEnemy questEnemy; // esto posiblemente se lleve a un futuro manager de la salud y se elimine la interfaz
    private QuestManager questManager;

    private void Awake()
    {
        currentWaypoint = waypointsManager.GetRandomPoint();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        questEnemy = GetComponent<QuestEnemy>();
        questManager = FindObjectOfType<QuestManager>();
    }

    private void Update()
    {
        if (active)
        {
            if(ghostState == GhostState.Patrol)
            {
                Patrol();
            }

            if (ghostState == GhostState.ChasePlayer)
            {
                ChasePlayer();
            }
        }
    }

    private void ChasePlayer()
    {
        if (HeroController.SharedInstance != null)
            MoveTo(HeroController.SharedInstance.transform.position);

        if (!rangeVision.isTouching)
        {
            ghostState = GhostState.Patrol;
        }
    }

    // Invento mío, la función mueve al jugador segun lo que le pase por parámetro
    private void MoveTo(Vector2 target)
    {
        var direction = target - (Vector2) this.transform.position;

        if (direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        _rigidbody.velocity = direction.normalized * speed;
    }

    private void Patrol()
    {
        MoveTo(currentWaypoint);

        // Distance: distancia entre los vectores
        if (Vector2.Distance(currentWaypoint, this.transform.position) < 0.1f)
        {
            currentWaypoint = waypointsManager.GetRandomPoint();
        }

        if (rangeVision.isTouching)
        {
            ghostState = GhostState.ChasePlayer;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = true;
        }

        if (collision.gameObject.CompareTag("MainCamera") && ghostState == GhostState.Inactive)
        {
            ghostState = GhostState.Patrol;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            active = false;
            _rigidbody.velocity = Vector2.zero;
        }
    }

    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, 100);
        damageFeedbackEffect.PlayDamageEffect();

        if (health <= 0)
        {
            if (destructionPrefab)
            {
                Instantiate(destructionPrefab, (Vector2)this.transform.position + Vector2.up, Quaternion.identity);
            }

            questManager.lastEnemyKilled = questEnemy;

            Destroy(this.gameObject);
        }
    }
}
