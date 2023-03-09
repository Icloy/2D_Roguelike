using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CamZoom : MonoBehaviour
{
    Camera cam;
    PixelPerfectCamera pixelPerfect;
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
        pixelPerfect = GetComponent<PixelPerfectCamera>();
    }

    private void Start()
    {
        if (pixelPerfect != null)
        {
            // Use pixelPerfect to access the properties and methods of the component.
            // For example, you can adjust the camera's zoom level:
            pixelPerfect.assetsPPU = 16;
        }

    }
    public void ZoomIn()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed);
    }

    public void ZoomOut()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 12, 1);
    }



}
