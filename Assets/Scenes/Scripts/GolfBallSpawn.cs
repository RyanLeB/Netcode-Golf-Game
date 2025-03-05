using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GolfBallSpawn : NetworkBehaviour
{
    // Ball prefab to spawn
    
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    // Reference to the current ball in the scene
    private NetworkVariable<NetworkObjectReference> currentBall = new NetworkVariable<NetworkObjectReference>();

    
    private void Start()
    {
        if (IsServer)
        {
            SpawnBall();
        }
    }
    
    
    // Spawns a ball at the spawn point
    private void SpawnBall()
    {
        if (!currentBall.Value.TryGet(out NetworkObject _))
        {
            GameObject ball = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
            NetworkObject networkObject = ball.GetComponent<NetworkObject>();
            networkObject.Spawn();
            currentBall.Value = networkObject;
        }
    }
    
    
    // When the ball enters the hole, destroy the ball and spawn a new one
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            Destroy(collider.gameObject);
            currentBall.Value = default;
            SpawnBall();
            
        }
    }
}
