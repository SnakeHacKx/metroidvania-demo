using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    private static SceneHelper _sharedInstance;
    // private SceneID previousScene;

    public static SceneHelper SharedInstance
    {
        get
        {
            if(_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<SceneHelper>();

                if (_sharedInstance == null)
                {
                    var gameObj = new GameObject("SceneHelper");
                    gameObj.AddComponent<SceneHelper>();

                    _sharedInstance = gameObj.GetComponent<SceneHelper>();
                }

                DontDestroyOnLoad(_sharedInstance.gameObject);
            }

            return _sharedInstance;
        }
    }

    public SceneID GetCurrentSceneID()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneID sceneID);
        return sceneID;
    }

    public void ReloadScene()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneID sceneID);
        StartCoroutine(LoadSceneCoroutine(sceneID));
    }

    public void RestartGame()
    {
        Debug.Log("He muerto sin haber guardado el juego, reiniciando el juego...");
        HeroController.SharedInstance.SetPlayerToNewGameStatus();
        SceneManager.LoadScene(SceneID.Level1_1.ToString());
    }

    public void GoToLastCheckPoint()
    {
        Debug.Log("El ultimo punto de guardado es: " + GameManager.SharedInstance.lastCheckpointScene);
        SceneManager.LoadScene(GameManager.SharedInstance.lastCheckpointScene.ToString());
        TeleportVirtualCamera.SharedInstance.ChangePosition(this.transform.position);
        HeroController.SharedInstance.SetPlayerToDefaultStatus();
    }

    public void LoadScene(SceneID sceneID)
    {
        StartCoroutine(LoadSceneCoroutine(sceneID));
    }

    private IEnumerator LoadSceneCoroutine(SceneID sceneID)
    {
        //print("Se comenzó a cargar otra escena");
        // SceneManager.GetActiveScene().name; Devuelve el nombre de la escena actual
        //yield return new WaitForSeconds(1);
 
        //yield return LoadingScreen.SharedInstance.OnLoadScreenCoroutine();

        //Enum.TryParse(SceneManager.GetActiveScene().name, out previousScene);
        //Debug.Log("La escena a cargar es: " + sceneID.GetHashCode());
      
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID.ToString());

        //CameraController.SharedInstance.FreezeCamera();

        /*if (Camera.main.GetComponent<CameraController>() != null)
        {
            Debug.Log("Debio llamar a Freeze Camera");
            Camera.main.GetComponent<CameraController>().FreezeCamera();
        }*/
            

        /*if (HeroController.SharedInstance != null)
        {
            HeroController.SharedInstance.PutOutBoundaries();
        }*/

        // Espera hasta que la escena asíncrona cargue completamente
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Sepa Cristo rey lo que hizo este malparido aquí :v
        //var list = FindObjectsOfType<PortalScene>().ToList();

        /*if (list != null)
        {
            try
            {
                if (previousScene != SceneID.TitleScreen)
                {
                    Debug.Log("PREVIUS SCENE: " + previousScene.ToString());
                    var spawnPosition = list.Find(x => x.SceneToLoad() == previousScene).GetSpawnPosition();
                    Debug.Log("EL SPAWN POSITION ES: " + spawnPosition.ToString());
                    if (HeroController.SharedInstance == null)
                    {
                        print("EL HEROE SE DESTRUYÓ");
                    }
                    HeroController.SharedInstance.PutOnSpawnPosition(spawnPosition);
                    Camera.main.GetComponent<CameraController>().UpdatePosition(spawnPosition);
                }

            }
            catch (Exception ex)
            {
                Debug.Log("Se ha producido un error al cargar la escena:\n" + ex.ToString());
            }
        }*/

        yield return new WaitForSeconds(1);
        //yield return LoadingScreen.SharedInstance.OnLoadedScreenCoroutine();
        //Debug.LogFormat("Se cargó la escena {0} correctamente, debería mostrase...", sceneID.ToString());
    }
}

/*
 * using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{

    private static SceneHelper _SharedInstance;


    public SceneID previousScene;
    public static SceneHelper SharedInstance
    {
        get
        {

            if (_SharedInstance == null)
            {
                _SharedInstance = FindObjectOfType<SceneHelper>();

                if (_SharedInstance == null)
                {
                    var go = new GameObject("SceneHelper");
                    go.AddComponent<SceneHelper>();

                    _SharedInstance = go.GetComponent<SceneHelper>();
                }
                DontDestroyOnLoad(_SharedInstance.gameObject);
            }
            return _SharedInstance;
        }
    }

    public SceneID GetCurrentSceneID()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneID sceneId);
        return sceneId;
    }
    public void ReloadScene()
    {
        Enum.TryParse(SceneManager.GetActiveScene().name, out SceneID sceneId);
        StartCoroutine(_LoadScene(sceneId));
    }

    public void LoadScene(SceneID sceneId)
    {

        StartCoroutine(_LoadScene(sceneId));
    }

    private IEnumerator _LoadScene(SceneID sceneId)
    {
        //  SceneManager.GetActiveScene().name
        yield return LoadingScreen.SharedInstance.OnLoadScreenCoroutine();

        Enum.TryParse(SceneManager.GetActiveScene().name, out previousScene);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId.ToString());
        if (Camera.main.GetComponent<CameraController>() != null)
        {
            Camera.main.GetComponent<CameraController>().FreezeCamera();
        }
        if (HeroController.SharedInstance != null)
        {
            HeroController.SharedInstance.PutOutBoundaries();
        }
        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        var list = FindObjectsOfType<PortalScene>().ToList();
        if (list != null)
        {
            try
            {
                var spawnPosition = list.Find(x => x.SceneToLoad() == previousScene).GetSpawnPosition();
                Debug.Log("spawnPosition " + spawnPosition);
                HeroController.SharedInstance.PutOnSpawnPosition(spawnPosition);
                Camera.main.GetComponent<CameraController>().UpdatePosition(spawnPosition);
            }
            catch (Exception ex)
            {
            }
        }


        yield return new WaitForSeconds(1);
        yield return LoadingScreen.SharedInstance.OnLoadedScreenCoroutine();
    }
}*/