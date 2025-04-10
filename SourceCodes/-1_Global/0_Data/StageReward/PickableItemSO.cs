using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PickableItemSO : GameData
{

    [TextArea(3, 10)]  public string description;    

    //===========================================

    public abstract void Acquire();

    //===========================================
}
