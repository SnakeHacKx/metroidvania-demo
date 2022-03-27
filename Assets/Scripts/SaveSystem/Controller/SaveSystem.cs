using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("Debug")]
    public bool save;
    public bool load;

    [SerializeField] GameData gameData;
    [SerializeField] HeroData heroData;
    [SerializeField] LevelData levelData;

    private const string gameName = "gothic1";

    private GameModel gameModel = new GameModel();
    private LevelModel levelModel = new LevelModel();
    private HeroModel heroModel = new HeroModel();

    private static SaveSystem _sharedInstance;

    public static SaveSystem SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {

                _sharedInstance = FindObjectOfType<SaveSystem>();

                GameObject gameO = null;
                if (_sharedInstance == null)
                {
                    gameO = new GameObject("SaveSystem");
                    gameO.AddComponent<SaveSystem>();
                    _sharedInstance = gameO.GetComponent<SaveSystem>();

                }
                DontDestroyOnLoad(_sharedInstance.gameObject);
            }
            return _sharedInstance;

        }

    }



    public void Save(GameData data)
    {
        data.gameName = gameName;
        gameData = data;
        gameModel.Save(data);
    }

    public void Save(LevelData data)
    {
        levelData = data;

        levelModel.Save(gameName, data);
    }

    public void Save(HeroData data)
    {
        heroData = data;

        heroModel.Save(gameName, data);
    }

    public void Load(out GameData data)
    {
        data = new GameData();
        data.gameName = gameName;
        data = gameModel.Load(data);
        if (data.levelData == null && data.heroData == null)
        {
            data = null;
        }
        gameData = data;
    }

    public void Load(out LevelData data)
    {
        data = new LevelData();
        data = levelModel.Load(gameName);
        levelData = data;
    }
    public void Load(out HeroData data)
    {
        data = new HeroData();
        data = heroModel.Load(gameName);
        heroData = data;
    }

    private void Update()
    {
        if (save)
        {
            save = false;
            Save(gameData);
        }
        if (load)
        {
            load = false;
            Load(out gameData);
        }
    }

}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] GameData gameData;
    [SerializeField] HeroData heroData;
    [SerializeField] LevelData levelData;

    private const string GAME_NAME = "Gothic1";


    private GameModel gameModel = new GameModel();
    private LevelModel levelModel = new LevelModel();
    private HeroModel heroModel = new HeroModel();

    private static SaveSystem _sharedInstance;

    public static SaveSystem SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<SaveSystem>();

                GameObject gameSaveSystem;

                if (_sharedInstance == null)
                {
                    gameSaveSystem = new GameObject("SaveSystem");
                    gameSaveSystem.AddComponent<SaveSystem>();
                    _sharedInstance = gameSaveSystem.GetComponent<SaveSystem>();
                    DontDestroyOnLoad(gameSaveSystem);
                }
            }

            return _sharedInstance;
        }
    }

    public void Save(GameData data)
    {
        data.gameName = GAME_NAME;
        gameModel.Save(data);
    }

    public void Save(LevelData data)
    {
        levelModel.Save(GAME_NAME, data);
    }

    public void Save(HeroData data)
    {
        heroModel.Save(GAME_NAME, data);
    }

    // TO LOAD

    // out: parametro de salida, funciona como un dato por referencia
    public void Load(out GameData data)
    {
        data = new GameData();
        data.gameName = GAME_NAME;
        data = gameModel.Load(data);

        if (data.levelData == null && data.heroData == null)
        {
            data = null;
        }

        gameData = data;
    }

    public void Load(out LevelData data)
    {
        data = new LevelData();
        data = levelModel.Load(GAME_NAME);
        levelData = data;
    }

    public void Load(out HeroData data)
    {
        data = new HeroData();
        data = heroModel.Load(GAME_NAME);
        heroData = data; // Esto es sólo para depurar
    }
}
 */
