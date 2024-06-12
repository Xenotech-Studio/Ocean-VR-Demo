using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VXREventBasedLogger : MonoBehaviour
{
    public bool ShowCurrentGameObjectName;
    
    public void Log(string log)
    {
        Debug.Log($"[VXR] {(ShowCurrentGameObjectName ? name + ": " : "")} {log}");
    }
}
