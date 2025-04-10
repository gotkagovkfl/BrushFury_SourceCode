using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerEquipment : PlayerItem<EquipmentItemSO>
{
    public int lifeTime;        // 수명 : 0이 되면 파괴. - 스테이지 진행할 때마다 -1; 처음부터 -1로 지정되면 자동 파괴되지 않음. 

    public int currStackCount;

    public Action<PlayerEquipment> onInit;  // 스킬 교체될 때, 
    //=================================================================


    public void Init(EquipmentItemSO passiveData )
    { 
        if ( passiveData == null)
        {
            Debug.Log($"패시브 장착 실패! 데이터가 null");
            currStackCount=0;
            return;
        }
        
        data?.OnUnEquip(currStackCount); 
        
        if (data == passiveData)
        {
            currStackCount ++;
        }
        else
        {
            currStackCount=1;
            data = passiveData;
        }
                
        data?.OnEquip(currStackCount);
        onInit?.Invoke(this);
    }


    
    public void UnEquip()
    {
        if( data != null)
        {
            data.OnUnEquip(currStackCount);
            data=null;
            currStackCount=0;
        }
    }

}
