using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField] DiaGage hpGage;
    [SerializeField] DiaGage inkGage;

    [SerializeField] DiaGage expGage;
    // [SerializeField] TextMeshProUGUI text_level;




    //==========================================
    public void Init(Player player)
    {
        UpdateMaxHp(player.status.maxHp.value);
        UpdateCurrHp(player.status.currHp);

        // OnInkSegmentsChanged(player);

        UpdateMaxInk(player.status.maxInk.value);
        UpdateCurrInk(player.status.currInk);


        UpdateMaxExp(player.status.maxExp.value);
        UpdateCurrExp(player.status.currExp.value);
        UpdateLevel( (int) player.status.level.value);

        // 이벤트 리스너 추가
        GameEventManager.Instance.onChangePlayerStatus_maxHp.AddListener(() => UpdateMaxHp(player.status.maxHp.value));
        // GameEventManager.Instance.onChangePlayerStatus_inkSegments.AddListener(() => OnInkSegmentsChanged(player));
    }



    #region ====== HP =======

    /// <summary>
    /// hp bar 의 최댓값을 플레이어 능력치 값에 맞춘다. (주로 체력 증가 이벤트 발생시 호출됨 )
    /// </summary>
    public void UpdateMaxHp(float value)
    {
        hpGage.UpdateMaxValue(value);
    }

    /// <summary>
    /// hp bar의 현재 값을 플레이어 능력치 값에 맞춘다. (주로 회복, 피해 발생시 호출됨)
    /// </summary>
    public void UpdateCurrHp(float value)
    {
        hpGage.UpdateCurrValue(value);
    }

    #endregion

    #region ====== Ink =======

    /// <summary>
    /// ink bar 의 최댓값을 플레이어 능력치 값에 맞춘다.
    /// </summary>
    public void UpdateMaxInk(float value)
    {
        inkGage.UpdateMaxValue(value);
    }

    /// <summary>
    /// ink bar의 현재 값을 맞춘다. (붓칠 시 호출됨)
    /// </summary>
    public void UpdateCurrInk(float value)
    {
        inkGage.UpdateCurrValue(value);


    }
    #endregion




    #region ====== Exp =======

    /// <summary>
    /// ink bar 의 최댓값을 플레이어 능력치 값에 맞춘다.
    /// </summary>
    public void UpdateMaxExp(float value)
    {
        expGage.UpdateMaxValue(value);
    }

    /// <summary>
    /// ink bar의 현재 값을 맞춘다. (붓칠 시 호출됨)
    /// </summary>
    public void UpdateCurrExp(float value)
    {
        expGage.UpdateCurrValue(value);
        
        // if(Player.Instance.status.canLevelUp)
        // {
        //     img_expFill.color = Color.magenta;    
        // }
        // else
        // {
        //     img_expFill.color = Color.cyan;    
        // }
    }



    public void UpdateLevel(int level)
    {
        // text_level.SetText($"등급{level}");
    }
    #endregion
}
