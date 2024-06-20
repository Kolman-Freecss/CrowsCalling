// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField]
        private SceneTypes m_TargetScene;
        public SceneTypes TargetScene => m_TargetScene;
    }
}