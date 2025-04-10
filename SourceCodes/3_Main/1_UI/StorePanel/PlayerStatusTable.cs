using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusTable : MonoBehaviour
{
    [SerializeField] GameObject prefab_playerStatusUI;


    [SerializeField] TextMeshProUGUI text_maxHp;
    [SerializeField] TextMeshProUGUI text_maxInk;
    [SerializeField] TextMeshProUGUI text_movementSpeed;
    [SerializeField] TextMeshProUGUI text_pDmg;
    [SerializeField] TextMeshProUGUI text_mDmg;

    
    public void Init()
    {
        UpdateTable();
    }

    
    /// <summary>
    /// 나중에 자동으로 세팅될 수 있도록 해야함. 
    /// </summary>
    public void UpdateTable()
    {
        // text_maxHp.SetText($"{Player.Instance.status.maxHp}"  );
        // text_maxInk.SetText($"{Player.Instance.status.maxInk}"  );
        // text_movementSpeed.SetText($"{Player.Instance.status.movementSpeedMultiplier.value}"  );
        // text_pDmg.SetText($"{Player.Instance.status.pDmg.value}"  );
        // text_mDmg.SetText($"{Player.Instance.status.mDmg.value}"  );
    }
}
