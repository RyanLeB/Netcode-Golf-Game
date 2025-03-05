using Unity.Netcode;
using TMPro;
using UnityEngine;

public class PointSystem : NetworkBehaviour
{
    
    // UI elements to display the points
    
    public TextMeshProUGUI homePointText;
    public TextMeshProUGUI awayPointText;

    private NetworkVariable<int> homePoint = new NetworkVariable<int>(0);
    private NetworkVariable<int> awayPoint = new NetworkVariable<int>(0);

    
    // Start is called before the first frame update
    
    public override void OnNetworkSpawn()
    {
        homePoint.OnValueChanged += UpdateHomeScoreUI;
        awayPoint.OnValueChanged += UpdateAwayScoreUI;
        
        UpdateHomeScoreUI(0, homePoint.Value);
        UpdateAwayScoreUI(0, awayPoint.Value);
    }
    
    public override void OnDestroy()
    {
        homePoint.OnValueChanged -= UpdateHomeScoreUI;
        awayPoint.OnValueChanged -= UpdateAwayScoreUI;
    }

    // Update the home score UI
    
    
    [ServerRpc(RequireOwnership = false)]
    public void AddHomePointServerRpc()
    {
        homePoint.Value++;
    }
    
    // Update the home score UI
    
    [ServerRpc(RequireOwnership = false)]
    public void AddAwayPointServerRpc()
    {
        awayPoint.Value++;
    }

    private void UpdateHomeScoreUI(int oldValue, int newValue)
    {
        homePointText.text = newValue.ToString();
    }

    // Update the away score UI
    
    private void UpdateAwayScoreUI(int oldValue, int newValue)
    {
        awayPointText.text = newValue.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    
    // Reset the points to 0
    
    public void ResetPointsServerRpc()
    {
        homePoint.Value = 0;
        awayPoint.Value = 0;
    }
}