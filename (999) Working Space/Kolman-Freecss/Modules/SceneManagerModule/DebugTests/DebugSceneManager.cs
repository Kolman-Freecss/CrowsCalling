using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.DebugTests
{
    /// <summary>
    /// If you need to test this put the scenes into Build Settings
    /// </summary>
    public class DebugSceneManager : MonoBehaviour
    {
        
        public static DebugSceneManager Instance { get; private set; }
        
        private void Start()
        {
            ManageSingleton();
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
        public void UI_BTN_PRESS_START_GAME()
        {
            SceneTransitionHandler.Instance.StartGame();
        }
        public void UI_BTN_PRESS_CROWS_CROWD()
        {
            SceneTransitionHandler.Instance.StartCrowsCrowd();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) // Start Game
            {
                SceneTransitionHandler.Instance.StartGame();
                
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                
                // SceneTransitionHandler.Instance.LoadAdditiveScene(SceneTypes.KolmanFreecssTestSceneScene2);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                SceneTransitionHandler.Instance.StartCrowsCrowd();
                // SceneTransitionHandler.Instance.LoadAdditiveScene(SceneTypes.KolmanFreecssTestSceneScene3);
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                // SceneTransitionHandler.Instance.LoadAdditiveScene(SceneTypes.KolmanFreecssTestSceneScene1);
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                // SceneTransitionHandler.Instance.LoadScene(SceneTypes.KolmanFreecssTestSceneHome);
            }
        }
    }
}