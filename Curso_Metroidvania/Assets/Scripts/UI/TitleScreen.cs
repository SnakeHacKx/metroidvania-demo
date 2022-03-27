using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public SceneID sceneID;
    public GameObject mainMenu;
    public GameObject newGameMenu;
    public GameObject loadGameMenu;

    void Start()
    {
        MainMenu();
    }

    public virtual void MainMenu()
    {
        mainMenu.SetActive(true);
        loadGameMenu.SetActive(false);
        newGameMenu.SetActive(false);
    }

    public virtual void NewGameMenu()
    {
        mainMenu.SetActive(false);
        loadGameMenu.SetActive(false);
        newGameMenu.SetActive(true);
    }

    public virtual void LoadGameMenu()
    {
        mainMenu.SetActive(false);
        loadGameMenu.SetActive(true);
        newGameMenu.SetActive(false);
    }

    public virtual void NewGame(int slot)
    {
        PlayerPrefs.SetInt("GameFile", slot);
        PlayerPrefs.SetInt(" " + slot + "SaveSpawnReference", 0);
        PlayerPrefs.SetInt(" " + slot + "SpawnReference", 0);
        PlayerPrefs.SetInt(" " + slot + "CurrentHealth", 100);
        PlayerPrefs.SetString(" " + slot + "LoadGame", sceneID.ToString());
    }

    public virtual void LoadGame(int slot)
    {
        PlayerPrefs.SetInt("GameFile", slot);
        bool loadFromSave = true;
        PlayerPrefs.SetInt(" " + slot + "LoadFromSave", loadFromSave ? 1 : 0);
        SceneManager.LoadScene(PlayerPrefs.GetString(" " + slot + "Loadgame"));
    }
}

