using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SFX : BwPoolObject
{
    public override PoolType poolType => PoolType.Sound; 
    
    
    AudioSource audioSource;

    static int defaultPriority=100;

    protected override string id => "SFX";

    //======================================
    public override void OnCreatedInPool()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnGettingFromPool()
    {
        
    }

    //======================================
    public void Play(SoundData soundData)
    {
        // 세팅
        myTransform.position = soundData.pos;
        audioSource.clip = soundData.audioClip;
        audioSource.priority = defaultPriority + soundData.rank;
        if(soundData.type == SoundType.SFX_UI)
        {
            audioSource.dopplerLevel = 0;
            audioSource.spatialBlend = 0;
        }else
        {

        }

        //After Setting
        audioSource.Play();
        StartCoroutine( DelayedDestroy( soundData.audioClip.length+ 0.1f)); 
    }

    IEnumerator DelayedDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        SoundManager.Instance.DestroySFX(this);
    }
}
