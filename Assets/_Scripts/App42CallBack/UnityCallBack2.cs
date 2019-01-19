using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System;
using UnityEngine;

public class UnityCallBack2 : App42CallBack
{
    public void OnSuccess(object response)
    {
        Debug.Log(response);
    }
    public void OnException(Exception e)
    {
      
    }
}
