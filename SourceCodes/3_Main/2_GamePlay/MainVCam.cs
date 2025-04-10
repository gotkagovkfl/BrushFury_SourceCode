using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

using Cinemachine;

public class MainVCam : Singleton<MainVCam>
{
    CinemachineVirtualCamera _vCam;

    CinemachineVirtualCamera vCam 
    {
        get 
        {   
            if(_vCam==null)
            {
                _vCam = GetComponent<CinemachineVirtualCamera>();
            }
            return _vCam;
        }
    }
                            
    /// <summary>
    /// 카메라 영역 제한.
    /// </summary>
    /// <param name="confiner"></param>
    /// <param name="centerPos"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void InitCameraConfiner(BoxCollider confiner,Vector3 centerPos, float width, float height)
    {
        float bonus = 5f;
        
        float orthoSize = Camera.main.orthographicSize;
        Vector3 offset = vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        float zDelta = offset.y + offset.z;
        

        // float angle = transform.localRotation.x;
        float angle = transform.localEulerAngles.x * Mathf.Deg2Rad;

        float targetWidth = width + bonus;
        float targetHeight = height - (2 * orthoSize * Mathf.Cos(angle)) + bonus;
        
           // Confiner dimensions and position
        confiner.size = new Vector3(targetWidth, 100, targetHeight); 
        confiner.center = centerPos + offset - new Vector3(0,0,zDelta);

        // Attach confiner to Cinemachine Confiner component
        var confinerComponent = vCam.GetComponent<CinemachineConfiner>();
        if (confinerComponent != null)
        {
            confinerComponent.m_BoundingVolume = confiner;
            confinerComponent.InvalidatePathCache(); // Reset cached data for the confiner
        }
    }
}
