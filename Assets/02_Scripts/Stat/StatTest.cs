using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTest : MonoBehaviour
{
    public Image HPbar1;
    public Image HPbar2;
    public Image HPbar3;


    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Player.GetComponent<Player>().curHp >= 100)
        {
            HPbar1.gameObject.SetActive(true);
        }
        else if (Player.GetComponent<Player>().curHp < 100)
        {
            HPbar1.gameObject.SetActive(false);
        }

        if (Player.GetComponent<Player>().curHp >= 200)
        {
            HPbar2.gameObject.SetActive(true);
        }
        else if (Player.GetComponent<Player>().curHp < 200)
        {
            HPbar2.gameObject.SetActive(false);
        }

        if (Player.GetComponent<Player>().curHp >= 300)
        {
            HPbar3.gameObject.SetActive(true);
        }
        else if (Player.GetComponent<Player>().curHp < 300)
        {
            HPbar3.gameObject.SetActive(false);
        }
    }

    
}
