using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    //public int health;
    //public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    Animator[] anim;
    private void Awake()
    {
        anim = GetComponentsInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine("Breath");
    }

    public void udtHp(int health, int numOfHearts)
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public IEnumerator Breath()
    {
        while (true)
        {
            foreach(Animator animator in anim)
            {
                animator.Play("Breath");
            }
            yield return new WaitForSeconds(5.0f);
        }
    }
}
