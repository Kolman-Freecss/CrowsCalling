// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule
{
    public class SceneLevelManager : MonoBehaviour
    {
        [SerializeField]
        private SceneTypes m_TargetScene;
        
        private bool pollData = false;

        private void Update()
        {
            if (!pollData && m_TargetScene == SceneTransitionHandler.Instance.CurrentSceneState)
            {
                PostStart();
            }
        }
        
        private void PostStart()
        {
            if (SceneTransitionHandler.Instance.IsPlayerInGameScene())
            {
                // Start the background music associated with the current scene.
                SoundManager.Instance.StartBackgroundMusic(SoundManager.Instance.GetAssociatedBackgroundMusic(m_TargetScene));
                Checkpoint Checkpoint = GameObject.FindObjectOfType<Checkpoint>();
                if (Checkpoint != null && Checkpoint.TargetScene == m_TargetScene)
                {
                    Debug.Log("Checkpoint found position: " + Checkpoint.transform.position);
                    Transform _NewPosition = SceneTransitionHandler.Instance.TransportPlayerToCheckpoint(Checkpoint.transform);
                    if (_NewPosition == null)
                    {
                        Debug.LogError("Checkpoint not found");
                    }
                    else
                    {
                        Debug.Log("Player transported to checkpoint. New player position: " + _NewPosition.position);
                    }
                }
                else
                {
                    Debug.LogError("Checkpoint not found");
                }
                pollData = true;
            }
        }
    }
}