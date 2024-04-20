using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class OwnerVirtualCamera : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            playerVirtualCamera.Follow = gameObject.transform;
            playerVirtualCamera.LookAt = gameObject.transform;
            playerVirtualCamera.gameObject.SetActive(true);
        }
    }
}
