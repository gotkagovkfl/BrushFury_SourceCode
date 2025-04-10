using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHud : MonoBehaviour
{
    [SerializeField] PlayerAbilityInfo playerAbilityUI_basicAttack;
    [SerializeField] PlayerAbilityInfo playerAbilityUI_unique;
    [SerializeField] List<PlayerAbilityInfo> playerAbilityUIs_auto;



    public void Init(PlayerSkills playerSkills)
    {
        playerAbilityUI_basicAttack.Init(playerSkills.basicAttack);
        playerAbilityUI_unique.Init(playerSkills.uniqueSkill);
        
        for(int i=0;i<playerAbilityUIs_auto.Count;i++)
        {
            PlayerSkill playerSkill = playerSkills.automaticSkills[i];
            playerAbilityUIs_auto[i].Init( playerSkill);
        }
    }
}
