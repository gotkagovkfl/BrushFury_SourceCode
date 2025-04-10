using System.Collections;
using TMPro;
using UnityEngine;



public class StageTimer : MonoBehaviour
{    
    BattleStage currStage;
    
    [SerializeField] TextMeshProUGUI text_time;

    Coroutine runningTimer;

    //=============================================================    




    public void Init(BattleStage currStage)
    {
        this.currStage = currStage;
        gameObject.SetActive(false);
    }


    public void Run()
    {
        gameObject.SetActive(true);

        if(runningTimer != null)
        {
            StopCoroutine(runningTimer);
        }

        runningTimer = StartCoroutine(RunTimer_Desc());
    }


    public void Stop()
    {
        if(runningTimer != null)
        {
            StopCoroutine(runningTimer);
        }
        gameObject.SetActive(false);
    }

    //==========================================================================


    /// <summary>
    /// 남은 시간 타이머 
    /// </summary>
    /// <returns></returns>
    IEnumerator RunTimer_Desc()
    {
        var waitForSeconds = new WaitForSeconds(0.5f);
        while(true)
        {
            float curr = currStage.stagePlayTime;
            SetTimer(curr);
            yield return waitForSeconds;
        }
    }



    void SetTimer(float time)
    {
        int mins = (int)time/60;
        int secs = (int)time%60;

        text_time.SetText($"{mins:00}:{secs:00}");
    }
}
