using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GolfHole : NetworkBehaviour
{
    private PointSystem pointSystem;
    private NetworkObject currentBall;
    
    public override void OnNetworkSpawn()
    {
        pointSystem = GameObject.Find("PointSystem").GetComponent<PointSystem>();
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Ball entered hole");
            HandleGoalServerRpc(gameObject.CompareTag("Flag"));
            NetworkObject networkObject = collider.gameObject.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Despawn();
            }
        }
    }

    [ServerRpc]
    private void HandleGoalServerRpc(bool isHomeGoal)
    {
        if (isHomeGoal)
        {
            pointSystem.AddHomePointServerRpc();
        }
        else
        {
            pointSystem.AddAwayPointServerRpc();
        }
    }
}
