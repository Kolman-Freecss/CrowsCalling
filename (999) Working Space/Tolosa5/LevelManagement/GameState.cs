using System;
using System.Collections;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule;
using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule.Model;
using UnityEngine;

/*
     * Each level has a Time Management script.

        Time Manager has helper functions for the passage of time in a day, and variables that are scalable

        Level progression is done by “days”.

        Level Management script handles the “Days”, so days increment after a variable of time.

        helper function to increment hours(useful for traveling script)
        boolean function for whether it’s daylight or not
        scalable variables that set the daylight hours(military time)
     */

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    
    
    [SerializeField] int maxDays = 7;
    [SerializeField] private float maxDayTime = 24;
    [SerializeField] private int fixedTimeSpent = 3;

    [SerializeField] private int maxNPCs = 6;
    
    private bool isGameStarted = false; // Modified by the SceneTransitionHandler
    public bool IsGameStarted
    {
        get => isGameStarted;
        set => isGameStarted = value;
    }
    
    #region Variable Data (Will need to be reset)

    private int currentDay = 1;

    private int currentHour = 6;
    
    private Dictionary<TownType, List<NPCInteractable>> npcs = new Dictionary<TownType, List<NPCInteractable>>();
    
    #endregion

    #region DayVariables

    private int daylightStart = 6;
    
    private int daylightEnd = 18;
    
    public TimeOfTheDay currentTimeOfTheDay = TimeOfTheDay.Morning;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// When the player leaves the game, the data must be reset
    /// </summary>
    public void Reset()
    {
        currentDay = 1;
        currentHour = 6;
        currentTimeOfTheDay = TimeOfTheDay.Morning;
        
        npcs.Clear();
    }
    
    
    public void AddToNPCDictionary(TownType town, NPCInteractable npc)
    {
        //TODO: Check if contains is based on the memory place of the object, not the values
        bool townExists = npcs.ContainsKey(town);
        if (!townExists)
        {
            Debug.Log("Town not found. Adding town to the dictionary.");
            npcs.Add(town, new List<NPCInteractable>());
        }
        bool npcExists = npcs.ContainsKey(town) && npcs[town].Contains(npc);
        if (npcExists)
        {
            Debug.LogWarning("(Travelling) NPC already exists in the dictionary. ");
            return;
        }
        npcs[town].Add(npc);
    }
    
    
    public void CheckSatisfiedNPCs()
    {
        if (npcs.Count < maxNPCs)
            return;

        foreach (KeyValuePair<TownType, List<NPCInteractable>> npcDict in npcs)
        {
            foreach (NPCInteractable npc in npcDict.Value)
            {
                if (npc.GetComponent<RequestHandler>().isRequestCompleted == false)
                    return;
            }
        }
        
        //If all requests are completed, win the game
        OnGameEnd(true);
    }

    #region TimeManagement

    public void IncreaseTimeTranscurred(int hoursToAdd)
    {
        currentHour += hoursToAdd;
        Debug.Log("Current hour: " + currentHour);
        if (currentHour >= daylightEnd)
        {
            SetIsDaylight(TimeOfTheDay.Night);
        }
        else
        {
            SetIsDaylight(TimeOfTheDay.Morning);
        }
        
        if (currentHour >= maxDayTime)
        {
            GoToNextDay();
        }
    }
    
    public void GoToSleep()
    {
        // TODO: Add a sleep animation and transition. Fade in and out ?
        IncreaseTimeTranscurred(fixedTimeSpent);
    }
    
    public void GoToNextDay()
    {
        currentDay++;
        OnDayChanged();
    }

    #endregion
    

    #region GameLogic

    public void OnGameStart()
    {
        Debug.Log("Game started.");
    }

    public void OnGameEnd(bool win)
    {
        Debug.Log("Game ended.");
        if (win)
        {
            Debug.Log("You win!");
            
        }
        else
        {
            Debug.Log("You lose!");
        }
    }
    
    public void OnDayChanged()
    {
        Debug.Log("Day changed.");
        currentHour = 6;
        currentTimeOfTheDay = TimeOfTheDay.Morning;
    }

    public void OnTownChange()
    {
        if (SceneTransitionHandler.Instance.GetPreviousSceneState() == SceneTypes.Main)
            return;
        
        Debug.Log("Town changed.");
        IncreaseTimeTranscurred(fixedTimeSpent);
    }

    #endregion
    

    #region Getters and Setters

    public int GetCurrentDay()
    {
        return currentDay;
    }
    
    //TODO: Get in a canvas to place it in the UI
    public float GetTimeTranscurred()
    {
        return currentHour;
    }
    
    
    public TimeOfTheDay GetIsDaylight()
    {
        return currentTimeOfTheDay;
    }
    
    public void SetIsDaylight(TimeOfTheDay timeOfTheDay)
    {
        currentTimeOfTheDay = timeOfTheDay;
    }
    
    
    public Dictionary<TownType, List<NPCInteractable>> GetNPCDictionary()
    {
        return npcs;
    }

    #endregion
    
}
