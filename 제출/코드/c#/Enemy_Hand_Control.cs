using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hand_Control : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform controlPoint;
    public Transform PlayerPos;
    public GameObject bullet;

    private float duration = 5f;
    private float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(SetPlayerPos());
    }

    void Update()
    {
        
    }

    public IEnumerator SetPlayerPos()
    {
        while(true)
        {
            yield return StartCoroutine(ResetMove());
            Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            yield return StartCoroutine(CatchPlayer());
        }
    }

    public IEnumerator CatchPlayer()
    {
        controlPoint.transform.position = new Vector3(PlayerPos.position.x - 3f, PlayerPos.position.y+1f, PlayerPos.position.z);
        endPoint.transform.position = new Vector3(PlayerPos.position.x, PlayerPos.position.y, PlayerPos.position.z);
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        elapsedTime = 0f;
    }

    public IEnumerator ResetMove()
    {
        controlPoint.position = new Vector3(startPoint.position.x + 2f, startPoint.position.y + 1f, startPoint.position.z);
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = CalculateBezierPoint(t, endPoint.position, controlPoint.position, startPoint.position);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        elapsedTime = 0f;
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
