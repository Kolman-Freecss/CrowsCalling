using _999__Working_Space.Kolman_Freecss.Modules.SceneManagerModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameTitle : MonoBehaviour
{
    [SerializeField] private GameObject gobj_GameTitle = null;
    [SerializeField] private GameObject gobj_GameStartGameUI = null;
    [SerializeField] private GameObject gobj_GameStartIMGBanner = null;
    //[SerializeField] private GameObject gobj_GameStartSelectionUI = null;
        
    // Start is called before the first frame update
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnUpdate()    
    {
        if (Input.GetKeyDown(KeyCode.F1)) // Start Game, kill menu ui
        {
            DisableGameTitleUI();
            this.gameObject.SetActive(false);
        }
        
    }
    public void DisableGameTitleUI()
    {
        //Sanitycheck null values & silently fail
        if (gobj_GameTitle == null || gobj_GameStartIMGBanner == null || gobj_GameTitle == null)
        {
            return;
        }
        gobj_GameStartGameUI.SetActive(false);
        gobj_GameStartIMGBanner.SetActive(false);
        gobj_GameTitle.SetActive(false);

    }
}
