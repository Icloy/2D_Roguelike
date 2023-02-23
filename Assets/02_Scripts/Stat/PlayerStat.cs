using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.Setup();
    }


    public override float MaxHP => 300;
    public override float HPRecovery => 25;


    public override float MaxMP => 300;
    public override float MPRecovery => 25;

    private void Update()
    {

    }

}
