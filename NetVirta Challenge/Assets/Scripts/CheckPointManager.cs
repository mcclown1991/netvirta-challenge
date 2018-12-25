using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {

    public static CheckPointManager instance = null;

    public Tracking start;
    public GameObject m_CheckPointPrefab;
    public UnityEngine.UI.Image m_ScanningBar;

    [Header("Debug")]
    public float m_Radius;
    public uint m_NumberOfCheckPoints;
    public List<CheckPointObject> m_CheckPoints = new List<CheckPointObject>();
    private List<bool> m_ScannedList = new List<bool>();
    public int tracedCP = -1;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
	
    public void ReportTrackingObject(Tracking obj)
    {
        Debug.Log("Reported trackable");
        // report start point
        if (start != null)
        {
            foreach(CheckPointObject cp in m_CheckPoints)
            {
                cp.gameObject.SetActive(true);
            }
            return;
        }
        start = obj;

        // Generate DOme of points
        /*
         *function sphere ( N:float,k:int):Vector3 {
            var inc =  Mathf.PI  * (3 - Mathf.Sqrt(5));
            var off = 2 / N;
            var y = k * off - 1 + (off / 2);
            var r = Mathf.Sqrt(1 - y*y);
            var phi = k * inc;
            return Vector3((Mathf.Cos(phi)*r), y, Mathf.Sin(phi)*r); 
        }; 
         */

        for(uint i = 0; i < m_NumberOfCheckPoints; ++i)
        {
            float inc = Mathf.PI * (3f - Mathf.Sqrt(5));
            float off = 2.0f / ((float)m_NumberOfCheckPoints);
            float y = i * (off / 2);
            float r = Mathf.Sqrt(1 - y * y);
            float phi = i * inc;
            GameObject cp = Instantiate(m_CheckPointPrefab, new Vector3(r * Mathf.Cos(phi), y, r * Mathf.Sin(phi)), Quaternion.identity);
            CheckPointObject cpobj = cp.GetComponent<CheckPointObject>();
            cpobj.m_ID = m_CheckPoints.Count;
            m_CheckPoints.Add(cpobj);
            m_ScannedList.Add(false);
        }
    }

    public void LostTrackingObject()
    {
        foreach(CheckPointObject obj in m_CheckPoints)
        {
            if(obj != null)
            obj.gameObject.SetActive(false);
        }
    }

    public bool CheckScanStatus(int id)
    {
        return m_ScannedList[id];
    }

    public void OnScannedCheckPoint(int id)
    {
        m_ScannedList[id] = true;
    }

    public void UpdateScaningBar()
    {
        m_ScanningBar.fillAmount += 1 * Time.deltaTime;
        if(m_ScanningBar.fillAmount >= 1)
        {
            OnScannedCheckPoint(tracedCP);
            m_CheckPoints[tracedCP].OnScanned();
            m_ScanningBar.fillAmount = 0;
        }
        
    }

    public void StopScanningBar()
    {
        m_ScanningBar.fillAmount = 0;
    }

    public void ReportRaytracedCP(int id)
    {
        tracedCP = id;
    }

    private void Update()
    {
        if(tracedCP != -1)
        {
            // traced a checkpoint
            if(!m_ScannedList[tracedCP])
            UpdateScaningBar();
        }

        //Debug.Log(m_ScanningBar.fillAmount);
    }
}
