using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    
    
    public Relay relay;
    public TMP_InputField joinCodeInput;
    public Button hostButton;
    public Button joinButton;

    
    // Start is called before the first frame update
    private void Start()
    {
        hostButton.onClick.AddListener(() => relay.StartHost());
        joinButton.onClick.AddListener(() => relay.JoinGame(joinCodeInput.text));
    }
}