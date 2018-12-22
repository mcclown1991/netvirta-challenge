using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {

    public static CheckPointManager instance = null;

    public Tracking start;
    public GameObject m_CheckPointPrefab;

    [Header("Debug")]
    public float m_Radius;
    public uint m_NumberOfCheckPoints;
    public List<GameObject> m_CheckPoints;

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
        start = obj;

        /*  For a circle with origin (j, k) and radius r:

            x(t) = r cos(t) + j
            y(t) = r sin(t) + k

            where you need to run this equation for t taking values within the range from 0 to 360, then you will get your x and y each on the boundary of the circle.
         */

        // find intervals of circle
        //uint interval = 360 / m_NumberOfCheckPoints;

        //for(uint i = 0; i < 360; i += interval)
        //{
        //    GameObject cp = Instantiate(m_CheckPointPrefab, new Vector3(m_Radius * Mathf.Cos(i * Mathf.Deg2Rad) + start.position.x, start.position.y, m_Radius * Mathf.Sin(i * Mathf.Deg2Rad) + start.position.z), Quaternion.identity);
        //    m_CheckPoints.Add(cp);
        //}

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
            GameObject cp = Instantiate(m_CheckPointPrefab, new Vector3(r * Mathf.Cos(phi) + start.position.x, y + start.position.y, r * Mathf.Sin(phi) + start.position.z), Quaternion.identity);
            m_CheckPoints.Add(cp);
        }
    }
}
