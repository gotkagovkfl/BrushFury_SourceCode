using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 장비 : 플레이어 장비칸에 장착되어 효과를 제공한다. 
/// </summary>
public class PlayerEquipments : MonoBehaviour
{
    //--------- equipments ------------
    // public SerializableDictionary<int, PlayerEquipment> equipments;
    // public List<EquipmentItemSO> _equipments;

    // public List<PlayerEquipment> equipments=new();

    /// <summary>
    /// 플레이어가 초기화 될 때, 데이터를 갖고와 세팅한다. - 복사 
    /// </summary>
    public void Init()
    {
        // List<PlayerEquipment> savedEquipments  = GameManager.Instance.playerData.savedEquipments;
        // int len = savedEquipments.Count;

        // equipments = new();
        // for(int i=0 ; i< len; i++)
        // {
        //     PlayerEquipment data = savedEquipments[i];
        //     if (data!=null )
        //     {
        //         equipments.Add(data);
        //     }
        // }
    }

    /// <summary>
    /// 스테이지가 종료되면 물건의 수명을 감소시킨다.
    /// </summary>
    // public void OnStageFinish()
    // {
    //     DecreaseLifeTime();
    //     RemoveExpiredEquipments(); 
    // }

    //===========================================
    /// <summary>
    /// 새로운 장비를 획득할 때, 장착한다. ( 장착효과 제공 )
    /// </summary>
    /// <param name="equipmentData"></param>
    // public void Equip(EquipmentItemSO equipmentData)
    // {
    //     if(equipmentData ==null)
    //     {
    //         Debug.Log("[장착실패] 데이터가 null !!");
    //         return;
    //     }
    //     PlayerEquipment newEquipment = new PlayerEquipment( equipmentData, -1); 
    //     newEquipment.OnEquip();
    //     equipments.Add(newEquipment);
    // }

    /// <summary>
    /// 장착중인 장비들의 수명을 감소시킨다. 
    /// </summary>
    // void DecreaseLifeTime()
    // {
    //     foreach(PlayerEquipment equipment in equipments)
    //     {
    //         if (equipment.permanent== false)
    //         {
    //             equipment.lifeTime--;
    //         }
    //     }
    // }


    /// <summary>
    /// 수명이 끝난 장비들 장착 해제, 리스트에서 제거 
    /// </summary>
    // void RemoveExpiredEquipments()
    // {
    //     for(int i=equipments.Count-1;i>=0;i--)
    //     {
    //         PlayerEquipment equipment = equipments[i];
    //         if(equipment.permanent ==false && equipment.lifeTime <= 0)
    //         {
    //             equipment.OnUnequip();
    //             equipments.Remove(equipment);
    //         }
    //     }
    // }











































    /// <summary>
    /// 첫번 째 빈공간 idx 를 반환.
    /// </summary>
    /// <returns></returns>
    // public int GetFirstEmptySpaceIdx()
    // {
    //     int idx = equipments.FindIndex( x=>x==null);
    //     return idx;
    // }

    // 빈공간이 있는 지, 
    // public bool HasEmptySpace()
    // {
    //     int idx = GetFirstEmptySpaceIdx();
        
    //     if( idx != -1)
    //     {
    //         return true;
    //     }

    //     // 이미 꽉차있을 땐, 한개 버려야함. 
    //     return false;
    // }

    //==============================================================

    // public void InitEquip(int idx, EquipmentItemSO equipmentData)
    // {
    //     equipments[idx] = new( equipmentData );
        // if (equipmentData !=null)
        // {
        //     equipmentData.InitEquip();
        // }
    // }





    /// <summary>
    /// 해당 슬롯 에 장착. 
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="equipmentData"></param>
    // public void Equip(int idx, EquipmentItemSO equipmentData, int lifeTime)
    // {
    //     //
    //     PlayerEquipment oldEquipment = equipments[idx];
    //     PlayerEquipment newEquipment = new(equipmentData, lifeTime);
    //     if (oldEquipment!=null)
    //     {
    //         oldEquipment.UnEquip();
    //     }
        
    //     //
    //     if (equipmentData !=null)
    //     {
    //         newEquipment.Equip();
    //     }
        
    //     //
    //     equipments[idx] = newEquipment;
    // }

    

    /// <summary>
    /// 장비 장착 시도 - 아이템을 획득할 때,
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns></returns> 장비 장착에 성공했는지. - true만 그냥 끼는데, false 면 선택해야함. 
    // public bool TryEquip(EquipmentItemSO equipment)
    // {
    //     int idx = GetFirstEmptySpaceIdx();
        
    //     if( idx != -1)
    //     {
    //         Equip( idx, equipment);
    //         return true;
    //     }

    //     // 이미 꽉차있을 땐, 한개 버려야함. 
    //     return false;
    // }



}
