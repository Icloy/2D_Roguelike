using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    private Stats stats;

    public float MP
    {
        set => stats.MP = Mathf.Clamp(value, 0, MaxMP);
        get => stats.MP;
    }

    public abstract float MaxMP { get; } //최대마나
    public abstract float MPRecovery { get; }

   

    protected void Setup()
    {
        MP = 25;

        StartCoroutine("Recovery");
    }

    protected IEnumerator Recovery()
    {
        while (true)
        {
            if (MP < MaxMP) MP += MPRecovery;

            yield return new WaitForSeconds(1);
        }
    }

    [System.Serializable]
    public struct Stats
    {
        [HideInInspector]
        public float MP;
    }
}
