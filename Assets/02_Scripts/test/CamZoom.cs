using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    Camera cam;
    Rigidbody2D playerRb;

    bool zoomIn;

    [Range(2, 10)]
    public float zoomSize;

    [Range(0.01f,0.1f)]
    public float zoomSpeed;

    private void Awake()
    {
        cam = Camera.main;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void ZoomIn()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed);
    }

    void ZoomOut()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 10, zoomSpeed);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log('1');
            ZoomIn();
        }
    }

}
