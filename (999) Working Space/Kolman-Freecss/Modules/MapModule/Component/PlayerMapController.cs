// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.MapModule;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.Common.UI
{
    /// <summary>
    /// You'll need to create a layer with TownBoundary to detect the player trying to exit the town
    /// </summary>
    public class PlayerMapController : MonoBehaviour
    {
        private MapManager m_MapManager;
        
        private void Awake()
        {
            m_MapManager = GetComponentInChildren<MapManager>();
            if (m_MapManager == null)
            {
                Debug.LogError("MapManager not found in player children components");
            }
        }

        private void Start()
        {
            if (m_MapManager != null)
                m_MapManager.ActiveMap(false);
        }
        
        private void Update()
        {
            if (m_MapManager != null)
            {
                // Close
                if (Input.GetKeyDown(KeyCode.Escape) && IsMapActive())
                {
                    m_MapManager.ActiveMap(false);
                }
            }
            // Player doesn't need to press M to show the map
            // if (Input.GetKeyDown(KeyCode.M))
            // {
            //     ShowCanvas(!m_Canvas.enabled);
            //     ShowMouseCursor(m_Canvas.enabled);
            // }
        }
        
        public bool IsMapActive()
        {
            return m_MapManager.IsCanvasActive();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("TownBoundary"))
            {
                if (m_MapManager != null)
                    m_MapManager.ActiveMap();
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("TownBoundary"))
            {
                if (m_MapManager != null)
                    m_MapManager.ActiveMap(false);
            }
        }
    }
}