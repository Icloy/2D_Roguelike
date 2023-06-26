using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Bringer_AttackCheck : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject deathbringer;
    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;
            Death_Bringer deathbringer = transform.parent.GetComponent<Death_Bringer>();
            if (deathbringer != null)
            {
                Debug.Log(deathbringer.attack_damage);
                CSake.instance.Vibrate(1f);
                Player.instance.Damaged(-deathbringer.attack_damage);
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