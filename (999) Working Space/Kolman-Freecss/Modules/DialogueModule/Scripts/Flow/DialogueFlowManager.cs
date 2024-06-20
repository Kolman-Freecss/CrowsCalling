// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Components;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Model;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.UI;
using _999__Working_Space.Kolman_Freecss.Modules.ReputationModule;
using UnityEngine;
using UnityEngine.Events;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow
{
    public enum DialogueState
    {
        InDialogue,
        InGame
    }
    
    public class DialogueFlowManager : MonoBehaviour
    {
        private static DialogueFlowManager ms_Instance;
        public static DialogueFlowManager Instance => ms_Instance;
        
        public UnityEvent<DialogueNode> OnDialogueStart;
        public UnityEvent<DialogueNode> OnDialogueNext;
        public UnityEvent<DialogueNode> OnDialogueEnd;
        
        #region Variable Data (Will need to be reset)
        
        [HideInInspector]
        private DialogueState m_CurrentDialogueState = DialogueState.InGame;
        
        public DialogueState CurrentDialogueState => m_CurrentDialogueState;
        
        [SerializeField]
        private UIDialogueController m_DialogueController;
        
        // Store previous choice selected by the player through the scenes
        private Dictionary<int, ChoiceOptionType> m_PreviousChoiceSelected = new Dictionary<int, ChoiceOptionType>();
        public Dictionary<int, ChoiceOptionType> PreviousChoiceSelected => m_PreviousChoiceSelected;
        // We've 3 possible bools for each thread (because we've 3 possible threads in every NPC)
        private Dictionary<int, List<bool>> m_NpcTrackerThreadThrown = new Dictionary<int, List<bool>>();
        public Dictionary<int, List<bool>> NpcTrackerThreadThrown => m_NpcTrackerThreadThrown;
        
        private NPCDialogueController m_CurrentNPCDialogueController;
        
        private bool pollData = false;

        #endregion

        
        #region Init Data

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
                ms_Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        /// When the player leaves the game, the data must be reset
        /// </summary>
        public void Reset()
        {
            m_CurrentDialogueState = DialogueState.InGame;
            m_CurrentNPCDialogueController = null;
            m_PreviousChoiceSelected.Clear();
            m_NpcTrackerThreadThrown.Clear();
            m_DialogueController = null;
            pollData = false;
        }

        private void Start()
        {
            Reset();
        }

        public bool IsThreadThrown(int npcIndex, int threadIndex)
        {
            if (m_NpcTrackerThreadThrown.ContainsKey(npcIndex))
            {
                if (m_NpcTrackerThreadThrown[npcIndex].Count > threadIndex)
                {
                    return m_NpcTrackerThreadThrown[npcIndex][threadIndex];
                }
                else
                {
                    Debug.LogError("Thread index out of range");
                }
            }
            else
            {
                Debug.LogError("NPC index not found");
            }
            return false;
        }
        
        /// <summary>
        /// Thread index MAX is 2 (because we've 3 possible threads in every NPC)
        /// IMPORTANT (As a workaround) Index MUST be NPCDialogueController.Index
        /// </summary>
        /// <param name="npcIndex"></param>
        /// <param name="threadIndex"></param>
        /// <param name="threadThrown"></param>
        public void UpdateNPCThread(int npcIndex, int threadIndex, bool threadThrown)
        {
            if (m_NpcTrackerThreadThrown.ContainsKey(npcIndex))
            {
                if (m_NpcTrackerThreadThrown[npcIndex].Count > threadIndex)
                {
                    m_NpcTrackerThreadThrown[npcIndex][threadIndex] = threadThrown;
                }
                else
                {
                    Debug.LogError("Thread index out of range");
                }
            }
            else
            {
                Debug.LogError("NPC index not found");
            }
        }
        
        public void AddNpcToTracker(int npcIndex)
        {
            if (!m_NpcTrackerThreadThrown.ContainsKey(npcIndex))
            {
                m_NpcTrackerThreadThrown.Add(npcIndex, new List<bool> {false, false, false});
            }
            else if (m_NpcTrackerThreadThrown[npcIndex].Count > 0)
            {
                Debug.LogWarning("NPC already added to the tracker");
            }
        }
        
        public ChoiceOptionType GetNPCChoice(int npcIndex)
        {
            if (m_PreviousChoiceSelected.ContainsKey(npcIndex))
            {
                return m_PreviousChoiceSelected[npcIndex];
            }
            else
            {
                Debug.LogError("NPC index not found");
            }
            return ChoiceOptionType.NONE;
        }
        
        /// <summary>
        /// IMPORTANT (As a workaround) Index MUST be NPCDialogueController.Index
        /// </summary>
        /// <param name="npcIndex"></param>
        /// <param name="choiceOptionType"></param>
        public void UpdateNPCChocie(int npcIndex, ChoiceOptionType choiceOptionType)
        {
            if (m_PreviousChoiceSelected.ContainsKey(npcIndex))
            {
                m_PreviousChoiceSelected[npcIndex] = choiceOptionType;
            }
            else
            {
                Debug.LogError("NPC index not found");
            }
        }
        
        public void AddNpcToChoiceTracker(int npcIndex)
        {
            if (!m_PreviousChoiceSelected.ContainsKey(npcIndex))
            {
                m_PreviousChoiceSelected.Add(npcIndex, ChoiceOptionType.NONE);
            }
            else if (m_PreviousChoiceSelected[npcIndex] != ChoiceOptionType.NONE)
            {
                Debug.LogWarning("NPC already added to the choice tracker");
            }
        }
        
        public void AddNPCThread(int npcIndex, int threadIndex)
        {
            if (!m_NpcTrackerThreadThrown.ContainsKey(npcIndex))
            {
                m_NpcTrackerThreadThrown.Add(npcIndex, new List<bool>());
            }
            m_NpcTrackerThreadThrown[npcIndex].Add(false);
        }

        private void Update()
        {
            if (!GameState.Instance.IsGameStarted)
            {
                return;
            }
            if (!pollData && m_DialogueController == null)
            {
                m_DialogueController = FindObjectOfType<UIDialogueController>();
                if (m_DialogueController != null)
                {
                    Debug.Log("DialogueFlowManager pollData OK");
                    pollData = true;
                }
            }
        }

        private void HideUIWidget(in bool hide = true)
        {
            m_DialogueController.gameObject.SetActive(!hide);
            m_DialogueController.Hide(m_DialogueController.GetCurrentContainerType(), hide);
        }
        
        public void OnStartDialogue(DialogueNode dialogueNode, NPCDialogueController npcDialogueController)
        {
            m_CurrentDialogueState = DialogueState.InDialogue;
            m_CurrentNPCDialogueController = npcDialogueController;
            HideUIWidget(false);
            m_DialogueController.OnDialogueNext(dialogueNode);
            OnDialogueStart.Invoke(dialogueNode);
        }
        
        public void OnNextDialogueNode(DialogueNode dialogueNode)
        {
            m_CurrentDialogueState = DialogueState.InDialogue;
            HideUIWidget(false);
            m_DialogueController.OnDialogueNext(dialogueNode);
            OnDialogueNext.Invoke(dialogueNode);
        }
        
        public void OnEndDialogue(DialogueNode dialogueNode)
        {   
            m_CurrentDialogueState = DialogueState.InGame;
            m_CurrentNPCDialogueController = null;
            m_DialogueController.OnDialogueEnd(dialogueNode);
            OnDialogueEnd.Invoke(dialogueNode);
        }
        
        public void OnDialogueChoice(DialogueNode dialogueNode, in ChoiceDialogueOption choice)
        {
            int NPCIndex = 0;
            if (m_CurrentNPCDialogueController != null &&
                m_CurrentNPCDialogueController.GetComponent<NPCInteractable>() != null)
            {
                NPCIndex = m_CurrentNPCDialogueController.GetComponent<NPCInteractable>().Index;
            }
            Debug.Log("Dialogue choice made -> " + choice.Index + ", Text : " + choice.DialogueLine.Text + ", NPCIndex : " + NPCIndex);
            float reputationForChoice = 0f;
            switch (choice.ChoiceOptionType)
            {
                case ChoiceOptionType.BAD:
                    reputationForChoice = ReputationManager.Instance.m_ReputationAmountForBadDialogueChoice;
                    break;
                case ChoiceOptionType.BEST:
                    reputationForChoice = ReputationManager.Instance.m_ReputationAmountForBestDialogueChoice;
                    break;
                case ChoiceOptionType.GOOD:
                    reputationForChoice = ReputationManager.Instance.m_ReputationAmountForGoodDialogueChoice;
                    break;
                case ChoiceOptionType.NEUTRAL:
                    reputationForChoice = ReputationManager.Instance.m_ReputationAmountForNeutralDialogueChoice;
                    break;
            }
            UpdateNPCChocie(GetNpcDialogueIndexFromCurrentNpc(), choice.ChoiceOptionType);
            ReputationManager.Instance.AddReputation(reputationForChoice, NPCIndex);
            if (m_CurrentNPCDialogueController != null)
            {
                m_CurrentNPCDialogueController.Next();
            }
            else
            {
                Debug.LogError("No NPCDialogueController to continue the dialogue");
            }
        }
        
        // TODO: This is a workaround. Unify index's in the future
        public int GetNpcDialogueIndexFromCurrentNpc()
        {
            NPCDialogueController npcDialogueController = m_CurrentNPCDialogueController.GetComponent<NPCDialogueController>();
            if (npcDialogueController != null)
            {
                return npcDialogueController.GetIndex;
            }

            Debug.LogError("No NPCDialogueController found");
            return -1;
        }
        
        public DialogueContainerType GetCurrentContainerType()
        {
            if (m_DialogueController == null)
            {
                Debug.LogError("No UIDialogueController found");
                return DialogueContainerType.Dialogue;
            }
            return m_DialogueController.GetCurrentContainerType();
        }
        
        #endregion
        
        #region Destructor
        
        private void OnDestroy()
        {
            OnDialogueStart.RemoveAllListeners();
            OnDialogueNext.RemoveAllListeners();
            OnDialogueEnd.RemoveAllListeners();
            if (ms_Instance == this)
            {
                ms_Instance = null;
            }
        }
        
        #endregion
    }
}