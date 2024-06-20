using _999__Working_Space.Kolman_Freecss.Modules.ReputationModule.UI;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.ReputationModule.Components
{
    public class PlayerReputationController : MonoBehaviour
    {
        [SerializeField]
        private UIWidgetReputationController m_ReputationWidget;
        
        public void UpdateReputation(in float reputation)
        {
            m_ReputationWidget.UpdateReputation();
        }
    }
}