// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.DebugTests
{
    /// <summary>
    /// If you need to test this put the scenes into Build Settings
    /// </summary>
    public class DebugSoundManager : MonoBehaviour
    {
        
        public static DebugSoundManager Instance { get; private set; }
        
        private void Start()
        {
            ManageSingleton();
            SoundManager.Instance.StartBackgroundMusic(SoundManager.BackgroundMusic.Crystal);
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
        
    }
}