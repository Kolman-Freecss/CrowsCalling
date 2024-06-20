using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLootAtPlayer : MonoBehaviour
{
    private Camera mainCamera;
    
    private void Start() 
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}
