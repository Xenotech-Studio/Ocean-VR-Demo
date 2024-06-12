using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


public partial class VXRSceneManager
{
    public static string sceneArrayDataPath = "Assets/Resources/SceneArrayData.asset";

    // Runtime-Accessible Method
    // Example: int index = VXRSceneManager.GetSceneIndexByName("0_Launcher");
    public static int GetSceneIndexByName(string sceneName)
    {
        SceneArrayData sceneArrayData = Resources.Load<SceneArrayData>("SceneArrayData");
        if (sceneArrayData == null)
        {
            return -1;
        }
        var enabledSceneList = new List<string>();
        foreach (var info in sceneArrayData.scenes)
        {
            if (info.enable)
            {
                enabledSceneList.Add(info.name);
            }
        }

        
       int sceneInfo = Array.IndexOf(enabledSceneList.ToArray(), sceneName);
       return sceneInfo;

    }
    
    // Runtime-Accessible Method
    public static string[] GetSceneNames()
    {
        SceneArrayData sceneArrayData = Resources.Load<SceneArrayData>("SceneArrayData");
        if (sceneArrayData == null)
        {
            return null;
        }
        return sceneArrayData.scenes.Select(s => s.name).ToArray();
    }
    
    public static string[] GetScenePaths()
    {
        SceneArrayData sceneArrayData = Resources.Load<SceneArrayData>("SceneArrayData");
        if (sceneArrayData == null)
        {
            return null;
        }
        return sceneArrayData.scenes.Select(s => s.path).ToArray();
    }

    

}

#if UNITY_EDITOR
public partial class VXRSceneManagerWindow : EditorWindow
{
    
    private List<EditorBuildSettingsScene> scenesInBuildSettings;
    private Vector2 scrollPosition;
    private ReorderableList reorderableList;

    
    [MenuItem("Versee/Scene Manager/Scene Manager", priority = 1000)]
    public static void ShowWindow()
    {
        GetWindow<VXRSceneManagerWindow>("VXR Scene Manager");
    }
    
    private void OnEnable()
    {
        //LoadOrCreateSceneArrayData(); // Ensure Asset Exists
        LoadScenes();
    }
    
    private void LoadScenes()
    {
        scenesInBuildSettings = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        reorderableList = new ReorderableList(scenesInBuildSettings, typeof(EditorBuildSettingsScene), true, false, true, true);

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var scene = scenesInBuildSettings[index];
            rect.y += 2;
    
            EditorGUILayout.BeginHorizontal();
            scene.enabled = EditorGUI.Toggle(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), scene.enabled);
            scene.path = EditorGUI.TextField(new Rect(rect.x + 25, rect.y, rect.width - 100, EditorGUIUtility.singleLineHeight), scene.path);
            if (GUI.Button(new Rect(rect.x + rect.width - 70, rect.y, 60, EditorGUIUtility.singleLineHeight), "Open"))
            {
                // if the scene has some changes, ask to save
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
            }
            EditorGUILayout.EndHorizontal();
        };
        
        reorderableList.onAddCallback = (ReorderableList list) =>
        {
            scenesInBuildSettings.Add(new EditorBuildSettingsScene("", true));
        };

        reorderableList.onRemoveCallback = (ReorderableList list) =>
        {
            scenesInBuildSettings.RemoveAt(list.index);
        };

        reorderableList.onChangedCallback = (ReorderableList list) =>
        {
            SaveScenes();
        };
 
    }
    
    private void SaveScenes()
    {
        EditorBuildSettings.scenes = scenesInBuildSettings.ToArray();
        SaveSceneArrayData(scenesInBuildSettings);
    }
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scenes in Build Settings", EditorStyles.boldLabel);
        reorderableList.DoLayoutList();
    }
    
    private static SceneArrayData LoadOrCreateSceneArrayData()
    {
        SceneArrayData sceneArrayData = Resources.Load<SceneArrayData>("SceneArrayData");

        if (sceneArrayData == null)
        {
            sceneArrayData = ScriptableObject.CreateInstance<SceneArrayData>();
            AssetDatabase.CreateAsset(sceneArrayData, VXRSceneManager.sceneArrayDataPath);
            AssetDatabase.SaveAssets();
            
            // Refresh the AssetDatabase to make sure the new asset is recognized
            AssetDatabase.Refresh();
            
            // Load the newly created asset
            sceneArrayData = Resources.Load<SceneArrayData>("SceneArrayData");
        }
        return sceneArrayData;
    }
    
    public static void SaveSceneArrayData(List<EditorBuildSettingsScene> scenesInBuildSettings)
    {
        SceneArrayData sceneArrayData = LoadOrCreateSceneArrayData();
        sceneArrayData.scenes = scenesInBuildSettings.Select((scene, index) => new SceneInfo
        {
            name = Path.GetFileNameWithoutExtension(scene.path),
            path = scene.path,
            index = index,
            enable=scene.enabled
        }).ToArray();

        EditorUtility.SetDirty(sceneArrayData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    public static void InitializeSceneArrayData()
    {
        var _scenesInBuildSettings = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        SceneArrayData sceneArrayData = LoadOrCreateSceneArrayData();
        sceneArrayData.scenes = _scenesInBuildSettings.Select((scene, index) => new SceneInfo
        {
            name = Path.GetFileNameWithoutExtension(scene.path),
            path = scene.path,
            index = index,
            enable=scene.enabled
        }).ToArray();

        EditorUtility.SetDirty(sceneArrayData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
#endif