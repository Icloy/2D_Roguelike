using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom_AttackCheck : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject mushroom;
    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;
            Mushroom mushroom = transform.parent.GetComponent<Mushroom>();
            if (mushroom != null)
            {
                Debug.Log(mushroom.attack_damage);
                CSake.instance.Vibrate(1f);
                Player.instance.Damaged(-mushroom.attack_damage);
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