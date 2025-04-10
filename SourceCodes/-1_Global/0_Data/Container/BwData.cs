using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using System.Data.Common;


//==========================================================

[Serializable]
public class JsonListWrapper<T>
{
    public List<T> data;

    public JsonListWrapper(List<T> data)
    {
        this.data = data;
    }
}
