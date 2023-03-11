using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject bolt;
    public Transform pos;
    public float Boltcooltime;
    private float curtime;

    private AudioSource AudioPlayer; //오디오 소스 컴포넌트
    public AudioClip BoltAttackSound;

    public GameObject Player;

    void Awake()
    {
        AudioPlayer = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        // 위치
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }



        if (curtime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(Player.GetComponent<Stat>().MP < 50)
                {
                    Debug.Log("마나가 부족합니다");
                }
                else
                {
                    Instantiate(bolt, pos.position, transform.rotation);
                    AudioPlayer.PlayOneShot(BoltAttackSound);
                    curtime = Boltcooltime;
                    Player.GetComponent<Stat>().MP -= 50;
                }
            }
        }
        curtime -= Time.deltaTime;
    }
    
}
