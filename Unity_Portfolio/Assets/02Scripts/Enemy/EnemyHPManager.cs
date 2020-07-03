using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPManager : MonoBehaviour
{
    EnemyFSM efs;

    //private int hp;
    public int HP
    {
        get { return efs.HP; }
        set { efs.HP = value; }
    }
    public int MaxHP
    {
        get { return efs.MaxHP; }
        set { efs.MaxHP = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.name.Contains("Archer"))
        {
            efs = GetComponent<ArcherEnemyFSM>();
            efs.MaxHP = 80;
            efs.HP = 80;
        }
        else
        {
            efs = GetComponent<EnemyFSM>();
            efs.MaxHP = 100;
            efs.HP = 100;
        }
    }

    public void StartPushed(Transform Lancer)
    {
        efs.StartPushed(Lancer);
    }
}
