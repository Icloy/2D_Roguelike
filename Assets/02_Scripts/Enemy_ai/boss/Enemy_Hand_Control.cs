using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hand_Control : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform controlPoint;

    private float duration = 2f;
    private float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(MoveCurve());
    }

    void Update()
    {

    }

    public IEnumerator MoveCurve()
    {
        controlPoint.position = new Vector3(startPoint.position.x - 5f, startPoint.position.y + 1f, startPoint.position.z);
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1f - t;
        float uu = u * u;
        float tt = t * t;
        Vector3 p = uu * p0;
        p += 2f * u * t * p1;
        p += tt * p2;
        return p;
    }
}
