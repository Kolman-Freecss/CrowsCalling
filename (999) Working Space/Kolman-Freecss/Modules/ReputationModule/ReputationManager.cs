using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.ReputationModule.Components;
using _999__Working_Space.Kolman_Freecss.Modules.Utils;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.ReputationModule
{
    public class ReputationManager : MonoBehaviour
    {
        public static ReputationManager Instance { get; private set; }
        
        // ------------------- Reputation values -------------------
        [SerializeField]
        private float m_MaxTimeInNegativeReputation = 600f;
        
        public float m_ReputationAmountForRequestCompleted = 10f;
        public float m_ReputationAmountForRequestFailed = -10f;
        
        // Dialogue choice reputation values
        public float m_ReputationAmountForBadDialogueChoice = -5f;
        public float m_ReputationAmountForGoodDialogueChoice = 5f;
        public float m_ReputationAmountForNeutralDialogueChoice = 0f;
        public float m_ReputationAmountForBestDialogueChoice = 10f;
        
        public float m_MaxGeneralReputation = 200f; // Max reputation the player can have with all the objectives
        public float m_MaxNpcReputation = 20f; // Max reputation the player can have with a single NPC
        // ------------------- ------------------- -------------------
        
        #region Variable Data (Will need to be reset)
        
        [SerializeField]
        private float m_CurrentReputation = 0f; // This will be displayed in the UI as a percentage
        
        public float CurrentReputation => m_CurrentReputation;
        
        [SerializeField]
        private PlayerReputationController m_PlayerReputationController;
        
        private ReputationStatusType m_ReputationStatus;
        
        public List<SerializableDictionaryEntry<int, float>> NPCGoodWillList; // NPC Index from NPCInteractable and its float value goodwill with the player
        
        // ------------------- Aux vars ------------------- 
        private float ElapsedTime = 0;
        
        private float m_CurrentTimeInNegativeReputation;
        
        private bool pollData = false;
        
        #endregion
        
        #region Init
        
        private void Awake()
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
        
        /// <summary>
        /// When the player leaves the game, the data must be reset
        /// </summary>
        public void Reset()
        {
            m_PlayerReputationController = null;
            m_CurrentReputation = 0;
            m_CurrentTimeInNegativeReputation = 0;
            ElapsedTime = 0;
            m_ReputationStatus = ReputationStatusType.NEGATIVE;
            NPCGoodWillList = new List<SerializableDictionaryEntry<int, float>>();
            pollData = false;
        }
        
        private void Start()
        {
            Reset();
            // TODO: Load NPCGoodWillList from GameManager with the NPC Index and its default goodwill value
        }
        
        #endregion

        private void Update()
        {
            if (!GameState.Instance.IsGameStarted)
            {
                return;
            }
            if (!pollData && m_PlayerReputationController == null)
            {
                m_PlayerReputationController = FindObjectOfType<PlayerReputationController>();
                if (m_PlayerReputationController != null)
                {
                    Debug.Log("ReputationManager pollData OK");
                    pollData = true;
                }
            }
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= 1)
            {
                ElapsedTime = 0;
                if (m_ReputationStatus == ReputationStatusType.NEGATIVE)
                {
                    m_CurrentTimeInNegativeReputation += Time.deltaTime;
                    if (m_CurrentTimeInNegativeReputation >= m_MaxTimeInNegativeReputation)
                    {
                        Debug.Log("Player has been in negative reputation for too long");
                        // TODO: Implement a game over screen
                    }
                }
                else
                {
                    m_CurrentTimeInNegativeReputation = 0;
                }
            }
        }

        #region Flow
        
        /// <summary>
        /// Add reputation to the player when they share an item with NPC
        /// </summary>
        /// <param name="amount"></param>
        public void AddReputation(float amount, int NpcIndex)
        {
            // Min 0, Max 100
            m_CurrentReputation += amount;
            m_CurrentReputation = Mathf.Clamp(m_CurrentReputation, 0, m_MaxGeneralReputation);
            bool NPCExists = NPCGoodWillList.Exists(x => x.Key == NpcIndex);
            if (NPCExists)
            {
                Debug.Log("Reputation added with NPC Index: " + NpcIndex);
                NPCGoodWillList.Find(x => x.Key == NpcIndex).Value += amount;
            }
            else
            {
                Debug.Log("New NPC -> Reputation added with NPC Index: " + NpcIndex);
                NPCGoodWillList.Add(new SerializableDictionaryEntry<int, float>(NpcIndex, amount));
            }
            Debug.Log("Reputation: " + m_CurrentReputation);
            m_PlayerReputationController.UpdateReputation(m_CurrentReputation);
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
    }
}