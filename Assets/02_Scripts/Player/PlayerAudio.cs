using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource mainEffectAudioSouce = null;
    [Header("Audio Sources")]

    [SerializeField] AudioSource AttackSound = null;
    [SerializeField] AudioSource JumpSound = null;
    [SerializeField] AudioSource HealSound = null;
    [SerializeField] AudioSource DashSound = null;
    [SerializeField] AudioSource DamagedSound = null;


    public enum AudioType
    {
        Attack, Jump, Heal, Dash, Damaged
    }

    public void Play(AudioType audioType, bool playState)
    {
        AudioSource audioSource = null;
        switch (audioType)
        {
            case AudioType.Attack:
                audioSource = AttackSound;
                Debug.Log("!");
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
