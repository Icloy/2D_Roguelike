using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Trigger : MonoBehaviour
{
    public GameObject Left_Door;
    public GameObject Right_Door;
    public GameObject Boss;

    public GameObject MainBGM;

    public AudioSource audioSource;
    public AudioClip closeDoor;

    [SerializeField] PlayerAudio playerAudio = null;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MainBGM.gameObject.SetActive(false);
            Left_Door.gameObject.SetActive(true);
            Right_Door.gameObject.SetActive(true);
            Boss.gameObject.SetActive(true);
            audioSource.clip = closeDoor;
            audioSource.Play();
            playerAudio.OtherPlay(PlayerAudio.OtherAudioType.BossBGM, true);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

}
