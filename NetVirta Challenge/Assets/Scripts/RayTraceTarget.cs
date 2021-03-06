﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTraceTarget : MonoBehaviour {

    private int id = -1;
    public float m_Deviation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public int CastRay () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            // check if camera is facing the origin
            float rad = Vector3.Angle(transform.forward, Vector3.zero - transform.position.normalized);
            if(rad > m_Deviation)
            {
                // object lost
                id = -1;
                CheckPointManager.instance.StopScanningBar();
                Debug.Log("Camera orientation is not facing the marker");
                CheckPointManager.instance.StartPulseOnLastObject();
                return -1;
            }

            CheckPointObject col = hit.transform.gameObject.GetComponent<CheckPointObject>();
            if (col != null)
            {
                if (id == -1)
                {// no previous reported hits
                    id = col.m_ID;
                    CheckPointManager.instance.StopScanningBar();
                    Debug.Log("Stopped at ray 1");
                    CheckPointManager.instance.StopPulse(-1);
                    return col.m_ID;
                }
                else if(col.m_ID == id)
                {
                    return id;
                }
                else{
                    // different object
                    CheckPointManager.instance.StopScanningBar();
                    Debug.Log("Stopped at ray 3");
                    CheckPointManager.instance.StopPulse(id);
                    return id;
                }
                
            }
        }
        else
        {
            if (id != -1)
            {
                // object lost
                id = -1;
                CheckPointManager.instance.StopScanningBar();
                Debug.Log("Stopped at no ray");
                CheckPointManager.instance.StartPulseOnLastObject();
                return -1;
            }
        }

        return -1;
    }
}
