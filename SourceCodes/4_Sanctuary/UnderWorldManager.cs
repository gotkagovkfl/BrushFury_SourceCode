using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 게임플레이매니저와 동격
/// </summary>
public class UnderWorldManager : Singleton<UnderWorldManager>
{
    public static bool isGamePlaying;
    [SerializeField] EntrancePortal entrancePortal;


    // Start is called before the first frame update
    void Start()
    {
        
        OnEnterUnderWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //==========================
    void OnEnterUnderWorld()
    {
        SoundManager.Instance.Invoke(transform, SoundEventType.BGM_Sanctuary);
        
        //
        isGamePlaying = false;
        UnderWorldPlayer.Instance.InitPlayer();
        GameManager.Instance.PauseGamePlay(false);
        StartCoroutine(EnterSequence());
    }

    IEnumerator EnterSequence()
    {
        DirectingManager.Instance.ZoomIn(UnderWorldPlayer.Instance.t_player);
        
        Sequence generatePortalSeq = entrancePortal.GetSeq_GeneratePortal(0.5f);
        Sequence playerEnterPortalSeq = UnderWorldPlayer.Instance.GetSequence_EnterPortal(false, 0.7f);

        yield return new WaitForSeconds(1f);    //대기시간

        generatePortalSeq.Play();
        SoundManager.Instance.Invoke(transform, SoundEventType.Sanc_PortalSpawn);
        yield return new WaitUntil( ()=> generatePortalSeq.IsActive()==false );

        yield return new WaitForSeconds(1f);    //대기시간

        playerEnterPortalSeq.Play();
        
        yield return new WaitUntil( ()=> playerEnterPortalSeq.IsActive()==false );

        entrancePortal.PlaySeq_DestroyPortal(2f);

        yield return new WaitForSeconds(1f);
        DirectingManager.Instance.ZoomOut();
        SoundManager.Instance.Invoke(transform, SoundEventType.Sanc_ZoomOut);
        //
        yield return new WaitForSeconds(1f);
        isGamePlaying = true;
    }


    public void LeaveUnderWorld()
    {
        isGamePlaying = false;
        
        SoundManager.Instance.Invoke(transform, SoundEventType.Portal);
        SceneLoadManager.Instance.Load_MainScene();
    }
}
