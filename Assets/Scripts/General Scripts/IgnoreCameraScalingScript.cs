﻿using UnityEngine;
using System.Collections;

public class IgnoreCameraScalingScript : MonoBehaviour 
{
    private Camera theCamera = null;

    private Vector3 initialScale = Vector3.zero;

    private float prevOrthoSize = 0.0f;

	// Use this for initialization
	void Start () 
    {
        theCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        prevOrthoSize = theCamera.orthographicSize;

        initialScale = gameObject.transform.localScale;

        NegateScaling();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (theCamera.orthographicSize != prevOrthoSize)
        {
            NegateScaling();
            prevOrthoSize = theCamera.orthographicSize;
        }
	}

    private void NegateScaling()
    {
        gameObject.transform.localScale = new Vector3(theCamera.orthographicSize/initialScale.x, theCamera.orthographicSize/initialScale.y, 1);
    }
}
