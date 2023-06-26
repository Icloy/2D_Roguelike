using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public Transform PlayerPos;
    Vector3 position;

    void Start()
    {
        position = PlayerPos.position;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Move()
    {
        while (true)
        {
            Vector2 dir = new Vector2(PlayerPos.transform.position.x - transform.position.x, PlayerPos.transform.position.y - transform.position.y);
            Quaternion rot = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            transform.rotation = rot;
            yield return new WaitForFixedUpdate();
        }
    }
}
