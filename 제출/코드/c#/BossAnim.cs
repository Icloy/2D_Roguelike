using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    public Animator anim;
    private bool a = false;

    private void Start()
    {
        // Call the function every 3 seconds, starting after 2 seconds
        InvokeRepeating("MyFunction", 2f, 5f);
    }

    private void MyFunction()
    {
        anim.SetBool("Attack", true );
        a = true;
    }

    private void Update()
    {
        if (a == true)
        {
            anim.SetBool("Attack", false);
        }
    }
}
