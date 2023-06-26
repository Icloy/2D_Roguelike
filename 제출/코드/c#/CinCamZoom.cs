using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinCamZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 9f;

    private void Update()
    {
        float zoom = 0f;
        if (Input.GetKey(KeyCode.Z))
        {
            zoom += zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.X))
        {
            zoom -= zoomSpeed * Time.deltaTime;
        }

        float newOrthoSize = virtualCamera.m_Lens.FieldOfView - zoom;

        // 최소값과 최대값을 벗어나지 않도록 조정합니다.
        newOrthoSize = Mathf.Clamp(newOrthoSize, minZoom, maxZoom);

        virtualCamera.m_Lens.FieldOfView = newOrthoSize;
    }
    
}
