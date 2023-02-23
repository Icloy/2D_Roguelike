using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    private Stats stats;

    public float HP
    {
        set => stats.HP = Mathf.Clamp(value, 0, MaxHP);
        get => stats.HP;
    }

    public float MP
    {
        set => stats.MP = Mathf.Clamp(value, 0, MaxMP);
        get => stats.MP;
    }

    public abstract float MaxHP { get; }
    public abstract float HPRecovery { get; }
    public abstract float MaxMP { get; } //최대마나
    public abstract float MPRecovery { get; }

   

    protected void Setup()
    {
        HP = 99;
        MP = 25;

        StartCoroutine("Recovery");
    }

    protected IEnumerator Recovery()
    {
        while (true)
        {
            if (HP < MaxHP) HP += HPRecovery;
            if (MP < MaxMP) MP += MPRecovery;

            yield return new WaitForSeconds(1);
        }
    }

    [System.Serializable]
    public struct Stats
    {
        [HideInInspector]
        public float HP;
        [HideInInspector]
        public float MP;
    }
}
