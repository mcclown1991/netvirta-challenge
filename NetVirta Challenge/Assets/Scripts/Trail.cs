using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {

    [SerializeField]
    Vector3 m_Destination;
    Vector3 m_Home;

    private void Start()
    {
        m_Home = transform.position;
    }

    /// <summary>
    /// Starts playing the trail animation
    /// </summary>
    /// <param name="dest"></param>
    public void Play(Vector3 dest)
    {
        m_Destination = dest;
        if (m_Destination == m_Home) return;
        StartCoroutine(MoveToDestination());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveToDestination()
    {
        while (Vector3.Distance(transform.position, m_Destination) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position, m_Destination, 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(MoveToHome());
    }

    IEnumerator MoveToHome()
    {
        while (Vector3.Distance(transform.position, m_Home) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Home, 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(MoveToDestination());
    }
}
