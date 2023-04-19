using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rotate : MonoBehaviour
{
    public Transform center;
    public float radius;
    public float rotatespeed;
    float angle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rotate(center, radius, rotatespeed));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator rotate(Transform center, float radius, float rotatespeed)
    {
        angle = 0f;
        while (true)
        {
            angle += rotatespeed * Time.deltaTime;

            // ���ο� ��ġ ���
            Vector3 newPosition = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)-0.2f, 0.0f) * radius;

            // �߽����� �Բ� ��ġ ����
            transform.position = center.position + newPosition;
            yield return new WaitForFixedUpdate();
        }
    }
}
