// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _999__Working_Space.Kolman_Freecss.Modules.MapModule
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_CrystalTooltip;
        [SerializeField] private Button m_CrystalTown;
        
        [SerializeField] private TextMeshProUGUI m_GooseTooltip;
        [SerializeField] private Button m_GooseTown;
        
        [SerializeField] private TextMeshProUGUI m_SpiresBurgTooltip;
        [SerializeField] private Button m_SpiresBurgTown;
        
        private Canvas m_Canvas;

        private void Awake()
        {
            m_Canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            ShowCanvas(false);
            m_CrystalTown.onClick.AddListener(() => OnTownClicked(SceneTypes.Location_Crystal));
            m_GooseTown.onClick.AddListener(() => OnTownClicked(SceneTypes.Location_Goose));
            m_SpiresBurgTown.onClick.AddListener(() => OnTownClicked(SceneTypes.Location_Spiresburg));
        }
        
        public void ActiveMap(in bool active = true)
        {
            ShowCanvas(active);
            ShowMouseCursor(active);
        }
        
        private void ShowCanvas(in bool show = true)
        {
            m_Canvas.enabled = show;
        }
        
        private void ShowMouseCursor(in bool show = true)
        {
            Cursor.visible = show;
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // private void Update()
        // {
        //     // Player doesn't need to press M to show the map
        //     // if (Input.GetKeyDown(KeyCode.M))
        //     // {
        //     //     ShowCanvas(!m_Canvas.enabled);
        //     //     ShowMouseCursor(m_Canvas.enabled);
        //     // }
        // }
        
        public bool IsCanvasActive()
        {
            return m_Canvas.enabled;
        }

        private void OnTownClicked(in SceneTypes townName)
        {
            ShowCanvas(false);
            Debug.Log($"Town {townName} clicked");
            SceneTransitionHandler.Instance.LoadAdditiveScene(townName);
        }
        
        #region Destructor
        
        private void OnDestroy()
        {
            m_CrystalTown.onClick.RemoveAllListeners();
            m_GooseTown.onClick.RemoveAllListeners();
            m_SpiresBurgTown.onClick.RemoveAllListeners();
        }
        
        #endregion

    }
}