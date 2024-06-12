using System;
using UnityEngine;

[Serializable]
public class SceneInfo
{
    public string name;
    public string path;
    public int index;
    public bool enable;
}

[CreateAssetMenu(fileName = "SceneArrayData", menuName = "ScriptableObjects/SceneArrayData", order = 1)]
public class SceneArrayData : ScriptableObject
{
    public SceneInfo[] scenes;
}