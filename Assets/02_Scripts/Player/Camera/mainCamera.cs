using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    public static mainCamera Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<mainCamera>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("mainCamera");
                    instance = instanceContainer.AddComponent<mainCamera>();
                }
            }
            return instance;
        }
    }

    private static mainCamera instance;

    public GameObject Player;

    public float offsetY = 1f;
    public float offsetZ = -10f;
    public float smooth = 5f;

    Vector3 target;

    public bool cameraSmoothMoving;


   

    private void LateUpdate()
    {
        target = new Vector3(Player.transform.position.x, Player.transform.position.y + offsetY, Player.transform.position.z + offsetZ);

        if (cameraSmoothMoving)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);
        }
        else
        {
            transform.position = target;
            cameraSmoothMoving = true;
        }
    }
}
