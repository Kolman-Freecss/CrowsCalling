// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using _999__Working_Space.Kolman_Freecss.Modules.Common.UI;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _999__Working_Space.Kolman_Freecss.Modules.MenuModule
{
    public class InGameMenuManager : MonoBehaviour
    {
        #region Inspector Variables

        [Header("Buttons")] 
        [SerializeField] private Button settingsButton;

        [SerializeField] private Button goToMenuButton;

        [SerializeField] private Canvas audioSettingsCanvas;

        [SerializeField] private Button audioBackButton;
        
        private PlayerMapController playerMapController; // Player reference
        
        private PlayerInventory playerInventory; // Player reference

        private Canvas canvas;
        
        private CanvasGroup canvasGroup;
        
        #endregion
        
        #region Init Data

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            playerMapController = GetComponentInParent<PlayerMapController>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            if (playerMapController == null)
            {
                Debug.LogError("PlayerMapController not found in parent components");
            }
        }

        private void OnEnable()
        {
            AddOnHoverEventTrigger(settingsButton);
            AddOnHoverEventTrigger(goToMenuButton);
            AddOnHoverEventTrigger(audioBackButton);
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
            if (audioSettingsCanvas)
            {
                audioSettingsCanvas.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Audio Settings Canvas is not assigned in the inspector.");
            }
            
            EnableCanvas(false);
            SubscribeToEvents();
            SoundManager.Instance.StartBackgroundMusic(SoundManager.BackgroundMusic.MainMenu);
        }

        private void Update()
        {
            // Input key Escape
            if (Input.GetKeyDown(KeyCode.Escape) && ShouldOpenMenu())
            {
                EnableCanvas(!canvas.enabled);
                ShowMouseCursor(canvas.enabled);
            }
        }

        private bool ShouldOpenMenu()
        {
            if (playerMapController == null)
            {
                Debug.LogError("PlayerMapController is not assigned in the prefab.");
                return false;
            }
            if (playerInventory == null || playerInventory.UIInventoryController == null)
            {
                Debug.LogError("PlayerInventory is not assigned in the prefab.");
                return false;
            }
            bool inventoryActive = playerInventory.UIInventoryController.IsInventoryActive();
            bool shouldOpen = !playerMapController.IsMapActive() && !inventoryActive;
            if (inventoryActive)
            {   
                playerInventory.UIInventoryController.Hide();
            }
            return shouldOpen;
        }
        
        private void ShowMouseCursor(in bool show = true)
        {
            Cursor.visible = show;
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        private void EnableCanvas(in bool enable = true)
        {
            canvas.enabled = enable;
            canvasGroup.alpha = enable ? 1 : 0;
            canvasGroup.blocksRaycasts = enable;
        }

        void SubscribeToEvents()
        {
            settingsButton
                .onClick
                .AddListener(() => { OnSettingsButtonClicked(); });
            goToMenuButton
                .onClick
                .AddListener(() => { OnGoToMenuButtonClicked(); });
            audioBackButton
                .onClick
                .AddListener(() => { OnAudioBackButtonClicked(); });
        }

        #endregion

        #region Logic
        
        void OnPointerEnter()
        {
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ButtonBack);
        }

        void OnSettingsButtonClicked()
        {
            SoundManager.Instance.PlayButtonClickSound();
            audioSettingsCanvas.gameObject.SetActive(true);
            DisableHomeCanvasRaycast();
            // Disable other buttons
            goToMenuButton.interactable = false;
        }

        void OnGoToMenuButtonClicked()
        {
            SoundManager.Instance.PlayButtonClickSound();
            DisableHomeCanvasRaycast();
            SceneTransitionHandler.Instance.LoadScene(SceneTypes.MainMenu);
        }
        
        void OnAudioBackButtonClicked()
        {
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ButtonClick);
            audioSettingsCanvas.gameObject.SetActive(false);
            DisableHomeCanvasRaycast(false);
            // Disable other buttons
            goToMenuButton.interactable = true;
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
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            goToMenuButton.onClick.RemoveListener(OnGoToMenuButtonClicked);
            audioBackButton.onClick.RemoveListener(OnAudioBackButtonClicked);
        }

        #endregion
    }
}