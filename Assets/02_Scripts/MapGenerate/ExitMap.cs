using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMap : MonoBehaviour
{
    bool exitTrigger = false;
    GameObject exitpos;
    GameObject player;
    GameObject map;
    [SerializeField] GameObject remap;
    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveMap()
    {
        while (exitTrigger)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                player.transform.position = new Vector3(exitpos.transform.position.x, exitpos.transform.position.y + 2f, exitpos.transform.position.z);
                Destroy(map);
                Instantiate(remap, new Vector3(-400, 0, 0), Quaternion.identity);
            }
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);
            player = collision.gameObject;
            exitpos = GameObject.FindWithTag("ExitMap");
            map = GameObject.FindWithTag("BSPMap");
            exitTrigger = true;
            StartCoroutine("MoveMap");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(false);
            exitTrigger = false;
            StopCoroutine("MoveMap");
        }
    }
}
