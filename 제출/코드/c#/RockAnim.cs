using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAnim : Breakable
{

    [SerializeField] private AudioClip RockCut;
    [SerializeField] private GameObject RockAlive;
    [SerializeField] private GameObject RockDeadParticle;

    [SerializeField] private SpriteRenderer RockDead;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {
        CheckIsDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("AttackZone"))
        {
            Hurt(1);
        }
    }

    protected override void Dead()
    {
        base.Dead();
        audioSource.PlayOneShot(RockCut);
        RockDeadParticle.SetActive(true);
        RockAlive.SetActive(false);
        RockDead.enabled = true;
    }

}
