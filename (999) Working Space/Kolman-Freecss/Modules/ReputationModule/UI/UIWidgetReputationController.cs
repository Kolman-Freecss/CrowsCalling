using System.Globalization;
using TMPro;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.ReputationModule.UI
{
    public class UIWidgetReputationController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_ReputationText;

        private void Start()
        {
            UpdateReputation();
        }

        public void UpdateReputation()
        {
            if (ReputationManager.Instance == null)
            {
                Debug.LogError("ReputationManager is null");
                return;
            }
            float reputation = ReputationManager.Instance.CurrentReputation;
            float maxReputation = ReputationManager.Instance.m_MaxGeneralReputation;
            float percentage = reputation / maxReputation * 100;
            m_ReputationText.text = percentage.ToString(CultureInfo.CurrentCulture) + "%";
        }
    }
}