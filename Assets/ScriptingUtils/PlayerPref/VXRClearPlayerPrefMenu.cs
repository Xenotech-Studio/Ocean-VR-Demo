
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Versee.Scripts.Utils
{
    #if UNITY_EDITOR
    public static class VXRClearPlayerPrefMenu
    {
        [MenuItem("Versee/PlayerPref/ClearPlayerPref", priority = 100)]
        private static void ClearPlayerPrefsMenuItem()
        {
            if (EditorUtility.DisplayDialog("Clear PlayerPrefs", "Are you sure you want to clear PlayerPrefs?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("PlayerPrefs cleared!");
            }
        }
    }
    #endif
}