using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTraceTarget : MonoBehaviour {

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
                col.OnRayHit();
            }
        }
    }
}
