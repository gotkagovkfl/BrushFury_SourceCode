using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// [RequireComponent(typeof(SphereCollider))]
public abstract class InteractiveObject : MonoBehaviour
{

    // [SerializeField] protected bool isPlayerInRange;
    public bool isActivated =>locked ==false && gameObject.activeSelf;
    
    public bool locked ;

    protected bool beingInspected; // 플레이어가 현재 바라보고 있는지;
    [SerializeField] protected TextMeshPro text_inspecting;
    protected abstract string inspectingText {get;}

    // public abstract bool hasSecondaryInteraction {get;} 

    // protected abstract float TargetInteractingTime {get;}
    // protected float currInteractingTime = 0;
    // protected bool isInteracting;   // 활성화중인지
    // protected bool interactImmediately => TargetInteractingTime <=0;

    

    void Awake()
    {
        GetComponent<SpriteEntity>()?.Init();


        text_inspecting.transform.localPosition = new UnityEngine.Vector3(0,-1,-1);
        text_inspecting.SetText($"<sprite=12> {inspectingText}");
        text_inspecting.gameObject.SetActive(false);
    }
    
    // void Update()
    // {
    //     if( interactImmediately || isInteracting )
    //     {
    //         return;
    //     }
        
    //     if( currInteractingTime > 0)
    //     {
    //         currInteractingTime -= Time.deltaTime;
    //     }
    //     else
    //     {
        
    //     }
    // }

    //==============================================================================
    
    protected void Activate()
    {
        locked = false;
        GetComponent<Collider>().enabled = true;
    }

    protected void Deactivate()
    {
        locked = true;
        GetComponent<Collider>().enabled = false;

        //
        OnInspect(false);
    }

    public void OnInspect(bool isOn)
    {
        if (beingInspected == isOn)
        {
            return;
        }
        beingInspected = isOn;
        text_inspecting.gameObject.SetActive(isOn);
        OnInspect_Custom(isOn);   
    }


    protected abstract void OnInspect_Custom(bool isOn);


    public void OnInteract()
    {
        // if (isInteracting )
        // {
        //     if( interactImmediately )
        //     {
        //         return;
        //     }

        // }
        // isInteracting = true;
        // if(  interactImmediately || currInteractingTime >= TargetInteractingTime)
        // {
        OnInteract_Custom();
        //     isInteracting = false;
        // }
    }

    // public void OnSecondaryInteract()
    // {
    //     if (hasSecondaryInteraction == false)
    //     {
    //         return;
    //     }
    //     OnSecondaryInteract_Custom();
    // }

    
    protected abstract void OnInteract_Custom();

    // protected abstract void OnSecondaryInteract_Custom();
}
