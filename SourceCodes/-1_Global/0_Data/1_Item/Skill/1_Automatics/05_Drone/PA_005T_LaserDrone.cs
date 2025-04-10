using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
기존 공격 시퀀스의 공격 발사 후에 레이저 공격이 추가됨.

    p1 부터 p2 까지 드론 중심에서 레이저를 그어 피해를 입히고,
    일정 시간후 경로를 따라 폭발을 explosionCount만큼 일으킨다. 

*/
[CreateAssetMenu(fileName = "PA_005T", menuName = "SO/SkillItem/005T_LaserDrone", order = int.MaxValue)]
public class PA_005T_LaserDrone : PA_005_Drone 
{
    public override string id_base => $"PA_005T_LaserDrone";
    public override string description => "레이저 드론";



    public override string dataName => "레이저 드론";
    
    [Header("Laser")]
    public int explosionCount_laser = 4;
    public float explosionDelay = 0.5f;
    public float unitDistance = 1.5f;          // 폭발이 일어나는 단위 길이
    public float duration_laser = 0.5f;   // 처음부터 끝까지 레이저를 긋는 시간. 
    public float explosionRadius = 3f;
    public PlayerProjectile pp_laser;
    public PlayerProjectile pp_laserExplosion;





}
