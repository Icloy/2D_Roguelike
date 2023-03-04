using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Sprite emptyHeart; //����Ʈ
    public Sprite staticHeart; //����� ��Ʈ

    Image image;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
        StartCoroutine("Breath");
    }

    public void damaged()
    {
        StopCoroutine("Breath");
        anim.SetTrigger("Damaged");
        image.sprite = emptyHeart;
    }

    public void Recovery()
    {
        anim.Play("Recovery");
        image.sprite = staticHeart;

    }

    public IEnumerator Breath()
    {
        while (true)
        {
            anim.Play("Breath");
            yield return new WaitForSeconds(5.0f);
        }
    }
}
