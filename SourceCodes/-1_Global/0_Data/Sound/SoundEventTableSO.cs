using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.EditorTools;



public enum SoundType
{
    SFX_GO,         // 게임 오브젝트의 sfx - 위치 지정 필요. - go : game object
    SFX_UI,         // ui의 sfx - 위치 지정 필요 없음. 
    BGM,            // bgm
}

//
public enum SoundEventType
{
    // Enemy
    Enemy_Die,           // - 소리 여러개임. 타입별로 지정     - 완료  - data의 sfx 파일 사용
    Enemy_Hit,           // -    attack_monster_hit  (3개중 랜덤)

    // Player
    Player_Hit,         // player_hit
    Player_Die,         //  player_die
    Player_Move,         // 걷는 소리 3개를 조합 [발자국]
    Player_Dash,
    Player_BasicAttack,              //  소리 2개 조합. [공격]
    Player_RyoikiTenkaiOn,           //  소리 2개 조합.  ( Q_Start+ Q_Voice)
    Player_RyoikiTenkaiOff,          // Q_end

    // drop item
    Item_coin,              // coint_pickup,    - 완료. - data의 sfx파일 사용
    Item_bak,           // item_pickup          - 완료  - data의 sfx 파일 사용


    //
    UI_GamePlayStart,               // game_start
    UI_ButtonClick,                 // main_click
    
    // Store
    Store_Open,       // shop_click
    Store_Close,     // shop_click_3
    Store_Buy,      // shop_click2
    Store_Lack,     // money_broke_click
    
    //BGM
    BGM_Battle,
    BGM_Sanctuary,
    BGM_Lobby,

    // Sanctuary
    Portal,
    Sanc_PortalSpawn,
    Sanc_PlayerSpawn,
    Sanc_ZoomOut

     
}

[CreateAssetMenu(fileName = "SoundEventTable", menuName = "SO/SoundEventTable", order = int.MaxValue)]
public class SoundEventTableSO : ScriptableObject
{
    // public List<SoundEvent> list = new();
    [Tooltip("각 소리가 발생하는 상황과 재생할 소리 파일")]
    public SerializableDictionary<SoundEventType, SoundSO> table = new();
    
    [Tooltip("동시에 재생할 수 있는 sfx 수\nBGM, rank가 0인 소리는   제외\n 아직 미완성 ")]
    public int maxSFXCount = 16;

    void OnValidate()
    {

    }
}

