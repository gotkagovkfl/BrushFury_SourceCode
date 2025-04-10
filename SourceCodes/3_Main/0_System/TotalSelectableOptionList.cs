using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class TotalSelectableOptionList 
{
    [SerializeField] List<ItemDataSO> list = new();
    [SerializeField] ItemDataSO defaultItem;  // 더이상 가능한 선택지가 없을때, 대신 진열할 아이템

    System.Random random = new();

    public TotalSelectableOptionList(List<BasicAttackSO> basicAttackData, List<SkillItemSO> uniqueAbilityData, SelectableOptionDictionary selectableOptionDictionary ) 
    {
        list= new();

        // 기본공격과, 고유능력들도 선택지에 추가 ( 0레벨은 기본 장착이니 넣지 않는다. ) 두 능력의 최대레벨이 달라지면 바꿔야함. 
        for(int i=1;i<basicAttackData.Count;i++)
        {
            list.Add( basicAttackData[i]);
            list.Add( uniqueAbilityData[i]);
        }


        //
        foreach(var kv in selectableOptionDictionary.table)
        {
            ItemDataSO item = kv.Value;

            list.Add(item);
        }

        defaultItem = selectableOptionDictionary.defaultOption;
    }

    //==============================================================================================
    public List<ItemDataSO> GetRandomOptions(int count)
    {
        // 선행조건을 만족하는 
        PlayerSkills playerSkills = Player.Instance.skills;
        List<ItemDataSO> ret = list.Where(x=>x.CanDisplay(playerSkills))
                                    .OrderBy(_ => random.Next())
                                    .Take(count).ToList();
        
        
        return ret;
    }

    public ItemDataSO GetDefaultItem()
    {
        return defaultItem;
    }
}
