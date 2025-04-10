using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] EnemyHpSlider hpSlider;
    
    
    public void Init(Enemy enemy)
    {
        hpSlider?.gameObject.SetActive(true);
        hpSlider?.UpdateHp(enemy);
    }

    public void OnHit(Enemy enemy)
    {
        hpSlider.UpdateHp(enemy);
    }


    public void OnEnemyDeath(Enemy enemy)
    {
        hpSlider?.gameObject.SetActive(false);
    }
}
