using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton_AttackCheck : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject skeleton;
    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;
            Skeleton skeleton = transform.parent.GetComponent<Skeleton>();
            if (skeleton != null)
            {
                Debug.Log(skeleton.attack_damage);
                CSake.instance.Vibrate(1f);
                Player.instance.Damaged(-skeleton.attack_damage);
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