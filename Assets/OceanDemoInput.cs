using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OceanDemoInput : MonoBehaviour
{
    
    public bool PressYToReloadScene = true;

    public Transform Portal;
    public Transform Head;
    
    public PostProcessLayer PostProcessLayer;
    public bool EnablePostProcessing
    {
        set => PostProcessLayer.enabled = value;
        get => PostProcessLayer.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        // if PICO Y button is pressed, then reload the scene
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && PressYToReloadScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
        Vector3 relativePosition = Head.transform.position - Portal.transform.position;
        Vector3 PortalForward = Portal.transform.forward;
        float relativeDistance = Vector3.Dot(relativePosition, PortalForward);
        
        //Debug.Log(relativeDistance);
    }
}
