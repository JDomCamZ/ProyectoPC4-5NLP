using Fusion;
using TMPro;
using UnityEngine;
using Fusion.XR.Host.Grabbing;


public class AvatarGrabbing : NetworkBehaviour
{   
    public TextMeshProUGUI authorityText;
    public TextMeshProUGUI debugText;
    private PlayerAvatar playerAvatar;
    public int avatarIndex;

    private void Awake()
    {
        debugText.text = "";
        var grabbable = GetComponent<NetworkGrabbable>();
        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.onDidUngrab.AddListener(OnDidUngrab);
    }

    private void DebugLog(string debug)
    {
        debugText.text = debug;
        Debug.Log(debug);
    }

    private void UpdateStatusCanvas()
    {
        if (Object.HasStateAuthority)
            authorityText.text = "You have the state authority on this object";
        else
            authorityText.text = "You have NOT the state authority on this object";
    }

    public override void FixedUpdateNetwork()
    {
        UpdateStatusCanvas();
    }

    void OnDidUngrab()
    {
        DebugLog($"{gameObject.name} ungrabbed");
    }

    void OnDidGrab(NetworkGrabber newGrabber)
    {
        DebugLog($"{gameObject.name} grabbed by {newGrabber.Object.InputAuthority} {newGrabber.hand.side} hand");

        DebugLog($"{newGrabber.Object.HasInputAuthority}");
        if (newGrabber.Object.HasInputAuthority == false)
        {
            if (playerAvatar == null)
            {
                // Busca dinámicamente el PlayerAvatar
                DebugLog("Primero es nulo");
                playerAvatar = FindObjectOfType<PlayerAvatar>();
                DebugLog($"{playerAvatar}");
                DebugLog("Ya no es nulo");
            }

            if (playerAvatar != null)
            {
                DebugLog($"{avatarIndex}");
                playerAvatar.Rpc_SelectAvatar(avatarIndex); // Cambiar al índice de avatar que desees
                DebugLog($"{playerAvatar}");
            }
        }
    }
}
