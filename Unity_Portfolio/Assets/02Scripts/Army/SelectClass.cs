using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClass : MonoBehaviour
{

    public int armyClass = 0;

    public int ArmyClass
    {
        get { return armyClass; }
        set { armyClass = value; }
    }

    private int index = 0;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }
}
