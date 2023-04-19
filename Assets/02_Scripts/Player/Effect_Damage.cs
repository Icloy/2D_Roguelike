using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Damage : MonoBehaviour
{
    public int AtDmg; //공격 데미지
    [HideInInspector] public GameObject Stat;

    public bool repeat = false;
    private Coroutine delayCoroutine = null;

    private void OnEnable()
    {
        AtDmg = Player.instance.AtDmg;
        repeat = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !repeat)
        {
            repeat = true;
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log(AtDmg);
                CSake.instance.Vibrate(1f);
                /*Stat.GetComponent<Stat>().MP += 30;*/
                // 데미지 계산 및 적용
                enemy.TakeDamage(AtDmg);
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