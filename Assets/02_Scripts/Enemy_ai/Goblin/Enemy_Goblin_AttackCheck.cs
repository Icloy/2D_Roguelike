using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goblin_AttackCheck : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject goblin;
    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;
            Goblin goblin = transform.parent.GetComponent<Goblin>();
            if (goblin != null)
            {
                Debug.Log(goblin.attack_damage);
                ShakeCamera.instance.StartShake(0.05f, 0.05f);
                Player.instance.Damaged(-goblin.attack_damage);
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