using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterList : MonoBehaviour
{
    [SerializeField] List<GameObject> walkmonsterList;
    [SerializeField] List<GameObject> flymonsterList;
    [SerializeField] List<GameObject> middlemonsterList;
    [SerializeField] GameObject walk1;
    [SerializeField] GameObject walk2;
    [SerializeField] GameObject walk3;
    [SerializeField] GameObject walk4;
    [SerializeField] GameObject fly1;
    [SerializeField] GameObject fly2;
    [SerializeField] GameObject middle1;

    // Start is called before the first frame update
    void Start()
    {

    }

    void walkmonsterinsert()
    {
        /*
        walkmonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/Goblin.prefab"));
        walkmonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/Mushroom.prefab"));
        walkmonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/Skeleton.prefab"));
        walkmonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/Slime.prefab"));
        */
        walkmonsterList.Add(walk1);
        walkmonsterList.Add(walk2);
        walkmonsterList.Add(walk3);
        walkmonsterList.Add(walk4);
    }

    void flymonsterinsert()
    {
        /*
        flymonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/Bat.prefab"));
        flymonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/Flyingeye.prefab"));
        */
        flymonsterList.Add(fly1);
        flymonsterList.Add(fly2);
    }

    void middlemonsterinsert()
    {
        //middlemonsterList.Add(Resources.Load<GameObject>("04_Prefabs/Monster/DeathBringer.prefab"));
        middlemonsterList.Add(middle1);
    }

    public GameObject GetRandomFlyMonster()
    {
        flymonsterinsert();
        int randomIndex = Random.Range(0, flymonsterList.Count);
        return flymonsterList[randomIndex];
    }

    public GameObject GetRandomWalkMonster()
    {
        walkmonsterinsert();
        int randomIndex = Random.Range(0, walkmonsterList.Count);
        return walkmonsterList[randomIndex];
    }

    public GameObject GetRandomMiddleMonster()
    {
        middlemonsterinsert();
        int randomIndex = Random.Range(0, middlemonsterList.Count);
        return middlemonsterList[randomIndex];
    }
}
