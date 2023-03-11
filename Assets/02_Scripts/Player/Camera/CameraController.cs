using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
        public float zoomSpeed = 5f; // 카메라 줌 속도 조절용 변수
        public float minZoom = 5f; // 최소 줌 거리
        public float maxZoom = 20f; // 최대 줌 거리

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
        }

        private void ZoomIn()
        {
        // 현재 카메라의 거리를 줄이면서 줌인
            float newZoom = Mathf.Clamp(mainCamera.transform.position.z + Time.deltaTime * zoomSpeed, -maxZoom, -minZoom);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, newZoom);
        }

        private void ZoomOut()
        {
            // 현재 카메라의 거리를 늘리면서 줌아웃
            float newZoom = Mathf.Clamp(mainCamera.transform.position.z - Time.deltaTime * zoomSpeed, -maxZoom, -minZoom);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, newZoom);
        }
}

