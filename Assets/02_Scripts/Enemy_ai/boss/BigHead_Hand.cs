using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHead_Hand : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject Enemy_Boss_BigHead;

    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;

                Debug.Log("notnull");
                CSake.instance.Vibrate(1f);
                Player.instance.Damaged(-1);
            if (delayCoroutine != null)
            {
                StopCoroutine(delayCoroutine);
            }
            delayCoroutine = StartCoroutine(Delay(0.2f));
        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        repeat = false;
        yield return null;
    }
}