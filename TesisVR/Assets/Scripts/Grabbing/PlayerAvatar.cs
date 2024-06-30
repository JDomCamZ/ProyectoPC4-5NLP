using UnityEngine;
using Fusion;

public class PlayerAvatar : NetworkBehaviour
{

    public GameObject HeadsetVisuals;

    [Networked(OnChanged = nameof(OnAvatarIndexChanged))]
    public int AvatarIndex { get; set; }

    public GameObject[] avatars;

    void Start()
    {
        Debug.Log("PlayerAvatar Start");
        if (avatars.Length > 0)
        {
            foreach (var avatar in avatars)
            {
                avatar.SetActive(false);
            }
            avatars[AvatarIndex].SetActive(true);
        }
    }

    public override void Spawned()
    {
        Debug.Log("PlayerAvatar Spawned");
        if (Object.HasInputAuthority)
        {
            AvatarIndex = 0;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_SelectAvatar(int index)
    {
        Debug.Log($"Rpc_SelectAvatar called with index: {index}");
        AvatarIndex = index;
    }

    private static void OnAvatarIndexChanged(Changed<PlayerAvatar> changed)
    {
        Debug.Log("OnAvatarIndexChanged called");
        changed.Behaviour.UpdateAvatar(changed.Behaviour.AvatarIndex);
    }

    private void UpdateAvatar(int index)
    {
        Debug.Log($"UpdateAvatar called with index: {index}");
        foreach (var avatar in avatars)
        {
            avatar.SetActive(false);
        }
        if (index >= 0 && index < avatars.Length)
        {
            avatars[index].SetActive(true);
        }
    }
}


