using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PlayerExtraItemCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_count;
    
    
    public void Init(int count)
    {
        if (count >0)
        {
            text_count.gameObject.SetActive(true);
            text_count.SetText($"+{count}");
        }
        else
        {
            text_count.gameObject.SetActive(false);
        }
        
        
    }
}
