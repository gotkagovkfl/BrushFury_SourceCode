using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "UserData", menuName = "SO/User")]
public class UserDataSO : ScriptableObject
{
    [Space(80)]
    [Header("Setting")]
    public AudioMixer audioMixer;
    public InputActionAsset inputActionSO;  




    [Space(80)]
    [Header("Progress")]
    public string currstageNodeId;
    
    // public int traitPoint;
    // public int currChapter = 0;
    public int currStageNum = 0;
    public int currStagePlayCount;
    public int deathCount; 

}
