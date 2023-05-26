using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource mainEffectAudioSouce;
    [Header("Audio Sources")]

    [SerializeField] AudioSource AttackSound = null;
    [SerializeField] AudioSource JumpSound = null;
    [SerializeField] AudioSource HealSound = null;
    [SerializeField] AudioSource DashSound = null;
    [SerializeField] AudioSource DamagedSound = null;
    [SerializeField] AudioSource LandSound = null;
    [SerializeField] AudioSource Damaged_EnemySound = null;
    [SerializeField] AudioSource takeDamagedSound = null;



    public enum AudioType
    {
        Attack, Jump, Heal, Dash, Damaged, Land, Dameged_Enemy, TakeDamaged
    }

    public void Play(AudioType audioType, bool playState)
    {
        AudioSource audioSource = null;
        switch (audioType)
        {
            case AudioType.Attack:
                audioSource = AttackSound;
                break;
            case AudioType.Jump:
                audioSource = JumpSound;
                break;
            case AudioType.Heal:
                audioSource = HealSound;
                break;
            case AudioType.Dash:
                audioSource = DashSound;
                break;
            case AudioType.Damaged:
                audioSource = DamagedSound;
                break;
            case AudioType.Land:
                audioSource = LandSound;
                break;
            case AudioType.Dameged_Enemy:
                audioSource = Damaged_EnemySound;
                break;
            case AudioType.TakeDamaged:
                audioSource = takeDamagedSound;
                break;
        }
        if (audioSource != null)
        {
            if (playState)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

}
