using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageGoalText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Init()
    {
        gameObject.SetActive(false);
        
    }

    public void Actiavte(string str)
    {
        gameObject.SetActive(true);
        text.SetText(str);
    }
}
