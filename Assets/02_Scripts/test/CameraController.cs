using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
        public float zoomSpeed = 5f; // ī�޶� �� �ӵ� ������ ����
        public float minZoom = 5f; // �ּ� �� �Ÿ�
        public float maxZoom = 20f; // �ִ� �� �Ÿ�

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // A Ű�� ������ ī�޶� ����
            if (Input.GetKey(KeyCode.X))
            {
                ZoomIn();
            }
            // A Ű�� �� �� ī�޶� �ܾƿ�
            if (Input.GetKeyUp(KeyCode.X))
            {
                ZoomOut();
            }
        }

        private void ZoomIn()
        {
        // ���� ī�޶��� �Ÿ��� ���̸鼭 ����
            float newZoom = Mathf.Clamp(mainCamera.transform.position.z + Time.deltaTime * zoomSpeed, -maxZoom, -minZoom);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, newZoom);
        }

        private void ZoomOut()
        {
            // ���� ī�޶��� �Ÿ��� �ø��鼭 �ܾƿ�
            float newZoom = Mathf.Clamp(mainCamera.transform.position.z - Time.deltaTime * zoomSpeed, -maxZoom, -minZoom);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, newZoom);
        }
}

