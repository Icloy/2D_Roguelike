using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGrassAnim : Breakable
{
    [SerializeField] private AudioClip grassMove;
    [SerializeField] private AudioClip grassCut;
    [SerializeField] private GameObject grassAlive;
    [SerializeField] private GameObject grassDeadParticle;

    private Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();

    }
    private void Update()
    {
        CheckIsDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 diff = (collision.transform.position - transform.position).normalized;
            if (diff.x > 0)
            {
                anim.SetTrigger("GrassRight");
            }
            else if (diff.x < 0)
            {
                anim.SetTrigger("GrassLeft");
            }
            audioSource.PlayOneShot(grassMove);
        }

        if (collision.gameObject.CompareTag("AttackZone"))
        {
            Hurt(1);
        }
    }

    protected override void Dead()
    {
        base.Dead();
        audioSource.PlayOneShot(grassCut);
        grassDeadParticle.SetActive(true);
        grassAlive.SetActive(false);
    }
}
