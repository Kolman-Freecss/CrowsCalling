using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QUIT_GAME : MonoBehaviour
{
   [SerializeField] private KeyCode quitKey = KeyCode.F3;
    // Update is called once per frame
    void Update()
        {
            // Check if the F1 key (default) is pressed
            if (Input.GetKeyDown(quitKey))
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            // Quit the application
            Application.Quit();
            }
        }
    public void UI_BTN_QuitGame()
    {
        Application.Quit();
    }
}
