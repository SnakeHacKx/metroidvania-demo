using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip levelAmbientSound;
    [SerializeField] AudioClip finalBossMusic;

    [Header("Final Boss")]
    [SerializeField] private bool isFinalBoss;
    [SerializeField] private GameObject finalBoss;

    [Header("Background & Camera")]
    [SerializeField] TagID confinerTag;
    //[SerializeField] GameObject cameraConfiner;
    [SerializeField] private float cameraSize;
    [SerializeField] private Sprite levelBackground;
    [SerializeField] private Vector3 backgroundScale;

    private bool activateBossFight;

    private static LevelManager _sharedInstance;

    public static LevelManager SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<LevelManager>();
            }
            return _sharedInstance;
        }
        set
        {
            _sharedInstance = value;
        }
    }

    void Start()
    {
        if (finalBoss != null)
        finalBoss.SetActive(false);

        if (levelMusic != null)
            AudioManager.SharedInstance.PlayMusic(levelMusic);

        if (levelAmbientSound != null)
            AudioManager.SharedInstance.PlayMusic(levelAmbientSound);

        if (ChangeBackground.SharedInstance != null)
        {
            ChangeBackground.SharedInstance.ChangeBackgroundTo(levelBackground, backgroundScale);
        }

        SetCameraBoundary();
    }

    /// <summary>
    /// Se encarga de colocar los confinadores de la cámara al momento de cargar una nueva escena.
    /// </summary>
    public void SetCameraBoundary()
    {
        var virtualCamera = GameObject.FindGameObjectWithTag(TagID.VirtualCamera.ToString());

        if (virtualCamera == null)
            return;

        var confiner = virtualCamera.GetComponent<CinemachineConfiner>();

        //todo: prueb
        confiner.m_BoundingShape2D = null;
        confiner.InvalidatePathCache();
        //confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag(confinerTag.ToString()).GetComponent<Collider2D>();
    }

    public void FinalBossWasVanquished()
    {
        if (levelMusic != null)
            AudioManager.SharedInstance.PlayMusic(levelMusic);
        if (levelAmbientSound != null)
            AudioManager.SharedInstance.PlayMusic(levelAmbientSound);

        var block = FindObjectOfType<BossBlock>();
        if (block)
        {
            block.StartUnlock();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!activateBossFight && isFinalBoss && collision.gameObject.CompareTag(TagID.Player.ToString()))
        {
            activateBossFight = true;

            AudioManager.SharedInstance.PlayMusic(finalBossMusic);
            var block = FindObjectOfType<BossBlock>();
            if (block != null)
            {
                block.StartBlock();
            }

            if(finalBoss != null)
            {
                finalBoss.SetActive(true);
            }
            FindObjectOfType<CameraController>().ChangeCameraSize(cameraSize);
        }
    }
}
