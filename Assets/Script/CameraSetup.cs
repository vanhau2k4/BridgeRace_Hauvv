using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public GameObject player;
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {

        if (virtualCamera != null && player != null)
        {
            Transform followPoint = player.transform.Find("CameraFollow");
            Transform lookAtPoint = player.transform.Find("CamerePlayer");

            if (followPoint != null && lookAtPoint != null)
            {
                virtualCamera.Follow = followPoint;
                virtualCamera.LookAt = lookAtPoint;
            }
            else
            {
                Debug.LogWarning("FollowPoint hoặc LookAtPoint không tìm thấy trên player!");
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Virtual Camera hoặc Player!");
        }
    }
    public static void GanCamera(Player player)
    {
        var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = player.followPoint;
            virtualCamera.LookAt = player.lookAtPoint;
        }
    }
}
