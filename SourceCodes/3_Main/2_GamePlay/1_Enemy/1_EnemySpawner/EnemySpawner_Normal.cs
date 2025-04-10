using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner_Normal : EnemySpawner
{
    private static float[] blinkIntervals = null;  // 최초 실행 시 생성됨.

    public override IEnumerator SpawnEffect()
    {
        gameObject.SetActive(true);

        float totalDurtaion = 2f;
        float seqInterval = 0.8f;


        // **최초 1회만 배열을 생성**
        if (blinkIntervals == null)
        {
            blinkIntervals = GetIntervals(totalDurtaion -seqInterval, 5);
        }

        Sequence seq_spawn = DOTween.Sequence();
        seq_spawn.AppendInterval(seqInterval);

        // 깜빡이는 간격을 역순으로 실행
        for (int i = blinkIntervals.Length - 1; i >= 0; i--)
        {
            float blinkInterval = blinkIntervals[i];
            seq_spawn.Append(sr.DOFade(0, blinkInterval * 0.5f)) // 반 투명
                     .Append(sr.DOFade(1, blinkInterval * 0.5f)); // 다시 보이기
        }

        // **시퀀스 완료 후 비활성화 처리**
        seq_spawn.OnComplete(() => DeactivateSpawner());
        seq_spawn.Play();

        yield return seq_spawn.WaitForCompletion(); // 안전한 종료
    }

    void DeactivateSpawner()
    {
        gameObject.SetActive(false);
    }







    // **최초 1회만 실행되도록 개선된 GetIntervals()**
    public static float[] GetIntervals(float t, int n)
    {
        if (blinkIntervals != null) return blinkIntervals; // 이미 생성된 경우 재사용

        blinkIntervals = new float[n];
        float sum = 0f;

        // **EaseOut 적용 (EaseOutCubic 사용 가능)**
        for (int i = 0; i < n; i++)
        {
            float progress = (float)i / (n - 1); // 0 ~ 1
            float easeOutValue = 1 - Mathf.Pow(1 - progress, 3); // EaseOutCubic 적용
            blinkIntervals[i] = easeOutValue;
            sum += blinkIntervals[i];
        }

        // **총합이 t가 되도록 정규화**
        for (int i = 0; i < n; i++)
        {
            blinkIntervals[i] = (blinkIntervals[i] / sum) * t;
        }

        return blinkIntervals;
    }
}
