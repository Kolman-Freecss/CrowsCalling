using System;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.Utils;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.AudioModule
{
    /// <summary>
    /// This class will be the responsible for managing the crows calling tracks orchestration
    /// </summary>
    public class SoundTrackCrowsManager : MonoBehaviour
    {
        
        #region Member Variables
        
        public static SoundTrackCrowsManager Instance { get; private set; }
        
        #endregion
        
        #region Variable Data (Will need to be reset)
        
        [SerializeField] 
        private AudioSource indexOneAudioSource;
        
        [SerializeField]
        private AudioSource indexTwoAudioSource;
        
        [SerializeField]
        private AudioSource indexThreeAudioSource;
        
        [SerializeField]
        private AudioSource indexFourAudioSource;
        
        [SerializeField]
        private AudioSource indexFiveAudioSource;
        
        [SerializeField]
        private AudioSource indexSixAudioSource;
        
        // This will be used to track all the audios when every request is made
        [SerializeField]
        private List<SerializableDictionaryEntry<int, AudioClip>> crowsCallingClips;
        
        #endregion
        
        #region Crows Calling
        
        private void Awake()
        {
            ManageSingleton();
        }

        void ManageSingleton()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        /// <summary>
        /// When the player leaves the game, the data must be reset
        /// </summary>
        public void Reset()
        {
            indexOneAudioSource.Stop();
            indexTwoAudioSource.Stop();
            indexThreeAudioSource.Stop();
            indexFourAudioSource.Stop();
            indexFiveAudioSource.Stop();
            indexSixAudioSource.Stop();
        }

        private void Start()
        {
            Reset();
        }
        
        // Another MONSTER function!!! :(
        public void PlayAllCrowsCalling(bool playAll = true)
        {
            if (playAll)
            {
                SoundManager.Instance.StopBackgroundMusic();
            }
            else
            {
                SoundManager.Instance.StartPreviousBackground();
            }
            crowsCallingClips.ForEach(x =>
            {
                switch (x.Key)
                {
                    case 0:
                        if (playAll)
                        {
                            indexOneAudioSource.clip = x.Value;
                            indexOneAudioSource.Play();
                        }
                        else
                        {
                            indexOneAudioSource.Stop();
                        }
                        break;
                    case 1:
                        if (playAll)
                        {
                            indexTwoAudioSource.clip = x.Value;
                            indexTwoAudioSource.Play();
                        }
                        else
                        {
                            indexTwoAudioSource.Stop();
                        }
                        break;
                    case 2:
                        if (playAll)
                        {
                            indexThreeAudioSource.clip = x.Value;
                            indexThreeAudioSource.Play();
                        }
                        else
                        {
                            indexThreeAudioSource.Stop();
                        }
                        break;
                    case 3:
                        if (playAll)
                        {
                            indexFourAudioSource.clip = x.Value;
                            indexFourAudioSource.Play();
                        }
                        else
                        {
                            indexFourAudioSource.Stop();
                        }
                        break;
                    case 4:
                        if (playAll)
                        {
                            indexFiveAudioSource.clip = x.Value;
                            indexFiveAudioSource.Play();
                        }
                        else
                        {
                            indexFiveAudioSource.Stop();
                        }
                        break;
                    case 5:
                        if (playAll)
                        {
                            indexSixAudioSource.clip = x.Value;
                            indexSixAudioSource.Play();
                        }
                        else
                        {
                            indexSixAudioSource.Stop();
                        }
                        break;
                    default:
                        Debug.LogError("No audio source found for the npc index: " + x.Key);
                        break;
                }
            });
        }

        /// <summary>
        /// Invoked when the player made a request from NPC
        /// </summary>
        /// <param name="npcIndex"></param>
        public void StartNPCTrackReferencedByIndex(int npcIndex)
        {
            AudioClip clip = crowsCallingClips.Find(x => x.Key == npcIndex).Value;
            if (clip == null)
            {
                Debug.LogError("No audio clip found for the npc index: " + npcIndex);
                return;
            }

            // Monster function!!! :(
            switch (npcIndex)
            {
                case 0:
                    indexOneAudioSource.clip = clip;
                    indexOneAudioSource.Play();
                    break;
                case 1:
                    indexTwoAudioSource.clip = clip;
                    indexTwoAudioSource.Play();
                    break;
                case 2:
                    indexThreeAudioSource.clip = clip;
                    indexThreeAudioSource.Play();
                    break;
                case 3:
                    indexFourAudioSource.clip = clip;
                    indexFourAudioSource.Play();
                    break;
                case 4:
                    indexFiveAudioSource.clip = clip;
                    indexFiveAudioSource.Play();
                    break;
                case 5:
                    indexSixAudioSource.clip = clip;
                    indexSixAudioSource.Play();
                    break;
                default:
                    Debug.LogError("No audio source found for the npc index: " + npcIndex);
                    break;
            }
        }

        #endregion
    }
}