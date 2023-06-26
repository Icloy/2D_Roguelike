using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTest : MonoBehaviour
{
    public Image HPbar1;
    public Image HPbar2;
    public Image HPbar3;
    public Image HPbar4;
    public Image HPbar4c;

    public Image HPbar5;
    public Image HPbar5c;

    public Image HPbar6;
    public Image HPbar6c;




    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        HealthUp();
    }

    void HealthUp()
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

        if (Player.GetComponent<Player>().curHp >= 400)
        {
            HPbar4.gameObject.SetActive(true);
        }
        else if (Player.GetComponent<Player>().curHp < 400)
        {
            HPbar4.gameObject.SetActive(false);
        }

        if (Player.GetComponent<Player>().curHp >= 500)
        {
            HPbar5.gameObject.SetActive(true);
        }
        else if (Player.GetComponent<Player>().curHp < 500)
        {
            HPbar5.gameObject.SetActive(false);
        }

        if (Player.GetComponent<Player>().curHp >= 600)
        {
            HPbar6.gameObject.SetActive(true);
        }
        else if (Player.GetComponent<Player>().curHp < 600)
        {
            HPbar6.gameObject.SetActive(false);
        }

        if (Player.GetComponent<Player>().maxHp >= 400)
        {
            HPbar4c.gameObject.SetActive(true);
        }
        if (Player.GetComponent<Player>().maxHp >= 500)
        {
            HPbar5c.gameObject.SetActive(true);
        }
        if (Player.GetComponent<Player>().maxHp >= 600)
        {
            HPbar6c.gameObject.SetActive(true);
        }
    }


}
