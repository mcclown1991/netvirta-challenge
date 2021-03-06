﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObject : MonoBehaviour {

    public int m_ID;
    public float m_Size;
    public Trail m_TrailObject;
    private bool m_IsPulse = false;
    private float m_Delay = 0.2f;

    public void StartPulse()
    {
        if (m_IsPulse) return;
        m_IsPulse = true;
        StartCoroutine(PulseDown());
    }

    public void StopPulse()
    {
        StopAllCoroutines();

        m_Size = 0.2f;
        transform.localScale = new Vector3(m_Size, m_Size, m_Size);
        m_IsPulse = false;
    }
    
    public void OnScanned()
    {
        Renderer rend = GetComponent<Renderer>();

        //Set the main Color of the Material to green
        rend.material.color = Color.green;
        m_Size = transform.localScale.x;

        GetComponent<Collider>().enabled = false;

        StartCoroutine(StartShrink());
    }

    public IEnumerator StartShrink()
    {
        yield return new WaitForSeconds(m_Delay);
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

    public IEnumerator PulseDown()
    {
        while(m_Size > 0.15f)
        {
            transform.localScale = new Vector3(m_Size, m_Size, m_Size);
            m_Size -= 0.1f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(PulseUp());
    }

    public IEnumerator PulseUp()
    {
        while(m_Size < 0.2f)
        {
            transform.localScale = new Vector3(m_Size, m_Size, m_Size);
            m_Size += 2f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(PulseDown());
    }
}
