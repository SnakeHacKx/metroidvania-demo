using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private static UIManager _sharedInstance;

    public GameObject hud;

    [Header("Inventory")]
    public GameObject inventoryPanel;
    public GameObject menuPanel;
    public Button inventoryButton;
    public TMP_Text inventoryDescription;
    public TMP_Text warning; // borrar cuando el inventario sirva para algo XD

    [Header("Control Menu")]
    [SerializeField] GameObject inventoryFirstButton;

    private ItemsManager itemsManager;

    public static UIManager SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<UIManager>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    /*var gameObj = new GameObject("GameManager");
                    gameObj.AddComponent<GameManager>();
                    _sharedInstance = gameObj.GetComponent<GameManager>();*/

                    var gameObj = Resources.Load("Managers/UIManager") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<UIManager>();
                }
                DontDestroyOnLoad(_sharedInstance.gameObject);
            }

            return _sharedInstance;
        }
    }

    private void Start()
    {
        itemsManager = FindObjectOfType<ItemsManager>();
        hud.SetActive(true);
        inventoryPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            ShowPauseScreen();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            ToggleInventory();
        }
    }

    public void ShowPauseScreen()
    {
        if (!GameManager.SharedInstance.GamePaused)
        {
            PauseScreen.SharedInstance.ShowPauseScreen();
            HeroController.SharedInstance.SetIsControlable(false);
        }
    }

    /// <summary>
    /// Muestra y quita el inventario de la pantalla.
    /// </summary>
    public void ToggleInventory()
    {
        inventoryDescription.text = "";
        // .activeInHierarchy devuelve true si esta activo y false si no
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        menuPanel.SetActive(!menuPanel.activeInHierarchy);

        if (inventoryPanel.activeInHierarchy)
        {
            warning.text = "Este inventario no tiene funcionalidades todavia";
            GameManager.SharedInstance.InInventoryScreen = true;
        }
        else
        {
            warning.text = "";
            GameManager.SharedInstance.InInventoryScreen = false;
        }

        if (inventoryPanel.activeInHierarchy)
        {
            // limpiar objeto seleccionado (hay que hacer esto si no, da error.. cosas raras de Unity)
            EventSystem.current.SetSelectedGameObject(null);

            // Poner un nuevo objeto seleccionado
            EventSystem.current.SetSelectedGameObject(inventoryFirstButton);

            // Vaciamos el inventario para que cuando lo rellenemos, no se dupliquen las cosas
            foreach (Transform t in inventoryPanel.transform)
            {
                Destroy(t.gameObject);
            }
            FillInventory();
        }
    }

    /// <summary>
    /// Rellena el inventario dinamicamente.
    /// </summary>
    public void FillInventory()
    {
        // Armas
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>(); 
        List<GameObject> weapons = weaponManager.GetAllWeapons();

        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            AddObjectToInventory(weapon, InventoryButton.ItemType.WEAPON, i);
            i++;
        }

        // Objetos Clave
        List<GameObject> keyItems = itemsManager.GetQuestItems();
        i = 0;
        foreach (GameObject item in keyItems)
        {
            AddObjectToInventory(item, InventoryButton.ItemType.SPECIAL_ITEM, i);
            i++;
        }

        // Aqui irian mas cosas que se le quiera anadir, por ejemplo, pociones
    }

    private void AddObjectToInventory(GameObject item, InventoryButton.ItemType type, int positionIndex)
    {
        // Instancia el inventory button como hijo del inventory panel
        Button temporaryButton = Instantiate(inventoryButton, inventoryPanel.transform);
        temporaryButton.GetComponent<InventoryButton>().type =type;
        temporaryButton.GetComponent<InventoryButton>().itemIdx = positionIndex;

        /*
             Como el boton espera un evento, necesitamos poner esta sintaxis del delegado, en donde el 
             boton escucha (recibe) el evento y el weaponManager lo envia
        */

        temporaryButton.onClick.AddListener(() => temporaryButton.GetComponent<InventoryButton>().ActivateButton());

        // Ponemos la imagen del arma en cada boton
        // Esto lo desactivo porque el arma del player esta en el sprite del player
        // Asi que el arma no tiene un sprite como tal
        if (type == InventoryButton.ItemType.SPECIAL_ITEM)
            temporaryButton.image.sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// Muestra los objetos de cierto tipo en el inventario (filtro).
    /// </summary>
    /// <param name="type">Tipo de objeto a mostrar, es un int pero recibe el numero del enumerado en realidad
    ///                     solo que al evento click no le gustan los enumerados asi que se hizo int.</param>
    public void ShowOnly(int type)
    {
        Debug.Log("Mostrar solo objeto de tipo :" + type);
        foreach (Transform t in inventoryPanel.transform)
        {
            t.gameObject.SetActive((int)t.GetComponent<InventoryButton>().type == type);
        }
    }

    /// <summary>
    /// Muestra todo los objetos del inventario.
    /// </summary>
    public void ShowAll()
    {
        foreach (Transform t in inventoryPanel.transform)
        {
            t.gameObject.SetActive(true);
        }
    }
}
