using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGrassAnim : Breakable
{
    [SerializeField] private AudioClip grassMove;

    private Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(grassMove);
        }

    }

}
