using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public GameObject HealEffect;
    [HideInInspector] public AudioSource AudioPlayer; //오디오 소스 컴포넌트
    public AudioClip AttackSound;

    // Start is called before the first frame update
    void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
