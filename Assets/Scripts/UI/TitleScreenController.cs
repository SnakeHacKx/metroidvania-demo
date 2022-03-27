using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenController : MonoBehaviour
{
    public delegate void _ContiueGame();
    public static event _ContiueGame ContinueGameFromTitleScreen;

    public delegate void _NewGame();
    public static event _NewGame NewGameFromTitleScreen;

    public delegate void _IsInMainMenu(bool activeHeroAndCamera);
    public static event _IsInMainMenu IsInMainMenu;

    public bool showOptions = false;

    [Header("Buttons")]
    public List<GameObject> optionButtons;
    public GameObject pressEnterButton;
    public GameObject continueButton;

    [Header("Audio")]
    [SerializeField] AudioClip enterSfx;
    [SerializeField] AudioClip buttonSfx;

    [Header("Control Menu")]
    [SerializeField] GameObject mainMenuFirstButton;
    // SI tengo otro menu dentro del menu, entonces elijo el boton que
    // quiere que se seleccione en ese nuevo menu
    // [SerializeField] GameObject optionFirstButton; 
    
    // Cuando regresemos de ese menu, querremos que el boton que llevaba a ese menu
    // es el que esté seleccionado
    // [SerializeField] GameObject optionsClosedButton;

    private GameData gameData;

    private void Awake()
    {
        IsInMainMenu?.Invoke(false);

        SaveSystem.SharedInstance.Load(out gameData);

        // limpiar objeto seleccionado (hay que hacer esto si no, da error.. cosas raras de Unity)
        EventSystem.current.SetSelectedGameObject(null);

        // Poner un nuevo objeto seleccionado
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }

    private void Update()
    {
        if (!showOptions && Input.GetKeyDown(KeyCode.Return))
        {
            showOptions = true;
            pressEnterButton.SetActive(false);
            foreach (var button in optionButtons)
            {
                button.SetActive(true);
            }
            AudioManager.SharedInstance.PlaySFX(enterSfx);

            if (gameData != null)
            {
                continueButton.SetActive(true);
            }
        }
    }

    public void ContinueGame()
    {
        AudioManager.SharedInstance.PlaySFX(buttonSfx);
        GameManager.SharedInstance.GetComponent<GameManager>();
        ContinueGameFromTitleScreen?.Invoke();

        SceneHelper.SharedInstance.LoadScene(gameData.levelData.lastCheckpointScene);

        //HeroController.SharedInstance.Coins = gameData.heroData.coinsAmount;

        print("SE HA CARGADO LA PARTIDA");
        Debug.Log("La escena cargada es: " + gameData.levelData.lastCheckpointScene);
        Debug.Log("La cantidad de coins cargada es: " + gameData.heroData.coinsAmount);
        IsInMainMenu?.Invoke(true);
    }

    public void ExitGame()
    {
        AudioManager.SharedInstance.PlaySFX(buttonSfx);

        Application.Quit();

    }

    public void StartNewGame()
    {
        //Debug.Log("Se presionó START NEW GAME");
        AudioManager.SharedInstance.PlaySFX(buttonSfx);
        GameManager.SharedInstance.GetComponent<GameManager>();
        NewGameFromTitleScreen?.Invoke();
        //print("SE debió invoncar el evento del start new game");
        SceneHelper.SharedInstance.LoadScene(SceneID.Level1_1);
        IsInMainMenu?.Invoke(true);

        if(HeroController.SharedInstance != null)
        {
            HeroController.SharedInstance.SetPlayerToNewGameStatus();
        }
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    public bool showOptions = false;

    [Header("Buttons")]
    public List<GameObject> optionButtons;
    public GameObject pressEnterButton;
    public GameObject continueButton;

    [Header("Audio")]
    [SerializeField] AudioClip enterSFX;
    [SerializeField] AudioClip buttonSFX;

    GameData gameData;

    private void Awake()
    {
        SaveSystem.SharedInstance.Load(out gameData); // Nos rellena los datos guardados
        Debug.Log("La escena que se ha cargado es: " + gameData.levelData.sceneID.ToString());
    }

    private void LateUpdate()
    {
        if (!showOptions && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
        {
            showOptions = true;
            pressEnterButton.SetActive(false);
            foreach (var button in optionButtons)
            {
                button.SetActive(true);
            }
            AudioManager.SharedInstance.PlaySFX(enterSFX);

            if (gameData != null) // si es diferente de null, sí hay datos guardados
            {
                continueButton.SetActive(true);
            }
        }

    }

    public void ContinueGame()
    {
        AudioManager.SharedInstance.PlaySFX(buttonSFX);

        SceneHelper.SharedInstance.LoadScene(gameData.levelData.sceneID);
    }

    public void ExitGame()
    {
        AudioManager.SharedInstance.PlaySFX(buttonSFX);
        Application.Quit();
    }

    public void StartNewGame()
    {
        AudioManager.SharedInstance.PlaySFX(buttonSFX);
        SceneHelper.SharedInstance.LoadScene(SceneID.Level1_1);
    }
}*/