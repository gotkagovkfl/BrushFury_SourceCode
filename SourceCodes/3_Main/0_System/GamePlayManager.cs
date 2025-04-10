using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading;
using System.ComponentModel;
using UnityEngine.Rendering;
using JetBrains.Annotations;

using Cinemachine;
using System;

/// <summary>
/// 메인 씬의 게임 플레이 로직 및 ui를 관리
/// </summary>
public class GamePlayManager : Singleton<GamePlayManager>
{
    // public static bool isPaused;
    public static bool isGamePlaying = false;

    public float gameStartTime;    // 해당 스테이지 시작 시간.
    public float gamePlayTime;
    public List<float> alarms = new();

    //
    public CinemachineVirtualCamera mainVCam;

    [Space(50)]
    [Header("UI Panels")]
    [SerializeField] List<GamePlayPanel> openedPanels = new();            //현재 켜진 패널 - 켜진 패널이 있으면 pause


    [SerializeField] PlayerInfoPanel playerInfoPanel; // 플레이어 정보 패널 - esc 눌렀을 때,
    [SerializeField] GamePlaySettingPanel settingPanel;
    [SerializeField] LevelUpPanel levelUpPanel;
    //
    [SerializeField] GameOverPanel gameOverPanel;   //게임오버 패널
    [SerializeField] StageClearUI stageClearUI;   //게임오버 패널
    
    //
    [SerializeField] SelectableOptionDictionary defaultSelectableOptions;
    TotalSelectableOptionList totalSelectableOptionList;



    // [Header("etc")]

    // [SerializeField] EnemyHpSlider enemyHpSlider;


    [Space(50)]
    [Header("Stage")]
    public StagePrefabDictionarySO stagePrefabDic;


    [Space(50)]
    [Header("Events")]
    public Action event_stageClear;

    //===================================================================================================================================================
    IEnumerator Start()
    {
        PoolManager.Instance.Init();


        
        
        GameManager.Instance.PauseGamePlay(false);
        isGamePlaying = false;

        // 데이터와 오브젝트 세팅
        CharacterDataSO characterInitData = GameManager.Instance.selectedCharacterData;
        UserDataSO userData = GameManager.Instance.userData;

        // 여기에 선택지 정보 초기화 필요.       
        totalSelectableOptionList  = new( characterInitData.basicAttackData, characterInitData.uniqueAbilityData, defaultSelectableOptions);

        
        // 현재 스테이지에 맞는 오브젝트 생성 후 초기화. 
        InitStage(userData);
        Player.Instance.InitPlayer(userData,characterInitData);

        
        yield return Stage.Instance.StageStartSequence();    // 게임 시작 시퀀스 재생 ( 스테이지 시작 연출)   

        StartGamePlay();        // 게임 플레이 시작 처리 
        // StartCoroutine( Stage.Instance.StageRoutine() ); // 스테이지 시작. 
        Stage.Instance.StartStage();
    }

    void InitStage(UserDataSO userData)
    {
        int currStageNum = userData.currStageNum;

        GameObject stagePrefab = stagePrefabDic.GetStagePrefab(currStageNum);
        Stage stage = Instantiate(stagePrefab, Vector3.zero, Quaternion.identity).GetComponent<Stage>();
        stage.Init();
    }



    void StartGamePlay()
    {
        Player.Instance.OnStartGamePlay();

        gameStartTime = Time.time;
        isGamePlaying = true;

        // bgm.Play();
    }

    void Update()
    {
        // esc 누르면 상태창나옴.
        if (PlayerInputManager.Instance.flowControl && openedPanels.Count==0)
        {
            OpenPlayerInfoPanel();
        }

        if( Input.GetKeyDown(KeyCode.Alpha6))
        {
            // OpenPlayerStatusUpgradePanel();
            OnLevelUp();
        }
        
        
        
        
        if (isGamePlaying == false)
        {
            return;
        }

        gamePlayTime += Time.deltaTime;
    }


    //========================================
    public void OpenPanel(GamePlayPanel panel)
    {
        if ( openedPanels.Contains(panel))
        {
            return;
        }
        openedPanels.Add(panel);    
        CheckPause();
    }

    public void ClosePanel(GamePlayPanel panel)
    {
        openedPanels.Remove(panel);
        CheckPause();
    }

    void CheckPause()
    {
        if (openedPanels.Count>0)
        {
            if(GameManager.isPaused ==false)
            {
                GameManager.Instance.PauseGamePlay(true);
            }
        }
        else
        {
            GameManager.Instance.PauseGamePlay(false);
        }
    }




    public void OpenSettingPanel()
    {
        settingPanel.Open();
        // GameManager.Instance.PauseGamePlay(true);   
    }

    public void CloseSettingPanel()
    {
        settingPanel.Close();
    }


    #region ==== Stage ====


    //--------------------------------------------------
    [Header("Stage")]

    public int killCount_currWave;

    //--------------------------------------------------
    public void OnStageStart()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public void OnStageClear()
    {
        // GameEventManager.Instance.onStageFinish.Invoke();
        // StartCoroutine(StageClearSequence());

        event_stageClear?.Invoke();

        Player.Instance.OnStageFinish();
    }

    IEnumerator StageClearSequence()
    {
        Sequence seq_stageClear = stageClearUI.GetSeq_StageClear();
        seq_stageClear.Play();
        yield return new WaitUntil(() => seq_stageClear.IsActive() == false);
    }


    // public void UpdateEnemyHpSlider(Enemy enemy)
    // {
    //     enemyHpSlider.UpdateHp(enemy);
    // }

    // public void DeactivateEnemyHpSlider()
    // {
    //     enemyHpSlider.gameObject.SetActive(false);
    // }

    #endregion
    //==========================================================



    //======================================================

    #region Upgrade

    public void OnLevelUp()
    {
        //
        levelUpPanel.Open();

        int selectableOptionCount = (int)Player.Instance.status.selectableOptionCount.value;
        List<ItemDataSO> optionDataList = totalSelectableOptionList.GetRandomOptions(selectableOptionCount);
        ItemDataSO defaultItem = totalSelectableOptionList.GetDefaultItem();
        levelUpPanel.FillOptionData(optionDataList, defaultItem);
    }










    public void OpenPlayerInfoPanel()
    {
        playerInfoPanel.Open();
    }

    public void ClosePlayerInfoPanel()
    {
        playerInfoPanel.Close();
    }

    #endregion


    //========================================
    public void GameOver()
    {
        // 해당 함수가 여러번 실행되지 않게. 
        if (isGamePlaying == false)
        {
            return;
        }

        Debug.Log("----게임오버 ------");
        isGamePlaying = false;
        GameEventManager.Instance.onGameOver.Invoke();
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        DirectingManager.Instance.ZoomIn(Player.Instance.t);
        GameManager.Instance.PauseGamePlay(true);
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.PauseGamePlay(false, 2f);
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(DirectingManager.Instance.FadeSequene());
        gameOverPanel.Open();
        DirectingManager.Instance.ZoomOut();
    }


    //====================================================


}
