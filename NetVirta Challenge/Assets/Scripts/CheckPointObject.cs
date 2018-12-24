using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObject : MonoBehaviour {

    public int m_ID;
    public bool m_Scanning;

	public void OnRayHit()
    {
        Debug.Log("Ray Hit!");
        if (!CheckPointManager.instance.CheckScanStatus(m_ID))
        {
            Debug.Log("Start scanning check point");
            m_Scanning = true;
        }
        else
        {
            Debug.Log("Check point is done");
            m_Scanning = false;
        }
    }
}
