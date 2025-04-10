using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DiaGage : MonoBehaviour
{
    [SerializeField] protected Color fillColor;

    [SerializeField] protected Image img_fill;
    [SerializeField] protected TextMeshProUGUI text;
    
    [SerializeField] protected float m_minValue;
    [SerializeField] protected float m_value;
    [SerializeField] protected float m_maxValue = 1;

    IEnumerator Start()
    {
        yield return new WaitUntil(()=>Player.Instance.initialized);
        OnValueChanged();
    }
    
    void OnValidate()
    {
        if( img_fill !=null)
        {
            img_fill.color = fillColor;
        }
        OnValueChanged();
    }


    public void UpdateMinValue(float value)
    {
        m_minValue = value;
        OnValueChanged();
    }


    public void UpdateMaxValue(float value)
    {
        m_maxValue = value;
        OnValueChanged();
    }
    public void UpdateCurrValue(float value)
    {
        m_value = value;
        OnValueChanged();
    }

    public void Refresh()
    {
         OnValueChanged();
    }


    void OnValueChanged()
    {
        if ( img_fill == null)
        {
            return;
        }
        
        m_value = Mathf.Clamp(m_value, m_minValue, m_maxValue);
        img_fill.fillAmount = m_value/ (m_maxValue-m_minValue);

        OnValueChanged_Custom();
    }

    protected abstract void OnValueChanged_Custom();
    
}
