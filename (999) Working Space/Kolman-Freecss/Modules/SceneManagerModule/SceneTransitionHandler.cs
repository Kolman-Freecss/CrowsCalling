// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow;
using _999__Working_Space.Kolman_Freecss.Modules.ReputationModule;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule
{
    public class SceneTransitionHandler : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private float transitionTime = 1f;

        #endregion
        
        #region Member properties

        public static SceneTransitionHandler Instance { get; private set; }

        private List<SceneTypes> m_PoolAdditiveScenes;
        
        private SceneTypes m_CurrentSceneState;
        public SceneTypes CurrentSceneState => m_CurrentSceneState;
        private SceneTypes m_PreviousSceneState;
        private List<SceneTypes> m_CurrentAdditiveScenes = new List<SceneTypes>();
        
        private Animator m_SceneTransitionAnimator;
        private bool m_hasAnimator;
        private int m_animIDTransitionStart = 0;
        private int m_animIDTransitionEnd = 0;

        public Action OnLoadingScene;

        #endregion

        #region Event Delegates

        public delegate void SceneStateChangedDelegateHandler(SceneTypes newState);

        public event SceneStateChangedDelegateHandler OnSceneStateChanged;

        #endregion

        #region InitData

        void Awake()
        {
            // Init possible additive scenes
            m_PoolAdditiveScenes = new List<SceneTypes>();
            m_PoolAdditiveScenes.Add(SceneTypes.Location_Goose);
            m_PoolAdditiveScenes.Add(SceneTypes.Location_Crystal);
            m_PoolAdditiveScenes.Add(SceneTypes.Location_Spiresburg);
            m_PoolAdditiveScenes.Add(SceneTypes.Location_Extra_CrowsCrowd);
            
            m_SceneTransitionAnimator = GetComponentInChildren<Animator>();
            m_hasAnimator = m_SceneTransitionAnimator != null;
            AssignAnimationIDs();
            ManageSingleton();
        }

        private void Start()
        {
            String ActiveScene = SceneManager.GetActiveScene().name;
            // Debug.Log("ActiveScene: " + ActiveScene);
            SceneTypes sceneType = (SceneTypes) Enum.Parse(typeof(SceneTypes), ActiveScene);
            SetSceneState(sceneType);
            m_PreviousSceneState = sceneType;
        }

        private void AssignAnimationIDs()
        {
            m_animIDTransitionStart = Animator.StringToHash("Transition");
            m_animIDTransitionEnd = Animator.StringToHash("TransitionEnd");
        }

        private void ManageSingleton()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        #endregion

        #region Logic

        public void StartGame() {
            Debug.Log("--------------------------------- StartGame ---------------------------------");
            LoadScene(SceneTypes.Main, false);
            LoadAdditiveScene(SceneTypes.Location_Spiresburg);
            ShowMouseCursor(false);
        }
        
        private void ShowMouseCursor(in bool show = true)
        {
            Cursor.visible = show;
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        public void StartCrowsCrowd()
        {
            Debug.Log("--Crows crowd--");
            LoadScene(SceneTypes.Main);
            LoadAdditiveScene(SceneTypes.Location_Extra_CrowsCrowd);
        }
        
        public void LoadAdditiveScene(SceneTypes sceneState)
        {
            m_CurrentAdditiveScenes.Add(sceneState);
            OnLoadingScene += OnTransitionLoaded;
            StartCoroutine(OnLoadNewScene());
            void OnTransitionLoaded()
            {
                SceneManager.LoadSceneAsync(sceneState.ToString(), LoadSceneMode.Additive);
                // Debug.Log("OnTransitionLoaded Additive: " + sceneState);
                SetSceneState(sceneState);
                UnloadAdditiveScene(m_PreviousSceneState);
                if (IsPlayerInGameScene())
                {
                    ShowMouseCursor(false);
                    GameState.Instance.IsGameStarted = true;
                }
                EndTransition();
                SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ScreenWoosh);
                OnLoadingScene -= OnTransitionLoaded;
            }
        }
        
        public void UnloadAdditiveScene(SceneTypes sceneState)
        {
            if (m_CurrentAdditiveScenes.Count == 0 || m_CurrentAdditiveScenes.Contains(sceneState) == false)
            {
                // Debug.Log("UnloadAdditiveScene: " + sceneState + " not found");
                return;
            }
            // Debug.Log("UnloadAdditiveScene: " + sceneState);
            m_CurrentAdditiveScenes.Remove(sceneState);
            SceneManager.UnloadSceneAsync(sceneState.ToString());
        }

        public void LoadScene(SceneTypes sceneState, in bool subscribeToOnLoaded = true)
        {
            m_CurrentAdditiveScenes.Clear();
            // Add as additive scene if it is not already loaded
            if (m_PoolAdditiveScenes.Contains(sceneState) && m_CurrentAdditiveScenes.Contains(sceneState) == false)
            {
                m_CurrentAdditiveScenes.Add(sceneState);
                // Debug.Log("LoadScene: " + sceneState + " as additive");
            }

            if (!subscribeToOnLoaded)
            {
                SceneManager.LoadSceneAsync(sceneState.ToString());
                Debug.Log("OnTransitionLoaded: " + sceneState);
                SetSceneState(sceneState);
                return;
            }
            
            if (sceneState == SceneTypes.MainMenu)
            {
                GameState.Instance.IsGameStarted = false;
            }
            OnLoadingScene += OnTransitionLoaded;
            StartCoroutine(OnLoadNewScene());

            void OnTransitionLoaded()
            {
                SceneManager.LoadSceneAsync(sceneState.ToString());
                Debug.Log("OnTransitionLoaded: " + sceneState);
                SetSceneState(sceneState);
                if (sceneState == SceneTypes.MainMenu)
                {
                    ShowMouseCursor();
                    ResetAllManagers();
                }
                EndTransition();
                SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ScreenWoosh);
                OnLoadingScene -= OnTransitionLoaded;
            }
        }

        private IEnumerator OnLoadNewScene()
        {
            StartTransition();
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ScreenFadeIn);
            yield return new WaitForSeconds(transitionTime);
            // Debug.Log("OnLoadNewScene: " + m_CurrentSceneState);
            OnLoadingScene?.Invoke();
            if (IsPlayerInGameScene())
            {
                GameState.Instance.OnTownChange();
            }
        }

        /// <summary>
        /// Invoked when player restarts the game (Go from game over or game to main menu)
        /// </summary>
        public void ResetAllManagers()
        {
            Debug.Log("ResetAllManagers");
            DialogueFlowManager.Instance.Reset();
            ReputationManager.Instance.Reset();
            GameState.Instance.Reset();
            SoundManager.Instance.Reset();
            SoundTrackCrowsManager.Instance.Reset();
        }

        public bool IsPlayerInGameScene()
        {
            return m_CurrentSceneState == SceneTypes.Location_Spiresburg || m_CurrentSceneState == SceneTypes.Location_Goose || m_CurrentSceneState == SceneTypes.Location_Crystal;
        }
        

        public Transform TransportPlayerToCheckpoint(Transform checkpoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = checkpoint.position;
                player.transform.rotation = checkpoint.rotation;
                return player.transform;
            }
            else 
            {
                Debug.LogError("Player not found");
            }
            return null;
        }

        private void StartTransition()
        {
            if (m_hasAnimator)
            {
                m_SceneTransitionAnimator.SetTrigger(m_animIDTransitionStart);
            }
        }
        
        public void EndTransition()
        {
            if (m_hasAnimator)
            {
                m_SceneTransitionAnimator.SetTrigger(m_animIDTransitionEnd);
            }
        }

        public IEnumerator OnGameStartTransition()
        {
            StartTransition();
            SoundManager.Instance.PlaySFXCommonSound(CommonSounds.ScreenFadeIn);
            yield return new WaitForSeconds(transitionTime);
            OnLoadingScene?.Invoke();
        }

        private void SetSceneState(SceneTypes sceneState)
        {
            if (m_CurrentSceneState != SceneTypes.None)
            {
                m_PreviousSceneState = m_CurrentSceneState;
            }
            m_CurrentSceneState = sceneState;
            if (OnSceneStateChanged != null)
            {
                OnSceneStateChanged.Invoke(m_CurrentSceneState);
            }

            
            // TODO: This logic we may put in Start lyfecycle of the scene 
            // if (sceneState == SceneTypes.Level1)
            // {
            //     Cursor.lockState = CursorLockMode.Locked;
            //     Cursor.visible = false;
            // }
            // else
            // {
            //     Cursor.lockState = CursorLockMode.None;
            //     Cursor.visible = true;
            // }
        }

        #endregion

        #region Destructor

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        #endregion

        #region Getter & Setter

        public SceneTypes GetCurrentSceneState()
        {
            return m_CurrentSceneState;
        }
        
        public SceneTypes GetPreviousSceneState()
        {
            return m_PreviousSceneState;
        }

        #endregion
    }
}