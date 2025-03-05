using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    float speed = 5.0f;
    private Rigidbody playerRb;
    private Camera playerCamera;
    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera != null)
        {
            cameraOffset = playerCamera.transform.position - transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        playerRb.MovePosition(playerRb.position + moveDirection * speed * Time.fixedDeltaTime);

        MovePlayerServerRpc(moveDirection);

        if (playerCamera != null)
        {
            playerCamera.transform.position = transform.position + cameraOffset;
            playerCamera.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }

    // Puts force on the ball when the player collides with it and the player presses the space key
    
    private void OnTriggerStay(Collider collider)
    {
        if (!IsOwner)
        {
            return;
        }

        if (collider.gameObject.CompareTag("Ball") && Input.GetKey(KeyCode.Space))
        {
            ulong networkObjectId = collider.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            ApplyForceToBallServerRpc(networkObjectId);
        }

        Debug.Log("Triggered");
    }

    [ServerRpc]
    private void ApplyForceToBallServerRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            Vector3 forceDirection = networkObject.transform.position - transform.position;
            forceDirection.Normalize();
            networkObject.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);
            Debug.Log("Hit the ball");
        }
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector3 moveDirection)
    {
        playerRb.MovePosition(playerRb.position + moveDirection * speed * Time.fixedDeltaTime);
    }
}