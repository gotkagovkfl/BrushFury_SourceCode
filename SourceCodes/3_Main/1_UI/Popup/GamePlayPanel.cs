using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayPanel : MonoBehaviour
{
    protected bool initialized;

    public void ForceInit()
    {
        Init();
    }
    protected abstract void Init();


    public void Open()
    {
        if (initialized==false)
        {
            Init();
            initialized = true;
        }
        
        GamePlayManager.Instance.OpenPanel(this);

        gameObject.SetActive(true);
        OnOpen();
        
    }

    protected abstract void OnOpen();

    public void Close()
    {
        GamePlayManager.Instance.ClosePanel(this);
        OnClose();
        gameObject.SetActive(false);
    }

    protected abstract void OnClose();
}
