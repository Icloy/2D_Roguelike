using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public float speed;
    public int BoltAtDmg; //볼트 공격 데미지


    public float distance;
    public LayerMask isLayer;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestoryBolt", 2);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.right, distance, isLayer);
        if(ray.collider != null)
        {
            if(ray.collider.tag == "Enemy")
            {
                Debug.Log("hit");
                ray.collider.GetComponent<Enemy>().TakeDamage(BoltAtDmg);
            }
            DestoryBolt();
        }
        if(transform.rotation.y == 0)
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
        else
        {
        transform.Translate(transform.right * speed * Time.deltaTime);
        }
    }

    void DestoryBolt()
    {
        Destroy(gameObject);
    }
}
