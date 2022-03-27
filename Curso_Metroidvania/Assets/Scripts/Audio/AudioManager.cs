using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float musicVolume;
    private float _musicVolume;

    [Range(0, 1)]
    [SerializeField] private float sfxVolume;
    private float _sfxVolume;

    private static AudioSource musicAudioSource;
    private static AudioSource sfxAudioSource;

    private static AudioManager _sharedInstance;

    public static AudioManager SharedInstance
    {
        get
        {
            if(_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<AudioManager>();

                GameObject gameAudioManager;

                if(_sharedInstance == null)
                {
                    gameAudioManager = new GameObject("AudioManager");
                    gameAudioManager.AddComponent<AudioManager>();
                    _sharedInstance = gameAudioManager.GetComponent<AudioManager>();
                }

                if (_sharedInstance != null)
                {
                    var gameMusic = new GameObject("Music");
                    gameMusic.AddComponent<AudioSource>();
                    musicAudioSource = gameMusic.GetComponent<AudioSource>();
                    gameMusic.transform.parent = _sharedInstance.gameObject.transform;

                    var gameSFX = new GameObject("SFX");
                    gameSFX.AddComponent<AudioSource>();
                    sfxAudioSource = gameSFX.GetComponent<AudioSource>();
                    gameSFX.transform.parent = _sharedInstance.gameObject.transform;

                    DontDestroyOnLoad(_sharedInstance.gameObject);
                }
            }

            return _sharedInstance;
        }
    }

    public void PlaySFX(AudioClip audioClip)
    {
        // Reproduce el audio una sola vez
        if (audioClip != null)
            sfxAudioSource.PlayOneShot(audioClip);
    }
    
    public void PlayMusic(AudioClip audioClip)
    {
        // Para evitar que se repita
        if(musicAudioSource.clip != audioClip)
        {
            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }

    private void Update()
    {
        //Debug.Log("POS DEL PLAYER EN AUDIO MANAGER: " + HeroController.SharedInstance.transform.position);  
        // si esto se cumple es porque el usuario modificó la música
        // se reasigna para que no haya problemas
        if(musicVolume != _musicVolume)
        {
            _musicVolume = musicVolume;
            if (musicAudioSource != null)
                musicAudioSource.volume = musicVolume;
        }

        if (sfxVolume != _sfxVolume)
        {
            _sfxVolume = sfxVolume;
            if (sfxAudioSource != null)
                sfxAudioSource.volume = sfxVolume;
        }
    }
}
