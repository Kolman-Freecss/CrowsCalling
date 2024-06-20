// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.Common.Config;
using UnityEngine;
using UnityEngine.UI;

namespace _999__Working_Space.Kolman_Freecss.Modules.AudioModule
{
    public class AudioSettingsUIManager : MonoBehaviour
    {
        #region Inspector Variables

        [Header("Buttons")] [SerializeField] private Button backButton;

        [Header("Sliders")] [SerializeField] private Slider m_MasterVolumeSlider;

        [SerializeField] private Slider m_SFXVolumeSlider;

        [SerializeField] private Slider m_MusicVolumeSlider;
        
        [SerializeField]
        private Toggle m_MasterMuteToggle;
        
        [SerializeField]
        private Toggle m_MusicMuteToggle;
        
        [SerializeField]
        private Toggle m_SFXMuteToggle;

        #endregion
        
        private Canvas m_Canvas;

        #region InitData

        private void Awake()
        {
            m_Canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            // backButton
            //     .onClick
            //     .AddListener(() =>
            //     {
            //         SoundManager.Instance.PlayButtonClickSound();
            //         ShowCanvas(false);
            //     });

            // Note that we initialize the slider BEFORE we listen for changes (so we don't get notified of our own change!)
            m_MasterVolumeSlider.value = ClientPrefs.GetMasterVolume();
            m_MasterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);

            // initialize music slider similarly.
            m_MusicVolumeSlider.value = ClientPrefs.GetMusicVolume();
            m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChanged);

            // initialize effects slider similarly.
            m_SFXVolumeSlider.value = ClientPrefs.GetSFXVolume();
            m_SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChanged);
            
            m_MasterMuteToggle.isOn = ClientPrefs.GetMasterMute();
            m_MasterMuteToggle.onValueChanged.AddListener(OnMasterToggleChanged);
            
            m_MusicMuteToggle.isOn = ClientPrefs.GetMusicMute();
            m_MusicMuteToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            
            m_SFXMuteToggle.isOn = ClientPrefs.GetSFXMute();
            m_SFXMuteToggle.onValueChanged.AddListener(OnSFXToggleChanged);

        }

        // private void Start()
        // {
        //     ShowCanvas(false);
        // }

        #endregion

        #region Logic
        
        public void ShowCanvas(bool show)
        {
            SoundManager.Instance.PlayButtonClickSound();
            m_Canvas.enabled = show;
        }
        
        private void OnMasterToggleChanged(bool newValue)
        {
            ClientPrefs.SetMasterMute(newValue);
            SoundManager.Instance.ConfigureMasterMute();
        }
        
        private void OnMusicToggleChanged(bool newValue)
        {
            ClientPrefs.SetMusicMute(newValue);
            SoundManager.Instance.ConfigureMusicMute();
        }
        
        private void OnSFXToggleChanged(bool newValue)
        {
            ClientPrefs.SetSFXMute(newValue);
            SoundManager.Instance.ConfigureSFXMute();
        }

        private void OnMasterVolumeSliderChanged(float newValue)
        {
            ClientPrefs.SetMasterVolume(newValue);
            SoundManager.Instance.ConfigureMasterVolume();
        }

        private void OnMusicVolumeSliderChanged(float newValue)
        {
            ClientPrefs.SetMusicVolume(newValue);
            SoundManager.Instance.ConfigureMusicVolume();
        }

        private void OnSFXVolumeSliderChanged(float newValue)
        {
            ClientPrefs.SetSFXVolume(newValue);
            SoundManager.Instance.ConfigureSFXVolume();
        }

        #endregion

        #region Destructor

        private void OnDisable()
        {
            // backButton.onClick.RemoveAllListeners();
            m_MasterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeSliderChanged);
            m_MusicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeSliderChanged);
            m_SFXVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeSliderChanged);
            m_MasterMuteToggle.onValueChanged.RemoveListener(OnMasterToggleChanged);
            m_MusicMuteToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
            m_SFXMuteToggle.onValueChanged.RemoveListener(OnSFXToggleChanged);
        }

        #endregion
    }
}