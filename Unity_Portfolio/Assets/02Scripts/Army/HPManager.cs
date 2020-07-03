using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    ArmyFSM afs;

    private bool isEscape = false;
    public bool IsEscape
    {
        get { return isEscape; }
        set { isEscape = value; }
    }
    //private int hp;
    public int HP
    {
        get { return afs.HP; }
        set { afs.HP = value; }
    }
    public int MaxHP
    {
        get { return afs.MaxHP; }
        set { afs.MaxHP = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetComponent<ArmyFSM>().enabled)
        {
            afs = transform.GetComponent<ArmyFSM>();
            afs.MaxHP = 100;
            afs.HP = 100;
        }
        else if (transform.GetComponent<ArcherFSM>().enabled)
        {
            afs = transform.GetComponent<ArcherFSM>();
            afs.MaxHP = 80;
            afs.HP = 80;
        }
        else if (transform.GetComponent<LancerFSM>().enabled)
        {
            afs = transform.GetComponent<LancerFSM>();
            afs.MaxHP = 100;
            afs.HP = 100;
        }
        else if (transform.GetComponent<WorriorFSM>().enabled)
        {
            afs = transform.GetComponent<WorriorFSM>();
            afs.MaxHP = 150;
            afs.HP = 150;
        }
    }
    
}
