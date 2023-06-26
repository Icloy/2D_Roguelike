using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCamera : MonoBehaviour
{
    public Transform target;
    public GameObject Player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float offsetY = 1f;
    public float offsetZ = -10f;
    public float smooth = 5f;

    Vector3 targett;

    public bool cameraSmoothMoving;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        targett = new Vector3(Player.transform.position.x, Player.transform.position.y + offsetY, Player.transform.position.z + offsetZ);

        if (cameraSmoothMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targett, Time.deltaTime * smooth);
        }
        else
        {
            transform.position = targett;
            cameraSmoothMoving = true;
        }
    }
}
