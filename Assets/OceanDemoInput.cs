using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanDemoInput : MonoBehaviour
{
    
    public bool PressYToReloadScene = true;

    // Update is called once per frame
    void Update()
    {
        // if PICO Y button is pressed, then reload the scene
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && PressYToReloadScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
