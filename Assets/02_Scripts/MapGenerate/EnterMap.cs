using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMap : MonoBehaviour
{
    bool enterTrigger;
    bool createTrigger;
    GameObject enterpos;
    GameObject player;
    [SerializeField] GameObject map;
    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        enterTrigger = false;
        createTrigger = true;
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
    void CreateMap()
    {
        createTrigger = false;
        if (map != null)
        {
            map.GetComponent<MapGenerator>().CreateBspMap();
        }
    }
    public void SetCreateTrigger()
    {
        createTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(createTrigger)
            {
                CreateMap();
            }
            text.gameObject.SetActive(true);
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
            text.gameObject.SetActive(false);
            enterTrigger = false;
            StopCoroutine("MoveMap");
        }
    }
}
