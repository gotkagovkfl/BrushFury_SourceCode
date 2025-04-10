using System.Collections;
using System.Collections.Generic;
using BW;
using UnityEngine;

public class InkTrailEffect : ParticleEffect
{
    protected override string id => "InkTrailEffect";



    [SerializeField] List<ParticleSystem> list_ps_child;

    protected override void Init_Custom()
    {
        // StartCoroutine( InitRoutine());
    }

    IEnumerator InitRoutine()
    {   
        var psMain = ps.main;

        psMain.loop=true;

        foreach( var ps_child in list_ps_child)
        {
            var psChildMain = ps_child.main;
            psChildMain.loop = true;   
            ps_child.Play();
        }


        yield return new WaitForSeconds(0.3f);
        psMain.loop = false;

        foreach( var ps_child in list_ps_child)
        {
            var psChildMain = ps_child.main;
            psChildMain.loop = false;
            ps_child.Stop(false, ParticleSystemStopBehavior.StopEmitting);        
        }
        // 부모에서 isAlive 할떄, 자식이 살아있는 지도 한 번에 알 수 있음. 

    }


    void Update()
    {
        myTransform.position = Player.Instance.t.position.WithStandardHeight(); 
    }
}
