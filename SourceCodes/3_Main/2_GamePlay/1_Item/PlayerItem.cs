using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class PlayerItem<T> where T : ItemDataSO 
{
    public T data;
    public bool activated => data != null;
}
