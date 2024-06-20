using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectXAxis : MonoBehaviour
{
	public Boolean shouldRotate = true;
	[Range(-100.0f, 100.0f)]
	public float rotateSpeed = 10f;


	public void ShouldRotate(bool b)
	{ if (b == true) shouldRotate = true;
	else shouldRotate = false;
	}
	
	public void ChangeRotationSpeed(float value)
	{
		if (value <= 100.0f && value >= -100.0f)
			rotateSpeed = value;
		else
			return;
	}
    // Update is called once per frame
    void Update()
    {
        if (shouldRotate) 
        { 
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
        }
    }
}
