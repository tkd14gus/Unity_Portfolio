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
}
