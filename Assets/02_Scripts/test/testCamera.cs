using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCamera : MonoBehaviour
{
    public Transform target;
    float offsetY = 6f;
    private float offsetZ = -10f;

    private void LateUpdate()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offsetY, target.transform.position.z + offsetZ);
    }
}
