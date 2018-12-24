using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTraceTarget : MonoBehaviour {

    private int id = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            CheckPointObject col = hit.transform.gameObject.GetComponent<CheckPointObject>();
            if (col != null)
            {
                if (id == -1)
                {// no previous reported hits
                    id = col.m_ID;
                    CheckPointManager.instance.StopScanningBar();
                    Debug.Log("Stopped at ray 1");
                    CheckPointManager.instance.ReportRaytracedCP(col.m_ID);
                }
                else if(id == col.m_ID){
                    // same object
                }
                else{
                    // different object
                    CheckPointManager.instance.StopScanningBar();
                    Debug.Log("Stopped at ray 3");
                    CheckPointManager.instance.ReportRaytracedCP(id);
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
                CheckPointManager.instance.ReportRaytracedCP(-1);
            }
        }
    }
}
