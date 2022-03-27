using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector2 lastCheckpointPos/* = new Vector2(35.53f, 1.97f)*/;
    private bool gamePaused = false;
    private PowerUpID currentPowerUpID;
    private int powerUpAmount = 0;
    private bool continueWasPressed = false;
    private bool newGameWasPressed = false;
    private bool inInventoryScreen;

    public Vector3 newGamePosition;

    public bool InInventoryScreen { get { return inInventoryScreen; } set { inInventoryScreen = value; } }

    public bool GamePaused { get { return gamePaused; } set { gamePaused = value; } }

    public SceneID lastCheckpointScene;

    // Guarda la referencia del siguiente escena a la que se quiere ir
    public StartAndTeleportPointsID nextUuid;

    [SerializeField] GameObject player;

    private static GameManager _sharedInstance;

    private void OnEnable()
    {
        TitleScreenController.NewGameFromTitleScreen += HandleNewGame;
        TitleScreenController.ContinueGameFromTitleScreen += HandleContinueGame;
        HeroController.HeroExists += HandleHeroIsNotNull;
        TitleScreenController.IsInMainMenu += SetActiveHeroAndCamera;
    }

    private void OnDisable()
    {
        TitleScreenController.NewGameFromTitleScreen -= HandleNewGame;
        TitleScreenController.ContinueGameFromTitleScreen -= HandleContinueGame;
        HeroController.HeroExists -= HandleHeroIsNotNull;
        TitleScreenController.IsInMainMenu -= SetActiveHeroAndCamera;
    }

    public static GameManager SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<GameManager>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    /*var gameObj = new GameObject("GameManager");
                    gameObj.AddComponent<GameManager>();
                    _sharedInstance = gameObj.GetComponent<GameManager>();*/

                    var gameObj = Resources.Load("Managers/GameManager") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<GameManager>();
                }
                DontDestroyOnLoad(_sharedInstance.gameObject);
            }

            return _sharedInstance;
        }
    }

    private void Awake()
    {
        //Application.targetFrameRate = 30;
    }

    private void Start()
    {
        continueWasPressed = false;
        newGameWasPressed = false;
    }

    private void Update()
    {
        if (GamePaused || InInventoryScreen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void SetActiveHeroAndCamera(bool active)
    {
        //Debug.Log("El hero y camara se activaran?: " + active);
        player.SetActive(active);
        CameraManager.SharedInstance.SetActiveBackground(active);
    }

    void HandleHeroIsNotNull()
    {
        //Debug.Log("****** El Hero ya no es nulo **********");

        if (continueWasPressed)
            HeroController.SharedInstance.LoadPlayerStatus();
        else if (newGameWasPressed)
            HeroController.SharedInstance.SetPlayerToNewGameStatus();
    }

    void HandleContinueGame()
    {
        Debug.Log("****** Se ha dado continue desde el tittle Screen **********");

        if (HeroController.SharedInstance == null)
            print("Espectacular");

        continueWasPressed = true;

        //HeroController.SharedInstance.LoadPlayerStatus();
    }

    void HandleNewGame()
    {
        Debug.Log("Paso por handle new game");
        newGameWasPressed = true;
    }

    public void HidePauseScreen()
    {
        GamePaused = false;
    }

    public void UpdateHealth(int health)
    {
        HUDScreen.SharedInstance.UpdateHealth(health);
    }

    public void UpdateCoins(int coins)
    {
        HUDScreen.SharedInstance.UpdateCoins(coins);
    }

    public void UpdatePowerUp(int amount)
    {
        powerUpAmount = amount;
        HUDScreen.SharedInstance.UpdatePowerUp(amount);
    }

    public void UpdatePowerUp(PowerUpID powerUpID, Sprite icon, int amount)
    {
        powerUpAmount = amount;
        currentPowerUpID = powerUpID;
        HUDScreen.SharedInstance.UpdatePowerUp(icon, powerUpAmount);
    }

    // Esto es lo del curso de Metroidvania

    public void SaveGame()
    {
        SaveSystem.SharedInstance.Load(out GameData gameData);
        if (gameData == null)
        {
            gameData = new GameData();

        }
        gameData.heroData.currentPowerUpID = currentPowerUpID;
        gameData.heroData.powerUpAmount = powerUpAmount;
        gameData.heroData.coinsAmount = HeroController.SharedInstance.Coins;
        gameData.heroData.health = HeroController.SharedInstance.Health;

        //Debug.Log("Cantidad de espacio en el vector posicion del gameData: " + gameData.heroData.lastPosition.Length);
        gameData.heroData.lastPositionX = lastCheckpointPos.x;
        gameData.heroData.lastPositionY = lastCheckpointPos.y;
        gameData.heroData.lastPositionZ = 0;

        //gameData.levelData.sceneID = SceneHelper.SharedInstance.GetCurrentSceneID();
        
        gameData.levelData.lastCheckpointScene = GetLastCheckpointScene();
        lastCheckpointScene = gameData.levelData.lastCheckpointScene;
        Debug.Log("La escena en la que se ha guardado la partida es: " + GetLastCheckpointScene());

        SaveSystem.SharedInstance.Save(gameData);
        //print("SE HA GUARDADO LA PARTIDA");
        //Debug.Log("Partida guardada en la POSSSSS: (" + lastCheckpointPos.x + "," + lastCheckpointPos.y + ", 0)");
        //Debug.Log("La escena guardada es: " + gameData.levelData.lastCheckpointScene);
        //Debug.Log("La cantidad de coins guardada es: " + gameData.heroData.coinsAmount);
    }

    public Vector3 GetLastCheckpointPosition()
    {
        return new Vector3(lastCheckpointPos.x, lastCheckpointPos.y, 0);
    }

    public SceneID GetLastCheckpointScene()
    {
        Enum.TryParse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out SceneID currentScene);
        Debug.Log("La escena en donde se guardo ers: " + currentScene);    
        return currentScene;
    }

    // Esto es de YouTube
    /*public void SavePlayer()
    {
        SaveSystem.SavePlayer(HeroController.SharedInstance);
    }

    public void LoadPlayer()
    {
        HeroData data = SaveSystem.LoadPlayer();
        HeroController.SharedInstance.Health = data.health;
        HeroController.SharedInstance.PowerUpAmount = data.powerUpAmount;
        HeroController.SharedInstance.Coins = data.coinsAmount;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        HeroController.SharedInstance.transform.position = position;
    }*/
}
