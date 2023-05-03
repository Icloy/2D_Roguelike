using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMap : MonoBehaviour
{
    bool enterTrigger = false;
    GameObject enterpos;
    GameObject player;
    [SerializeField] GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(map, new Vector3(-400,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveMap()
    {
        while (enterTrigger)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                player.transform.position = new Vector3(enterpos.transform.position.x, enterpos.transform.position.y + 2f, enterpos.transform.position.z);
            }
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            enterpos = GameObject.FindWithTag("EnterMap");
            enterTrigger = true;
            StartCoroutine("MoveMap");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enterTrigger = false;
            StopCoroutine("MoveMap");
        }
    }
}
