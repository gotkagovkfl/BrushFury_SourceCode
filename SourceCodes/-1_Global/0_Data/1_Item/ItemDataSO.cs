using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public enum ItemTier
{
    // Normal,
    Rare,
    Epic,
    Unique,
    // Legendary
}

public enum ItemType
{
    Consumable,
    Equipment,
    Skill
}

public enum CantGetReason
{
    None,
    NoSpace
}


[Serializable]
public abstract class ItemDataSO : GameData 
{
    // public Sprite sprite; 
    public override string id => $"{type}_{id_detail}"; 
    protected abstract string id_detail {get;}

    public ItemType type;
    public ItemTier tier;
    public abstract string description {get;}
    public abstract string typeKor {get;}
    public List<ItemDataSO> displayCondition;     //선행조건 : 상점 등장 조건. 해당 아이템을 보유해야지만 상점에 나타난다. 
    public bool hasDisplayCondition => displayCondition !=null && displayCondition.Count>0;

    public bool CanDisplay(PlayerSkills playerSkills)
    {
        bool ret = playerSkills.CanEquip(this) && AdditionalDisplayCondition();
        return ret ;
    }
    protected bool AdditionalDisplayCondition()
    {
        return true;
    }

    public bool TryGet()
    {
        if (CanGet(out CantGetReason reason))
        {
            Get();
            return true;
        }
        else
        {
            OnCantGet(reason);
            return false;
        }
    }
    
    protected abstract bool CanGet(out CantGetReason reason);
    
    protected abstract void Get();

    protected abstract void OnCantGet(CantGetReason reason);


    //====================
    void OnValidate()
    {
        // Debug.Log(displayCondition.Count);
        displayCondition = displayCondition.Where(x=>x!=null).ToList();
    }
    

}
