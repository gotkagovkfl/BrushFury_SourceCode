using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerDataText;    
    
    // Start is called before the first frame update
    void Start()
    {
        // Init( GameManager.Instance.userData );
    }


    /// <summary>
    /// 로비 초기화  - 일단은 플레이어 데이터가 어쩐지 보여줄 수 있도록 세팅 .
    /// </summary>
    /// <param name="playerData"></param>
    // public void Init(UserDataSO playerData)
    // {
    //     string str = $"PlayerData\n\n-Chapter.{playerData.currChapter}\n-Stage.{playerData.currStageNum}\n-TraitPoint.{playerData.traitPoint}";
        
    //     playerDataText.SetText(str);


    //     SoundManager.Instance.Invoke(transform, SoundEventType.BGM_Lobby);
    // }
}
