using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public int collision_damage;
    public int attack_damage;
    public int Hp;
    public int dropcoincnt;
    public GameObject Item;

    public virtual void TakeDamage(int damege)
    {
    }
}
