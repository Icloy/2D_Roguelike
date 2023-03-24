using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Flyingeye_Boom : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject flyingeye;
    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;
            Flyingeye flyingeye = transform.parent.GetComponent<Flyingeye>();
            if (flyingeye != null)
            {
                Debug.Log(flyingeye.attack_damage);
                ShakeCamera.instance.StartShake(0.05f, 0.05f);
                Player.instance.Damaged(-flyingeye.attack_damage);
            }
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