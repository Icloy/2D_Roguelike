using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Damage : MonoBehaviour
{

    public int AtDmg; //���� ������
    [HideInInspector] public GameObject Stat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("1");
            Stat.GetComponent<Stat>().MP += 30;
            // ������ ��� �� ����
            other.gameObject.GetComponent<Enemy>().TakeDamage(AtDmg);
            ShakeCamera.instance.StartShake(0.05f, 0.05f);
        }
    }
}
