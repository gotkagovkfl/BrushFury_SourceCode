using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    SkillType skillType;

    PlayerSkill playerSkill;

    [SerializeField] PlayerItemUI playerItemUI;
    [SerializeField] TextMeshProUGUI text_keyCode;

    void Awake()
    {
        // GameEventManager.Instance.onChangeSkill.AddListener(OnChangeSkill);
    }


    public void Init(SkillType skillType, PlayerSkill playerSkill)
    {
        this.skillType = skillType;
        this.playerSkill = playerSkill;

        text_keyCode.SetText( playerSkill?.data?.typeKor);

        playerItemUI.Init(0,playerSkill.data);
    }


    /// <summary>
    /// 스킬 바뀔 때 호출됨. 바뀐 스킬칸의 경우에만 반응하도록함. 
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="playerSkill"></param>
    public void OnChangeSkill(SkillType skillType, PlayerSkill playerSkill)
    {
        if (this.skillType != ((SkillItemSO)playerSkill.data).skillType)
        {
            return;
        }

        this.playerSkill = playerSkill;

        text_keyCode.SetText(playerSkill.data.typeKor);

        playerItemUI.Init(0,playerSkill.data);
    }

}
