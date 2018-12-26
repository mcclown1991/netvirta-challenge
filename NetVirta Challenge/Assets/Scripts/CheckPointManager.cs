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
    public int prevCP = -1;
    public int m_ScannedCount = 0;

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
            GameObject cp = Instantiate(m_CheckPointPrefab, new Vector3(Mathf.Cos(phi) * r * m_Radius, y * m_Radius, Mathf.Sin(phi) * r * m_Radius) , Quaternion.identity);
            CheckPointObject cpobj = cp.GetComponent<CheckPointObject>();
            cpobj.m_ID = m_CheckPoints.Count;
            cp.SetActive(false);
            m_CheckPoints.Add(cpobj);
            m_ScannedList.Add(false);
        }

        StartCoroutine(GenerateCheckPoints());
    }

    /// <summary>
    /// Shows the checkpoint in a sequence 
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateCheckPoints()
    {
        foreach(CheckPointObject obj in m_CheckPoints)
        {
            obj.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// The origin of the marker is lost, hide all objects
    /// </summary>
    public void LostTrackingObject()
    {
        foreach(CheckPointObject obj in m_CheckPoints)
        {
            if(obj != null)
            obj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if a checkpoint is scanned
    /// </summary>
    /// <param name="id">id of the checkpoint</param>
    /// <returns>status of the checkpoint</returns>
    public bool CheckScanStatus(int id)
    {
        return m_ScannedList[id];
    }

    /// <summary>
    /// Calls when a checkpoint is successfully scanned
    /// </summary>
    /// <param name="id"></param>
    public void OnScannedCheckPoint(int id)
    {
        m_ScannedList[id] = true;
        if (prevCP != -1) m_CheckPoints[prevCP].StopTrail();
        ++m_ScannedCount;
    }

    /// <summary>
    /// This function updates the circle for indicating scanning progress
    /// </summary>
    public void UpdateScaningBar()
    {
        m_ScanningBar.fillAmount += 1 * Time.deltaTime;
        if(m_ScanningBar.fillAmount >= 1)
        {
            OnScannedCheckPoint(tracedCP);
            m_CheckPoints[tracedCP].OnScanned();
            m_ScanningBar.fillAmount = 0;
            prevCP = tracedCP;
        }
        
    }

    /// <summary>
    /// Resets the scanning progress bar
    /// </summary>
    public void StopScanningBar()
    {
        m_ScanningBar.fillAmount = 0;
    }

    /// <summary>
    /// report the Raytrace result
    /// </summary>
    /// <param name="id"></param>
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
    }

    /// <summary>
    /// find the next checkpoint to scan with right movement first before left 
    /// </summary>
    /// <returns>position of the next checkpoint</returns>
    public Vector3 GetNextCheckPointLoaction()
    {
        if (m_ScannedCount == m_NumberOfCheckPoints) return m_CheckPoints[prevCP].transform.position;
        for(int i = prevCP + 1; i < m_CheckPoints.Count; ++i)
        {
            if (m_ScannedList[i] == false)
            {
                return m_CheckPoints[i].transform.position;
            }
        }
        // if it reaches here there is no more next aval, search for previous
        
        for(int i = prevCP; i > 0; --i)
        {
            if (m_ScannedList[i] == false)
                return m_CheckPoints[i].transform.position;
        }

        // this return is to prevent error in c#
        return m_CheckPoints[prevCP].transform.position;
    }
}
