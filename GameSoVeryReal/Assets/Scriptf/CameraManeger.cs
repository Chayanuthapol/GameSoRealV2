using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManeger : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera[] cameras;
    public CameraManeger cameraManeger;
    public CinemachineVirtualCamera startcamfirst;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera camera3;
    public CinemachineVirtualCamera camera4;
    public CinemachineVirtualCamera camera5;
    public CinemachineVirtualCamera topDownCam;

    public CinemachineVirtualCamera startCamera;
    private CinemachineVirtualCamera currentCam;

    void Start()
    {
        currentCam = startcamfirst;
        StartCoroutine(SwitchCameraAfterDelay());

    }

    IEnumerator SwitchCameraAfterDelay()
    {
        yield return new WaitForSeconds(4);  
        currentCam = startCamera;

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == currentCam)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }

        }
    }

// Update is called once per frame
    void Update()
    {
        // Check if key 1 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Key 1 pressed");
            cameraManeger.SwitchCamera(cameraManeger.startCamera);
        }

        // Check if key 2 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Key 2 pressed");
            cameraManeger.SwitchCamera(cameraManeger.camera2);
        
        }
        // Check if key 3 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Key 3 pressed");
            cameraManeger.SwitchCamera(cameraManeger.camera3);
        }

        // Check if key 4 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Key 4 pressed");
            cameraManeger.SwitchCamera(cameraManeger.camera4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Key 5 pressed");
            cameraManeger.SwitchCamera(cameraManeger.camera5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Key 5 pressed");
            cameraManeger.SwitchCamera(cameraManeger.startcamfirst);
        }
    }
    public void SwitchCamera(CinemachineVirtualCamera newCam)
    {
        currentCam = newCam;

        currentCam.Priority = 20;

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != currentCam)
            {
                cameras[i].Priority = 10;
            }
        }
    }

}
