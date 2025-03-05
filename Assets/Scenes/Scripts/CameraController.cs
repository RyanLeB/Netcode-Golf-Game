using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    private void Start()
    {
        // Get the camera component from the child object
        
        Camera playerCamera = GetComponentInChildren<Camera>();

        
        // If the player is the owner of the object, enable the camera
        if (IsOwner)
        {
            playerCamera.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
        }
    }
}