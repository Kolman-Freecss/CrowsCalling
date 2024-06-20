using System;
using System.Collections.Generic;
using _999__Working_Space.Tolosa5.Inventory.Model;
using UnityEngine;

namespace _999__Working_Space.Tolosa5.RequestSystem
{
    public struct RequestPayload
    {
        private ObjectType itemType;
        private Sprite requestedSprite;
        private bool isRequestMade;
        private bool isCanBeGifted;
        private bool isRequestCompleted; // TODO: NOT USED but will be used in the future
        
        public RequestPayload(ObjectType itemType, Sprite requestedSprite, bool isRequestMade, bool isCanBeGifted, bool isRequestCompleted)
        {
            this.itemType = itemType;
            this.requestedSprite = requestedSprite;
            this.isRequestMade = isRequestMade;
            this.isCanBeGifted = isCanBeGifted;
            this.isRequestCompleted = isRequestCompleted;
        }
        
        public ObjectType ItemType => itemType;
        public Sprite RequestedSprite => requestedSprite;
        public bool IsRequestMade => isRequestMade;
        public bool IsCanBeGifted => isCanBeGifted;
        public bool IsRequestCompleted => isRequestCompleted;
    }
    
    public class RequestFlowManager : MonoBehaviour
    {
        private static RequestFlowManager ms_Instance;
        public static RequestFlowManager Instance => ms_Instance;
        
        #region Variable Data (Will need to be reset)
        
        // DDBB with the requests stored from each NPC
        private Dictionary<int, RequestPayload> m_RequestPayloads = new Dictionary<int, RequestPayload>();
        
        private bool pollData = false;
        
        #endregion

        #region InitData

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
            m_RequestPayloads.Clear();
            pollData = false;
        }

        private void Start()
        {
            Reset();
        }

        #endregion

        #region Flow

        // private void Update()
        // {
        //     if (!GameState.Instance.IsGameStarted)
        //     {
        //         return;
        //     }
        // }
        
        public void AddNpcToTrackerRequests(int npcIndex)
        {
            if (!m_RequestPayloads.ContainsKey(npcIndex))
            {
                m_RequestPayloads.Add(npcIndex, new RequestPayload());
            }
        }
        
        public RequestPayload GetRequestPayload(int npcIndex)
        {
            if (m_RequestPayloads.ContainsKey(npcIndex))
            {
                return m_RequestPayloads[npcIndex];
            }

            return new RequestPayload();
        }
        
        public void UpdateRequestPayload(int npcIndex, RequestPayload requestPayload)
        {
            if (m_RequestPayloads.ContainsKey(npcIndex))
            {
                m_RequestPayloads[npcIndex] = requestPayload;
            }
            else
            {
                m_RequestPayloads.Add(npcIndex, requestPayload);
            }
        }

        #endregion
    }
}