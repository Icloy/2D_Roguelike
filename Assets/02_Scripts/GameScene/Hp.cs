using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    Animator[] anim;

    public static Hp instance = null;

    public static Hp Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        anim = GetComponentsInChildren<Animator>();
    }

    private void Start()
    {
        StopAllCoroutines();
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
                anim[i].enabled = true; 
            }
            else
            {
                StartCoroutine(Damaged(i));
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

    public void Recover(int a)
    {
        hearts[a].sprite = fullHeart;
        anim[a].enabled = true;
        anim[a].Play("Recovery");
    }

    public void buyHp(int m)
    {
        hearts[m - 1].enabled = true;
    }

    IEnumerator Breath()
    {
        while (true)
        {
           foreach (Animator animator in anim)
            {
                animator.Play("Breath");
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator Damaged(int a)
    {
        anim[a].SetTrigger("Damaged");

        yield return new WaitForSeconds(0.45f);

        hearts[a].sprite = emptyHeart;
        anim[a].enabled = false;
    }
}
