using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("State")]
    [SerializeField] DiaGage hpGage;
    [SerializeField] DiaGage inkGage;

    [SerializeField] DiaGage expGage;


    [Space(40)]
    [Header("Skills")]
    [SerializeField] PlayerAbilityHud playerAbilityHud;
    // [SerializeField] PlayerStateUI stateUI;


    public void Init(Player player)
    {
        playerAbilityHud.Init(player.skills);

        UpdateMaxHp();
        UpdateCurrHp();

        // OnInkSegmentsChanged(player);

        UpdateMaxInk();
        UpdateCurrInk();


        UpdateMaxExp();
        UpdateCurrExp();


        GameEventManager.Instance.onChangePlayerStatus_maxHp.AddListener(UpdateMaxHp);
    }


    //=====================================================================
    public void UpdateMaxHp()
    {
        float value = Player.Instance.status.maxHp.value;
        hpGage.UpdateMaxValue(value);
    }

    public void UpdateMaxInk()
    {
        float value = Player.Instance.status.maxInk.value;
        inkGage.UpdateMaxValue(value);
    }
    public void UpdateMaxExp()
    {
        float value = Player.Instance.status.maxExp.value;
        expGage.UpdateMaxValue(value);
    }
    
    
    
    
    public void UpdateCurrHp()
    {
        float value = Player.Instance.status.currHp;
        hpGage.UpdateCurrValue(value);
    }


    public void UpdateCurrInk()
    {
        float value = Player.Instance.status.currInk;
        inkGage.UpdateCurrValue(value);
    }

    public void UpdateCurrExp()
    {
        float value = Player.Instance.status.currExp.value;
        expGage.UpdateCurrValue(value);
    }


    public void UpdateLevel()
    {
        // int value = (int)Player.Instance.status.level.value;
        expGage.Refresh();
    }


}
