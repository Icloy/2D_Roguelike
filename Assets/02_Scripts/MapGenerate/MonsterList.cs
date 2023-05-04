using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterList : MonoBehaviour
{
    [SerializeField] List<GameObject> walkmonsterList;
    [SerializeField] List<GameObject> flymonsterList;
    [SerializeField] List<GameObject> middlemonsterList;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void insertmonster()
    {
        middlemonsterinsert();
        walkmonsterinsert();
        flymonsterinsert();
    }

    void walkmonsterinsert()
    {
        walkmonsterList.Add(Resources.Load<GameObject>("Monster/Goblin"));
        walkmonsterList.Add(Resources.Load<GameObject>("Monster/Mushroom"));
        walkmonsterList.Add(Resources.Load<GameObject>("Monster/Skeleton"));
        walkmonsterList.Add(Resources.Load<GameObject>("Monster/Slime"));
    }

    void flymonsterinsert()
    {
        flymonsterList.Add(Resources.Load<GameObject>("Monster/Bat"));
        flymonsterList.Add(Resources.Load<GameObject>("Monster/Flyingeye"));
    }

    void middlemonsterinsert()
    {
        middlemonsterList.Add(Resources.Load<GameObject>("Monster/DeathBirnger"));
    }

    public GameObject GetRandomFlyMonster()
    {
        int randomIndex = Random.Range(0, flymonsterList.Count);
        return flymonsterList[randomIndex];
    }

    public GameObject GetRandomWalkMonster()
    {
        int randomIndex = Random.Range(0, walkmonsterList.Count);
        return walkmonsterList[randomIndex];
    }

    public GameObject GetRandomMiddleMonster()
    {
        int randomIndex = Random.Range(0, middlemonsterList.Count);
        return middlemonsterList[randomIndex];
    }
}
