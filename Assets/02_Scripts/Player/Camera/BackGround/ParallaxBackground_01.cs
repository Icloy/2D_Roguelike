using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_01 : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float scrollAmount;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 moveDirection;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if(transform.position.x <= -scrollAmount)
        {
            transform.position = target.position - moveDirection * scrollAmount;
        }
    }
}
