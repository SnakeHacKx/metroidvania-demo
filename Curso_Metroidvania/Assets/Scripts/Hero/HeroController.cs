using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour, ITargetCombat
{
    public delegate void _HeroExists();
    public static event _HeroExists HeroExists;

    [Header("Power Ups")]
    [SerializeField] private PowerUpID currentPowerUp;
    [SerializeField] SpellLauncherController bluePotionLauncher;
    [SerializeField] SpellLauncherController redPotionLauncher;
    [SerializeField] private int _powerUpAmount;

    public int PowerUpAmount
    {
        get
        {
            return _powerUpAmount;
        }
        set
        {
            if (_powerUpAmount != value)
            {
                //print("Debio actualizarse el numero de power ups");
                GameManager.SharedInstance.UpdatePowerUp(value);
            }
            _powerUpAmount = value;
        }
    }

    [Header("Attack Variables")]
    [SerializeField] SwordController swordController;
    [SerializeField] float delayAttack;
    [SerializeField] float attackDuration;

    [Header("Health Variables")]
    // Patrón del observador
    [SerializeField] private int _initialHealh;
    [SerializeField] private int _health = 0; // variable de apollo

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            if (_health != value)
            {
                GameManager.SharedInstance.UpdateHealth(value);
            }
            _health = value;
        }
    }

    [SerializeField] private DamageFeedbackEffect damageFeedbackEffect;

    [Header("Animation Variables")]
    [SerializeField] private AnimatorController animatorController;

    [Header("Checker Variables")]
    [SerializeField] private LayerChecker footA;
    [SerializeField] private LayerChecker footB;

    [Header("Interruption Variables")]
    public bool canCheckGround;
    public bool canMove;
    public bool canFlip;

    [Header("Boolean Variables")]
    public bool canDoubleJump;
    public bool playerIsAttacking;
    public bool playerIsRecovering;
    public bool isLookingRight;
    public bool playerIsUsingPowerUp;
    public bool playerIsFalling;

    [Header("Rigid Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float damageForce; // fuerza de repulsión cuando somos golpeados
    [SerializeField] private float damageForceUp;
    [SerializeField] private float spikeDamageForceUp;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip healthUpSFX;

    // Control Variables
    [SerializeField] private Vector2 movementDirection;
    private bool jumpPressed = false;
    private bool attackPressed = false;
    private bool usePowerUpPressed = false;
    private bool playerIsOnGround;
    private bool isJumping;
    private bool playerDied;
    private bool isControlable = true;
    private bool playerIsTakingDamage = false;
    public bool isTalking;

    private Vector3 lastPositionOnGround;

    public Vector3 LastPositionOnGround { get { return lastPositionOnGround; } set { lastPositionOnGround = value; } }

    [Tooltip("Tiempo máximo en el que será válido que el usuario presione el botón de saltar")]
    [SerializeField] private float jumpTime;
    private float jumpTimeCounter;

    private int _coins = -1;

    public int Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            if (_coins != value)
            {
                GameManager.SharedInstance.UpdateCoins(value);
            }
            _coins = value;
        }
    }

    private Rigidbody2D _rigidbody;

    GameData gameData;

    SpikeSurroundings spikesSurroundings;

    // Permite saber a todo el programa que el jugador ya ha sido creado, esto
    // para que al cargar más de una escena, no se creen clones de este
    public static bool playerCreated;

    private static HeroController _sharedInstance;

    public static HeroController SharedInstance
    {
        get
        {
            // Debug.Log("Posición del player en su instancia: " + _sharedInstance.transform.position);
            if (_sharedInstance == null)
            {

                _sharedInstance = FindObjectOfType<HeroController>();
                //DontDestroyOnLoad(_sharedInstance);
            }
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    private void Awake()
    {
        if (!playerCreated)
        {
            DontDestroyOnLoad(this.transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        ////if (_sharedInstance != null /*&& _sharedInstance != this*/)
        ////{
        ////    Destroy(this.gameObject);

        ////    // anadí esto, no se si funcione
        ////    _sharedInstance = this;
        ////    //DontDestroyOnLoad(gameObject);
        ////}

        //Health = initialHealh;
    }

    LevelManager levelManager;

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        isTalking = false;

        //Debug.Log("PASO POR START");

        //SaveSystem.SharedInstance.Load(out gameData);
        // Si no se ha creado el jugador, "se crea"
        if (!playerCreated)
        {
            playerCreated = true;
        }

        canCheckGround = true;
        canMove = true;
        canFlip = true;
        _rigidbody = GetComponent<Rigidbody2D>();
        animatorController.Play(AnimationID.Idle);

        spikesSurroundings = FindObjectOfType<SpikeSurroundings>();

        HeroExists?.Invoke();
    }

    private void Update()
    {
        //Debug.Log("[HeroController] NEXT UUID = " + nextUuid);
        if (!playerDied && isControlable)
        {
            HandleIsGrounding();
            HandleControls();
            HandleFalling();
            HandleFlip();
            HandleAttack();
            HandleJump();
            HandleUsePowerUp();
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("La posición real del player es: " + this.transform.position);
        if (isTalking)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        if (!playerDied && isControlable)
        {
            HandleMovement();
        }
    }

    public void LoadPlayerStatus()
    {
        if (gameData == null)
            return;

        Debug.Log("La vida del jugador en la BD es: " + gameData.heroData.health.ToString());
        Health = gameData.heroData.health;
        Coins = gameData.heroData.coinsAmount;
        PowerUpAmount = gameData.heroData.powerUpAmount;
        currentPowerUp = gameData.heroData.currentPowerUpID;
    }

    /*public void StopPlayerMovement()
    {
        SetIsControlable(false);
    }*/

    public void SetIdleAnimToPlayer()
    {
        animatorController.Play(AnimationID.Idle);
    }

    public void SetPlayerToNewGameStatus()
    {
        Invoke(nameof(PutOnNewGamePosition), 0.2f);
        Invoke(nameof(SetPlayerToDefaltAtributes), 0.3f);
        SetIsControlable(true);
        //SceneHelper.SharedInstance.LoadScene(SceneID.check1_1);
        //this.transform.position = GameManager.SharedInstance.lastCheckpointPos;
        Health = 10;
        Coins = 0;
        PowerUpAmount = 0;
        currentPowerUp = PowerUpID.Nothing;
        
        //Debug.Log("LA VIDA EN NEW GAME ES DE: " + Health.ToString());
    }

    void PutOnNewGamePosition()
    {
        this.transform.position = GameManager.SharedInstance.newGamePosition;
        Debug.Log("sE INCIO EN LA POSOICION: " + GameManager.SharedInstance.newGamePosition);
    }

    public void SetPlayerToDefaultStatus()
    {
        Invoke(nameof(PutOnLastCheckpointPosition), 0.2f);
        Invoke(nameof(SetPlayerToDefaltAtributes), 0.3f);
        SetIsControlable(true);
        Health = 10; // Esto se debe cambiar cuando tenga metodos para cambiar la vida MAXIMA
        Coins = 0;
        PowerUpAmount = 0;
        animatorController.Play(AnimationID.Idle);
        currentPowerUp = PowerUpID.Nothing;
    }

    public void SetPlayerToDefaltAtributes()
    {
        playerDied = false;
        playerIsAttacking = false;
        playerIsFalling = false;
        playerIsRecovering = false;
        playerIsTakingDamage = false;
        Debug.Log("is taking damage se hizo falso");
        playerIsUsingPowerUp = false;
        isTalking = false;
        isJumping = false;
        canMove = true;
        canFlip = true;
    }

    private void PutOnLastCheckpointPosition()
    {   
        this.transform.position = GameManager.SharedInstance.GetLastCheckpointPosition();
        TeleportVirtualCamera.SharedInstance.ChangePosition(GameManager.SharedInstance.GetLastCheckpointPosition());
        Debug.Log("Posición de reaparición: " + GameManager.SharedInstance.GetLastCheckpointPosition());
    }

    public void PutOutBoundaries()
    {
        canFlip = false;
        this.transform.position = new Vector3(-15, 0, 0);
        _rigidbody.velocity = Vector2.zero;
    }

    public void PutOnSpawnPosition(Vector2 position)
    {
        canFlip = true;
        this.transform.position = position;
    }

    public void SetIsControlable(bool isControlable)
    {
        this.isControlable = isControlable;

        if (!this.isControlable)
        {
            StopAllCoroutines();
            animatorController.Play(AnimationID.Idle);
            _rigidbody.velocity = Vector2.zero;
        }
    }

    public void SetPlayerIsDead(bool isDead)
    {
        playerDied = isDead;

        /*if (playerDied)
        {
            StopAllCoroutines();
            animatorController.Play(AnimationID.Idle);
            _rigidbody.velocity = Vector2.zero;
        }*/
    }

    public void GiveCoin()
    {
        Coins = Mathf.Clamp(Coins + 1, 0, 10);
        if (Coins == 10)
        {
            AudioManager.SharedInstance.PlaySFX(healthUpSFX);
            GiveHealthPoint();
            Coins = 0;
        }
    }

    public void GiveHealthPoint()
    {
        Health = Mathf.Clamp(Health + 1, 0, 10);
    }

    public void ChangePowerUp(PowerUpID powerUpID, int amount)
    {
        currentPowerUp = powerUpID;
        PowerUpAmount = amount;
        // Debug.Log(currentPowerUp);
    }

    private void HandleControls()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // GetButton vale sólamente para un fotograma
        // necesitamos el GetButtonDown para que sea mientras presionamos la tecla
        jumpPressed = Input.GetButtonDown("Jump"); // Por default es la barra espaciadora
        attackPressed = Input.GetButtonDown("Attack");
        usePowerUpPressed = Input.GetButtonDown("UsePowerUp");
    }

    private void HandleMovement()
    {
        if (!canMove) return;

        _rigidbody.velocity = new Vector2(movementDirection.x * speed, _rigidbody.velocity.y);

        if (playerIsOnGround)
        {
            if (Mathf.Abs(_rigidbody.velocity.x) > 0) // se está moviendo?
            {
                // AQUI SE DEBERÍA AGREGAR UNA ANIMACION y COMPORTAMIENTO DE CUANDO ESTA ATACANDO
                // Y CORRIENDO A LA MISMA VEZ
                if (!playerIsAttacking) animatorController.Play(AnimationID.Run);
            }
            else
            {
                if (!playerIsAttacking) animatorController.Play(AnimationID.Idle);
            }
        }
        else if (playerIsFalling && !playerIsAttacking) 
        { 
            animatorController.Play(AnimationID.Jump);
        }
    }

    private void HandleFalling()
    {
        if(!playerIsOnGround && !isJumping)
        {
            playerIsFalling = true;
        }
        
        if(playerIsOnGround || isJumping)
        {
            playerIsFalling = false;
        }
    }

    private void HandleFlip()
    {
        if (!canFlip) return;

        if (_rigidbody.velocity.magnitude > 0)
        {
            if (_rigidbody.velocity.x > 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                isLookingRight = true;
            }
            else if (_rigidbody.velocity.x < 0)
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                isLookingRight = false;
            }
        }
    }

    private void HandleJump()
    {
        // Doble Salto
        if(canDoubleJump && jumpPressed && !playerIsOnGround)
        {
            this._rigidbody.velocity = Vector2.zero;
            this._rigidbody.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }

        // Salto normal
        if (jumpPressed && playerIsOnGround)
        {
            isJumping = true;

            jumpTimeCounter = jumpTime;

            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Comienza la corrutina del salto
            StartCoroutine(HandleJumpAnimation());

            canDoubleJump = true; 
        }

        if (jumpTimeCounter > 0 && isJumping == true)
        {
            _rigidbody.velocity = Vector2.up * jumpForce;
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    IEnumerator HandleJumpAnimation()
    {
        canCheckGround = false;
        playerIsOnGround = false;
        playerIsFalling = false;

        if (!playerIsAttacking)
            animatorController.Play(AnimationID.PrepareJump);

        yield return new WaitForSeconds(0.1f); // retraso de 0.3 segundos

        if (!playerIsAttacking)
            animatorController.Play(AnimationID.Jump);

        canCheckGround = true;
    }

    private void HandleIsGrounding()
    {
        if (!canCheckGround) return;

        playerIsOnGround = footA.isTouching || footB.isTouching;
    }

    private void HandleAttack()
    {
        if (attackPressed && !playerIsAttacking)
        {
            if(playerIsOnGround) _rigidbody.velocity = Vector2.zero;

            AudioManager.SharedInstance.PlaySFX(attackSFX);
            animatorController.Play(AnimationID.Attack);
            playerIsAttacking = true;
            swordController.Attack(delayAttack, attackDuration);

            StartCoroutine(RestoreAttack());
        }
    }

    IEnumerator RestoreAttack()
    {
        if (playerIsOnGround)
            canMove = false;

        yield return new WaitForSeconds(0.35f); // El ataque durará 3s
        playerIsAttacking = false;
        
        canMove = true;
    }

    public void UpdatePosition(Vector2 position)
    {
        //Debug.Log("Debio teletransportame a: " + position);
        _rigidbody.velocity = Vector2.zero;
        this.transform.position = position;

        
        //Debug.Log("La velocidad del jugador debería ser 0");
    }

    private void HandleUsePowerUp()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button1)) && !playerIsUsingPowerUp && currentPowerUp != PowerUpID.Nothing)
        {
            if (playerIsOnGround) _rigidbody.velocity = Vector2.zero;

            AudioManager.SharedInstance.PlaySFX(attackSFX); //change
            animatorController.Play(AnimationID.UsePowerUp);
            playerIsUsingPowerUp = true;

            if(currentPowerUp == PowerUpID.BluePotion)
            {
                // el tranform.right es en realidad el eje X, o sea, la flecha roja que aparece en unity
                bluePotionLauncher.Launch((Vector2)this.transform.right + Vector2.up * 0.3f);
            }

            if (currentPowerUp == PowerUpID.RedPotion)
            {
                redPotionLauncher.Launch(this.transform.right);

            }


            StartCoroutine(RestoreUsePowerUp());

            PowerUpAmount--;

            if(PowerUpAmount <= 0)
            {
                currentPowerUp = PowerUpID.Nothing;
            }
        }
    }

    IEnumerator RestoreUsePowerUp()
    {
        if (playerIsOnGround)
            canMove = false;

        yield return new WaitForSeconds(0.4f); // El ataque durará 3s
        playerIsUsingPowerUp = false;

        canMove = true;
    }

    public void TakeDamage(int damagePoints)
    {
        if (!playerIsRecovering && !playerDied && !playerIsTakingDamage)
        {
            playerIsTakingDamage = true;
            //Debug.LogFormat("1. Me han hecho {0} puntos de daño...", damagePoints);
            // Clamp evita que se haga negativa la variable
            Health = Mathf.Clamp(Health - damagePoints, 0, 10);

            if (Health <= 0)
            {
                playerDied = true;
                SetIsControlable(false);
                _rigidbody.velocity = Vector2.zero;
                // Aqui debería llamar a alguna corrutina de muerte

                if (GameManager.SharedInstance.lastCheckpointScene == 0)
                {
                    Debug.Log("Mori y se reinico el juego");
                    StopAllCoroutines();
                    SceneHelper.SharedInstance.RestartGame();
                    return;
                }
                
                SceneHelper.SharedInstance.GoToLastCheckPoint();
            }
            
            //Debug.LogFormat("2. Se empezara a recobrar el player, no debería recibir daño");
            StartCoroutine(StartPlayerRecover());
            DamageImpulse();

            playerIsTakingDamage = false;
            //Debug.LogFormat("3. Ahora sí puedo recibir daño");
        }
        //if (Health == 0) Destroy(this.gameObject);
    }

    public void TakeSpikeDamage(int damagePoints)
    {
        Debug.LogFormat("El player se esta recobrando?: {0}", playerIsRecovering);
        Debug.LogFormat("El player esta muerto?: {0}", playerDied);
        Debug.LogFormat("AL player le estan haciendo dano?: {0}", playerIsTakingDamage);
        if (!playerIsRecovering && !playerDied && !playerIsTakingDamage)
        {
            Debug.Log("El player ha sufrido dano por espinas");
            playerIsTakingDamage = true;
            // Clamp evita que se haga negativa la variable
            Health = Mathf.Clamp(Health - damagePoints, 0, 10);

            if (Health <= 0)
            {
                playerDied = true;
                SetIsControlable(false);
                _rigidbody.velocity = Vector2.zero;
                // todo: Hacer una corrutina de muerte

                if (GameManager.SharedInstance.lastCheckpointScene == 0)
                {
                    Debug.Log("Mori y se reinicio el juego");
                    StopAllCoroutines();
                    SceneHelper.SharedInstance.RestartGame();
                    return;
                }

                SceneHelper.SharedInstance.GoToLastCheckPoint();
                Debug.Log("Mori y spawnee en el ultimo checkpoint...");
                return;
            }
            PlayerTakeSpikeDamage();
            playerIsTakingDamage = false;
            Debug.Log("En este momento me pueden volver a hacer dano");
            SpikeDamageTeleport.FindObjectOfType<SpikeDamageTeleport>().SendPlayerToRespawnPosition();
        }
    }

    void DamageImpulse()
    {
        if (isLookingRight)
        {
            _rigidbody.AddForce(Vector2.left * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * damageForce + Vector2.up * damageForceUp, ForceMode2D.Impulse);
        }
    }

    void SpikeDamageImpulse()
    {
        if (isLookingRight)
        {
            _rigidbody.AddForce(Vector2.left * damageForce + Vector2.up * spikeDamageForceUp, ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * damageForce + Vector2.up * spikeDamageForceUp, ForceMode2D.Impulse);
        }
    }

    public void PlayerTakeSpikeDamage()
    {
        StartCoroutine(PlayerTakeSpikeDamageCoroutine());
    }

    public IEnumerator PlayerTakeSpikeDamageCoroutine()
    {
        canMove = false;
        canFlip = false;
        animatorController.Play(AnimationID.Hurt);
        SpikeDamageImpulse();
        yield return new WaitForSeconds(0.5f);
        animatorController.Play(AnimationID.Idle);
        canMove = true;
        canFlip = true;
        //_rigidbody.velocity = Vector2.zero;
        playerIsRecovering = true;
        Debug.Log("EL player empezo a recuperarse");

        damageFeedbackEffect.PlayBlinkDamageEffect();
        yield return new WaitForSeconds(2f);
        damageFeedbackEffect.StopBlinkDamageEffect();

        playerIsRecovering = false;
        Debug.Log("EL player termino de recuperarse");
        
    }

    IEnumerator StartPlayerRecover()
    {
        // esto es más que todo para que el impulso (el que pasa cuando nos hacen daño) tenga efecto
        canMove = false;
        canFlip = false;
        animatorController.Play(AnimationID.Hurt);
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        canFlip = true;
        _rigidbody.velocity = Vector2.zero;

        playerIsRecovering = true;
        damageFeedbackEffect.PlayBlinkDamageEffect();
        yield return new WaitForSeconds(2);
        damageFeedbackEffect.StopBlinkDamageEffect();
        playerIsRecovering = false;
    }
}