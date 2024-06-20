// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Narration/Line")]
    public class NarrationLine : ScriptableObject
    {
        [SerializeField]
        private NarrationCharacter m_Speaker;

        [SerializeField]
        private string m_Text;

        public NarrationCharacter Speaker => m_Speaker;
        public string Text => m_Text;
    }
}