using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

using System.Linq;
using Unity.Collections;


/// <summary>
/// 레벨업하면 선택지에 등장할 수 있는 아이템 목록. 
/// </summary>
[CreateAssetMenu(fileName = "SelectableOptionDictionary", menuName = "SO/Dictionary/SelectableOptions", order = int.MaxValue)]
public class SelectableOptionDictionary :ScriptableObject
{
    [SerializeField] List<EquipmentItemSO> passives = new();
    [SerializeField] List<SkillItemSO>    automatics = new(); 
    // [SerializeField] List<ConsumableItemSO> consumables = new();



    public SerializableDictionary<string, ItemDataSO> table = new(); 
    public ConsumableItemSO defaultOption;          

    //======================================================
    void OnValidate()
    {
        //
        passives = passives.OrderBy(x=>x.id).ToList();
        automatics = automatics.OrderBy(x=>x.id).ToList();
        table?.Clear();
        List<ItemDataSO> allItems = passives.Cast<ItemDataSO>()
                                    .Concat(automatics.Cast<ItemDataSO>())
                                    .ToList();
        //
        foreach( var item in allItems)
        {
            if( table.ContainsKey(item.id) == false)
            {
                table[item.id] = item;
            }
        }
    }

    

    
}
