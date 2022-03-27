using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroMaliusController : MonoBehaviour, ITargetCombat
{

    public enum AndroiMaliusState
    {
        Presentation,
        Teleport,
        FadeIn,
        MoveInScenario,
        LaunchProjectiles,
        LaunchThunders,
        Die
    }
    [Header("Animation")]

    [SerializeField] AnimatorController animatorController;
    [SerializeField] GameObject dieEffectPrefab;
    [SerializeField] GameObject rewardPrefab;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject thunderPrefab;
    [SerializeField] GameObject projectilePivot;
    [SerializeField] float projectileForce;


    [Header("Visual Effects")]
    [SerializeField] GameObject feedbackProjectile;

    [SerializeField] AlphaBlinkEffect alphaBlinkEffect;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;
    [Header("WayPoints")]

    [SerializeField] WaypointsManager wayPointsManager;

    [Header("Boss States")]
    public AndroiMaliusState androiMaliusState = AndroiMaliusState.Presentation;
    [Header("Health")]

    [SerializeField] int health = 30;
    [SerializeField] int healthMax = 30;

    [Header("Boss States")]
    [SerializeField] bool debugMode = false;


    [Header("Rigid Variables")]
    [SerializeField] float moveSpeed = 4;
    [SerializeField] AudioClip dieSfx;


    [SerializeField] private bool stateExecuted = false;
    private Rigidbody2D _rigidbody2D;

    private bool canFloat = false;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (!debugMode)
        {
            androiMaliusState = AndroiMaliusState.Presentation;
        }
        canFloat = true;
    }

    void Update()
    {
        if (androiMaliusState != AndroiMaliusState.Die)
        {

            if (!stateExecuted && androiMaliusState == AndroiMaliusState.Presentation)
            {
                Presentation();
                stateExecuted = true;
            }
            if (!stateExecuted && androiMaliusState == AndroiMaliusState.Teleport)
            {
                Teleport();
                stateExecuted = true;
            }
            if (!stateExecuted && androiMaliusState == AndroiMaliusState.FadeIn)
            {
                FadeIn();
                stateExecuted = true;
            }

            if (!stateExecuted && androiMaliusState == AndroiMaliusState.MoveInScenario)
            {
                MoveInScenario();
                stateExecuted = true;
            }

            if (!stateExecuted && androiMaliusState == AndroiMaliusState.LaunchProjectiles)
            {
                LaunchProjectiles();
                stateExecuted = true;
            }

            if (!stateExecuted && androiMaliusState == AndroiMaliusState.LaunchThunders)
            {
                LaunchThunders();
                stateExecuted = true;
            }

            if (androiMaliusState != AndroiMaliusState.MoveInScenario && canFloat)
            {
                _rigidbody2D.velocity = new Vector2(0, Mathf.Cos(Time.time));
            }
        }
    }

    void LaunchThunders()
    {
        StartCoroutine(_LaunchThunders());

    }

    IEnumerator _LaunchThunders()
    {

        yield return new WaitForSeconds(0.4f);
        animatorController.Play(AnimationID.Idle);
        var direction = new Vector3(-38, -23, 0) - this.transform.position;

        canFloat = false;
        while (Vector2.Distance(new Vector3(-38, -23, 0), this.transform.position) > 0.3F)
        {
            _rigidbody2D.velocity = direction.normalized * moveSpeed;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        var positionX = HeroController.SharedInstance.transform.position.x;
        yield return new WaitForSeconds(0.4f);

        canFloat = true;

        for (int i = 0; i < 10; i++)
        {


            if (i == 5)
            {
                positionX += 4;
            }
            var thunder = Instantiate(thunderPrefab, new Vector3(positionX + i * 3, -14.84f, 0), Quaternion.identity);

            thunder.GetComponent<RedThunderController>().LaunchThunder();
            //yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(6f);

        if (Random.Range(-10, 10) > 0)
        {
            androiMaliusState = AndroiMaliusState.Teleport;

        }
        else
        {
            androiMaliusState = AndroiMaliusState.LaunchProjectiles;

        }


        stateExecuted = false;
    }

    void LaunchProjectiles()
    {

        StartCoroutine(_LaunchProjectiles());
    }

    IEnumerator _LaunchProjectiles()
    {
        yield return new WaitForSeconds(0.4f);
        animatorController.Play(AnimationID.LookAtTarget);

        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < 5; i++)
        {
            feedbackProjectile.SetActive(true);

            var direction = (HeroController.SharedInstance.transform.position - this.transform.position).normalized;
            HandleFlip(direction.x);

            yield return new WaitForSeconds(0.6f);

            animatorController.Play(AnimationID.Attack);
            yield return new WaitForSeconds(0.2f);

            var projectile = Instantiate(projectilePrefab, projectilePivot.transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.3f);

            animatorController.Play(AnimationID.LookAtTarget);
            feedbackProjectile.SetActive(false);

            yield return new WaitForSeconds(1f);

        }
        feedbackProjectile.SetActive(false);

        yield return new WaitForSeconds(1f);

        if (Random.Range(-10, 10) > 0)
        {
            androiMaliusState = AndroiMaliusState.Teleport;

        }
        else
        {
            androiMaliusState = AndroiMaliusState.MoveInScenario;

        }


        stateExecuted = false;

    }

    private void MoveInScenario()
    {
        StartCoroutine(MoveInScenarioCoroutine());
    }

    IEnumerator MoveInScenarioCoroutine()
    {
        animatorController.Play(AnimationID.LookAtTarget);
        var target = wayPointsManager.GetRandomPoint();

        yield return new WaitForSeconds(0.4f);

        var direction = (target - (Vector2)this.transform.position).normalized;
        HandleFlip(direction.x);

        while (Vector2.Distance(this.transform.position, target) > 2f)
        {
            _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed + Mathf.Cos(Time.time));
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(1f);

        if (health < (int)(healthMax * 0.5f))
        {
            androiMaliusState = AndroiMaliusState.LaunchThunders;

        }
        else
        {
            androiMaliusState = AndroiMaliusState.LaunchProjectiles;

        }
        animatorController.Play(AnimationID.Idle);

        stateExecuted = false;


    }

    void HandleFlip(float x)
    {
        if (x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.identity;
        }

    }

    void FadeIn()
    {
        StartCoroutine(_FadeIn());
    }

    IEnumerator _FadeIn()
    {
        alphaBlinkEffect.PlayBlinkEffect();

        yield return new WaitForSeconds(2);
        alphaBlinkEffect.BecomeVisible();
        yield return new WaitForSeconds(2);

        if (Random.Range(-10, 10) > 0)
        {
            androiMaliusState = AndroiMaliusState.MoveInScenario;

        }
        else
        {
            androiMaliusState = AndroiMaliusState.LaunchProjectiles;

        }
        stateExecuted = false;


    }

    void Teleport()
    {
        StartCoroutine(_Teleport());
    }

    IEnumerator _Teleport()
    {
        yield return new WaitForSeconds(1);

        alphaBlinkEffect.PlayBlinkEffect();
        yield return new WaitForSeconds(2);
        alphaBlinkEffect.BecomeInvisible();
        var teleportPoint = wayPointsManager.GetRandomPoint();

        this.transform.position = teleportPoint;

        androiMaliusState = AndroiMaliusState.FadeIn;
        stateExecuted = false;
    }

    void Presentation()
    {
        StartCoroutine(_Presentation());
    }

    IEnumerator _Presentation()
    {

        alphaBlinkEffect.PlayBlinkEffect();
        yield return new WaitForSeconds(5);
        alphaBlinkEffect.StopBlinkEffect();
        androiMaliusState = AndroiMaliusState.Teleport;
        stateExecuted = false;
    }

    public void TakeDamage(int damagePoints)
    {
        health = Mathf.Clamp(health - damagePoints, 0, healthMax);
        damageFeedbackEffect.PlayBlinkDamageEffect(0.8f);
        if (health <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        damageFeedbackEffect.PlayBlinkDamageEffect();

        _rigidbody2D.velocity = Vector3.zero;
        androiMaliusState = AndroiMaliusState.Die;
        AudioManager.SharedInstance.PlayMusic(null);
        yield return new WaitForSeconds(1);

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(rewardPrefab, this.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0), Quaternion.identity);

                Instantiate(dieEffectPrefab, this.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));

            }
            yield return new WaitForSeconds(0.3f);

        }
        AudioManager.SharedInstance.PlaySFX(dieSfx);

        FindObjectOfType<LevelManager>().FinalBossWasVanquished();
        gameObject.SetActive(false);
    }


}