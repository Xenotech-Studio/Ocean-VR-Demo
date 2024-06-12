using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    
    public Transform PlayerAnchor;
    
    public UnityEvent OnEnterPortal;

    // Update is called once per frame
    void Update()
    {
        // if PICO Y button is pressed, then reload the scene
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && PressYToReloadScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
        if (Portal.gameObject.activeSelf)
        {
            // detect whether the player has entered the portal
            Vector3 relativePosition = Head.transform.position - Portal.transform.position;
            Vector3 PortalForward = Portal.transform.forward;
            
            // calculate projection length
            Debug.DrawLine(Portal.transform.position, Portal.transform.position + PortalForward, Color.red);
            Debug.DrawLine(Portal.transform.position, Head.transform.position, Color.green);
            float relativeDistance = Vector3.Dot(relativePosition, PortalForward);
            //Debug.Log(relativeDistance);
            if (relativeDistance < 0.1f)
            {
                OnEnterPortal?.Invoke();
                Portal.GetComponent<Animator>().SetBool("IsOpen", false);
            }
        }
    }

    public void MovePlayerToAnchor(Transform anchor)
    {
        PlayerAnchor.position = anchor.position;
        PlayerAnchor.rotation = anchor.rotation;
    }
}
