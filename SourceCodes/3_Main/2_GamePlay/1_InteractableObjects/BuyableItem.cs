// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using TMPro;
// using BW;

// public class BuyableItem  : InteractiveObject
// {
//     [SerializeField] SpriteRenderer sr_item;
//     // [SerializeField] TextMeshPro text;
//     public BuyableItemSO data;
//     [SerializeField] TextMeshPro text_cost;

//     PlayerItem displayingItem;

//     protected override string inspectingText => "구매";
//     // protected override float TargetInteractingTime => 0;

//     //========================================================================
//     protected override void OnInspect_Custom(bool isOn)
//     {

//     }

//     protected override void OnInteract_Custom()
//     {
//         Vector3 textPos=  Player.Instance.t.position.WithStandardHeight();
//         int gold = Player.Instance.status.gold;
//         if(CanBuy(gold))
//         {
//             Player.Instance.status.UseGold(data.cost);
//             Player.Instance.AcquireItem(data.baseData);

//             PoolManager.Instance.GetText( textPos, "구매 성공");
//             Destroy(gameObject);
//         }
//         else
//         {
//             PoolManager.Instance.GetText( textPos, "금화 부족!");
//         }
        
//     }

//     //========================================================================
//     public void Init(BuyableItemSO data)
//     {
//         displayingItem = GeneratePlayerItem(data); 
//         if( displayingItem ==null)
//         {
//             Destroy(gameObject);
//             return;
//         }
        
//         this.data = data;
//         sr_item.sprite =    displayingItem.data.sprite;
//         text_cost.SetText($"<size=8><sprite=12></size>{data.cost}");
//         SetGoldText(true);
//         GameEventManager.Instance.onChangePlayerGold.AddListener((int a,int b)=>SetGoldText());
//     }
    
//     void SetGoldText(bool onInit=false)
//     {
//         // int gold = onInit?GameManager.Instance.playerData.savedStatus.gold:Player.Instance.status.gold;
//         // if( CanBuy(gold))
//         // {
//         //     text_cost.color = Color.white;
//         //     text_inspecting.color = Color.white;
//         // }
//         // else
//         // {
//         //     text_cost.color = new(0.5f,0,0);
//         //     text_inspecting.color = new(0.5f,0,0);
//         // }
//     }




//     PlayerItem GeneratePlayerItem(BuyableItemSO data)
//     {
//         if(data.baseData.type == ItemType.Equipment)
//         {
//             int randomLifeTime = Math.GetRandom(1,4);
//             return new PlayerEquipment( (EquipmentItemSO)data.baseData, randomLifeTime);
//         }

//         return null;
//     }



//     bool CanBuy(int gold)
//     {
//         if( gold >= data.cost)
//         {
//             return true;
//         }
//         return false;
//     }
// }

