using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public Slider m_CheckpointSlider;
    //public Slider m_RadiusSlider;
    public Text m_CPCount;
    //public Text m_RCount;


    public void Start()
    {
        m_CheckpointSlider.value = CheckPointManager.instance.m_NumberOfCheckPoints;
    }

    public void OnCheckPointValChanged()
    {
        m_CPCount.text = m_CheckpointSlider.value.ToString();
        CheckPointManager.instance.m_NumberOfCheckPoints = (uint)m_CheckpointSlider.value;
    }

    //public void OnRadiusValueChanged()
    //{
    //    m_RCount.text = m_RadiusSlider.value.ToString();
    //    CheckPointManager.instance.m_NumberOfCheckPoints = (uint)m_RadiusSlider.value;
    //}
}
