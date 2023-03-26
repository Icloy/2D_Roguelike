using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_AttackCheck : MonoBehaviour
{
    public bool repeat;
    private Coroutine delayCoroutine = null;
    GameObject boss;
    private void OnEnable()
    {
        repeat = false;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !repeat)
        {
            repeat = true;
            Enemy_Boss boss = transform.parent.GetComponent<Enemy_Boss>();
            if (boss != null)
            {
                Debug.Log(boss.attack_damage);
                ShakeCamera.instance.StartShake(0.05f, 0.05f);
                Player.instance.Damaged(-boss.attack_damage);
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