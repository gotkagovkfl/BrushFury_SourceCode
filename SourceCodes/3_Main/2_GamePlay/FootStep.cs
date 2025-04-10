using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public void OnGround()
    {
        if(Player.Instance.status.isStunned== false)
        {
            SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Player_Move);
        }
    }
}
