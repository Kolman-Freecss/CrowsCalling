// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.Common.Config;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using _999__Working_Space.Kolman_Freecss.Modules.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace _999__Working_Space.Kolman_Freecss.Modules.AudioModule
{
    public enum CommonSounds
    {
        ButtonClick,
        ButtonOk,
        ButtonBack,
        InputEscape,
        ScreenFadeIn,
        ScreenWoosh,
        Accolades
    }
    
    public class SoundManager : MonoBehaviour
    {
        [Serializable]
        public enum BackgroundMusic
        {
            MainMenu,
            Crystal,
            Spiresburg,
            Goose,
            EndGame,
        }

        #region Inspector Variables

        [Header("Audio Mixer Settings")] [SerializeField]
        private AudioMixer audioMixer;

        [SerializeField] private string m_MixerVarMainVolume = "MasterVolume";

        [SerializeField] private string m_MixerVarMusicVolume = "MusicVolume";

        [SerializeField] private string m_MixerVarSFXVolume = "SFXVolume";

        [Header("Audio Volume Settings")] [Range(0, 1)]
        public readonly static float SFXAudioVolume = 1f;

        [Range(0, 1)] public readonly static float MusicAudioVolume = 1f;

        [Range(0, 1)] public readonly static float MasterAudioVolume = 1f;

        [Header("Clips")] 
        public List<SerializableDictionaryEntry<BackgroundMusic, AudioClip>> BackgroundMusicClips;
        
        public List<SerializableDictionaryEntry<CommonSounds, AudioClip>> CommonSoundsClips;
        
        public AudioClip ButtonClickSound;

        [SerializeField] private AudioSource backgroundAudioSources;

        [SerializeField] private AudioSource sfxCommonAudioSource;
        
        public bool debug;

        #endregion

        #region Member Variables

        public static SoundManager Instance { get; private set; }

        private Dictionary<SceneTypes, BackgroundMusic> sceneBackgroundMusicDictionary = new()
        {
            { SceneTypes.MainMenu, BackgroundMusic.MainMenu },
            { SceneTypes.Location_Crystal, BackgroundMusic.Crystal },
            { SceneTypes.Location_Goose, BackgroundMusic.Goose },
            { SceneTypes.Location_Spiresburg, BackgroundMusic.Spiresburg },
        };

        #endregion
        
        #region Variable Data (Will need to be reset)
        
        private BackgroundMusic currentBackgroundClip;
        private BackgroundMusic previousBackgroundClip;
        
        #endregion

        #region InitData

        private void Awake()
        {
            ManageSingleton();
        }

        void ManageSingleton()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        /// <summary>
        /// When the player leaves the game, the data must be reset
        /// </summary>
        public void Reset()
        {
            if (BackgroundMusicClips == null)
                BackgroundMusicClips = new List<SerializableDictionaryEntry<BackgroundMusic, AudioClip>>();
            currentBackgroundClip = BackgroundMusic.MainMenu;
            previousBackgroundClip = BackgroundMusic.MainMenu;
        }

        private void Start()
        {
            Reset();
            ConfigureSFXVolume();
            ConfigureMusicVolume();
        }

        #endregion

        #region Logic

        public void StartTemporalBackground(BackgroundMusic backgroundMusic)
        {
            previousBackgroundClip = currentBackgroundClip;
            StartBackgroundMusic(backgroundMusic);
        }

        public void StartPreviousBackground()
        {
            StartBackgroundMusic(previousBackgroundClip);
        }

        public void StopBackgroundMusic()
        {
            if (backgroundAudioSources.isPlaying)
            {
                previousBackgroundClip = currentBackgroundClip;
                backgroundAudioSources.Stop();
            }
        }

        public void StartBackgroundMusic(BackgroundMusic backgroundMusic)
        {
            if (BackgroundMusicClips == null || BackgroundMusicClips.Count == 0)
            {
                Debug.LogWarning("No background music clips found");
                return;
            }

            AudioClip clip = BackgroundMusicClips.Find(x => x.Key == backgroundMusic).Value;
            if (clip != null)
            {
                if (backgroundAudioSources.isPlaying)
                    backgroundAudioSources.Stop();
                backgroundAudioSources.clip = clip;
                backgroundAudioSources.Play();
                currentBackgroundClip = backgroundMusic;
            }
            else
            {
                Debug.LogWarning($"No clip found for {backgroundMusic}");
            }
        }

        private float Linear2DB(float linear)
        {
            return Mathf.Log10(Mathf.Clamp(linear, 0.00001f, 1f)) * 20f;
        }

        public void ConfigureSFXVolume()
        {
            audioMixer.SetFloat(m_MixerVarSFXVolume, Linear2DB(ClientPrefs.GetSFXVolume()));
        }

        public void ConfigureMusicVolume()
        {
            audioMixer.SetFloat(m_MixerVarMusicVolume, Linear2DB(ClientPrefs.GetMusicVolume()));
        }

        public void ConfigureMasterVolume()
        {
            audioMixer.SetFloat(m_MixerVarMainVolume, Linear2DB(ClientPrefs.GetMasterVolume()));
        }

        public void ConfigureMasterMute()
        {
            audioMixer.SetFloat(m_MixerVarMainVolume, ClientPrefs.GetMasterMute() ? -80 : Linear2DB(ClientPrefs.GetMasterVolume()));
        }
        
        public void ConfigureMusicMute()
        {
            audioMixer.SetFloat(m_MixerVarMusicVolume, ClientPrefs.GetMusicMute() ? -80 : Linear2DB(ClientPrefs.GetMusicVolume()));
        }
        
        public void ConfigureSFXMute()
        {
            audioMixer.SetFloat(m_MixerVarSFXVolume, ClientPrefs.GetSFXMute() ? -80 : Linear2DB(ClientPrefs.GetSFXVolume()));
        }
        

        public void PlayButtonClickSound()
        {
            sfxCommonAudioSource.PlayOneShot(ButtonClickSound);
        }
        
        public void PlaySFXCommonSound(CommonSounds sound)
        {
            AudioClip clip = CommonSoundsClips.Find(x => x.Key == sound).Value;
            if (clip != null)
            {
                sfxCommonAudioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"No clip found for {sound}");
            }
        }

        public void PlayAudioSourceEffect(AudioSource audioSource)
        {
            audioSource.Play();
        }

        public void PlayWorldEffectAtPosition(Vector3 position, AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, position, ClientPrefs.GetSFXVolume());
        }
        
        public BackgroundMusic GetAssociatedBackgroundMusic(SceneTypes sceneType)
        {
            return sceneBackgroundMusicDictionary[sceneType];
        }

        #endregion
    }
}