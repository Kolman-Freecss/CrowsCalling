// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _999__Working_Space.Kolman_Freecss.Modules.MenuModule
{
    public class HomeManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private TextMeshProUGUI versionText;

        [Header("Buttons")] [SerializeField] private Button quitButton;

        [SerializeField] private Button startButton;

        [SerializeField] private Button settingsButton;

        [SerializeField] private Button creditsButton;

        [SerializeField] private Canvas audioSettingsCanvas;

        [SerializeField] private Button audioBackButton;
        
        [SerializeField] private Canvas creditsCanvas;

        [SerializeField] private Button creditsBackButton;
        
        [SerializeField]
        private Toggle playOrchestraToggle;

        #endregion
        
        #region Init Data

        private void OnEnable()
        {
            AddOnHoverEventTrigger(quitButton);
            AddOnHoverEventTrigger(startButton);
            AddOnHoverEventTrigger(settingsButton);
            AddOnHoverEventTrigger(creditsButton);
            AddOnHoverEventTrigger(audioBackButton);
            AddOnHoverEventTrigger(creditsBackButton);
        }
        
        private void AddOnHoverEventTrigger(Button button)
        {
            if (button == null || button.GetComponent<EventTrigger>() == null)
            {
                Debug.LogError("Button or EventTrigger is not assigned in the inspector.");
                if (button != null) Debug.LogError("Button name: " + button.name);
                return;
            }
            button
                .GetComponent<EventTrigger>()
                .triggers.Clear();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnPointerEnter(); });
            button
                .GetComponent<EventTrigger>()
                .triggers.Add(entry);
        }

        void Start()
        {
            if (versionText)
            {
                versionText.text = "v1.0.0-beta";
                // versionText.text = PlayerSettings.bundleVersion;
            }
            else
            {
                Debug.LogError("Version Text is not assigned in the inspector.");
            }

            if (audioSettingsCanvas)
            {
                audioSettingsCanvas.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Audio Settings Canvas is not assigned in the inspector.");
            }
            
            if (creditsCanvas)
            {
                creditsCanvas.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Credits Canvas is not assigned in the inspector.");
            }
            
            if (playOrchestraToggle)
            {
                playOrchestraToggle.isOn = false;
            }
            else
            {
                Debug.LogError("Play Orchestra Toggle is not assigned in the inspector.");
            }
            SubscribeToEvents();
            SoundManager.Instance.StartBackgroundMusic(SoundManager.BackgroundMusic.MainMenu);
        }

        void SubscribeToEvents()
        {
            quitButton
                .onClick
                .AddListener(() => { OnQuitButtonClicked(); });
            startButton
                .onClick
                .AddListener(() => { OnPlayButtonClicked(); });
            settingsButton
                .onClick
                .AddListener(() => { OnSettingsButtonClicked(); });
            creditsButton
                .onClick
                .AddListener(() => { OnCreditsButtonClicked(); });
            audioBackButton
                .onClick
                .AddListener(() => { OnAudioBackButtonClicked(); });
            creditsBackButton
                .onClick
                .AddListener(() => { OnCreditsBackButtonClicked(); });
            playOrchestraToggle
                .onValueChanged
                .AddListener((value) => { OnPlayOrchestraToggle(value); });
        }

        #endregion

        #region Logic
        
        void OnPointerEnter()
        {
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ButtonBack);
        }
        
        private void OnPlayOrchestraToggle(bool value)
        {
            SoundManager.Instance.PlayButtonClickSound();
            SoundTrackCrowsManager.Instance.PlayAllCrowsCalling(value);
        }

        void OnQuitButtonClicked()
        {
            SoundManager.Instance.PlayButtonClickSound();
            Application.Quit();
            DisableHomeCanvasRaycast();
        }

        void OnPlayButtonClicked()
        {
            SoundManager.Instance.PlayButtonClickSound();
            SceneTransitionHandler.Instance.StartGame();
            DisableHomeCanvasRaycast();
        }

        void OnSettingsButtonClicked()
        {
            SoundManager.Instance.PlayButtonClickSound();
            audioSettingsCanvas.gameObject.SetActive(true);
            DisableHomeCanvasRaycast();
            // Disable other buttons
            quitButton.interactable = false;
            startButton.interactable = false;
            creditsButton.interactable = false;
        }

        void OnCreditsButtonClicked()
        {
            SoundManager.Instance.PlayButtonClickSound();
            creditsCanvas.gameObject.SetActive(true);
            DisableHomeCanvasRaycast();
            // Disable other buttons
            quitButton.interactable = false;
            startButton.interactable = false;
            settingsButton.interactable = false;
        }
        
        void OnAudioBackButtonClicked()
        {
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ButtonClick);
            audioSettingsCanvas.gameObject.SetActive(false);
            DisableHomeCanvasRaycast(false);
            // Disable other buttons
            quitButton.interactable = true;
            startButton.interactable = true;
            creditsButton.interactable = true;
        }
        
        void OnCreditsBackButtonClicked()
        {
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ButtonClick);
            creditsCanvas.gameObject.SetActive(false);
            DisableHomeCanvasRaycast(false);
            // Enable other buttons
            quitButton.interactable = true;
            startButton.interactable = true;
            settingsButton.interactable = true;
        }
        
        void DisableHomeCanvasRaycast(in bool disable = true)
        {
            GetComponent<GraphicRaycaster>().enabled = !disable;
        }
        
        #endregion

        #region Destructor

        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        void UnsubscribeToEvents()
        {
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
            startButton.onClick.RemoveListener(OnPlayButtonClicked);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            creditsButton.onClick.RemoveListener(OnCreditsButtonClicked);
            audioBackButton.onClick.RemoveListener(OnAudioBackButtonClicked);
        }

        #endregion
    }
}