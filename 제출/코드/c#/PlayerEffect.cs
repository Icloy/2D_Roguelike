using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem WalkDustL;
    [SerializeField] ParticleSystem WalkDustR;

    public enum EffectType
    {
        WalkDustL, WalkDustR
    }

    public void DoEffect(EffectType effectType, bool enabled)
    {
        switch (effectType)
        {
            case EffectType.WalkDustL:
                if (enabled)
                    WalkDustL.Play();
                break;
            case EffectType.WalkDustR:
                if (enabled)
                    WalkDustR.Play();
                break;
        }
    }
}
