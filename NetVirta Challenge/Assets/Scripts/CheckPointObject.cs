using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObject : MonoBehaviour {

    public int m_ID;
    public float m_Size;
    public Trail m_TrailObject;
    
    public void OnScanned()
    {
        Renderer rend = GetComponent<Renderer>();

        //Set the main Color of the Material to green
        rend.material.color = Color.green;
        m_Size = transform.localScale.x;

        StartCoroutine(Shrink());
    }

    public IEnumerator Shrink()
    {
        while (m_Size > 0.1f)
        {
            transform.localScale = new Vector3(m_Size, m_Size, m_Size);
            m_Size -= 0.5f * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        m_TrailObject.Play(CheckPointManager.instance.GetNextCheckPointLoaction());
    }
    

    public void StopTrail()
    {
        m_TrailObject.Stop();
    }
}
