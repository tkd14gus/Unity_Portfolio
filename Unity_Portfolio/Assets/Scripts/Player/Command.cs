using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    //몇마리인지
    private int num;

    public int Num
    {
        get { return num; }
        set { num = value; }
    }

    public void MoveCommand(Transform GrassBlockMove)
    {
        //갈 곳의 위치를 찾는다.
        Vector3 vt = GrassBlockMove.position;

        ArmyFSM[] army = gameObject.GetComponentsInChildren<ArmyFSM>();

        army[0].MPOINT = GrassBlockMove.position;

        //병사들에게 움직이라고 명령
        for (int i = 1; i < army.Length; i++)
        {
            //위치 조정
            //한 병사마다 45도 각도를 더해서 나온다.
            Quaternion ro = Quaternion.Euler(0, (360 / (army.Length - 1)) * i, 0);
            //가야할 위치가 po에서 각도별로 나눠지니
            //po를 중심으로 각보별 forward를 더해준다.
            Vector3 po = GrassBlockMove.position;
            po += ro * (GrassBlockMove.forward * 0.35f);
            army[i].MPOINT = po;
        }

    }

    //치료 받으라고 건물로 이동 명령
    public void HealCommand(Transform HealMove)
    {
        ArmyFSM[] army = gameObject.GetComponentsInChildren<ArmyFSM>();

        //병사들에게 움직이라고 명령
        for (int i = 0; i < army.Length; i++)
        {
            army[i].HPOINT = HealMove;
        }
    }


    //몇마리를 다시 소환할지와 어느 위치에 소환되는지를 받는다.
    public void HealComeOut(int count, Transform place, ref Transform[] armyArray)
    {
        int aClass = 0;

        //armyArray[0]은 armyGroup이다.
        for (int i = 1; i < armyArray.Length; i++)
        {
            //꺼져있다면 치료중인 녀석들
            //살려준다.
            armyArray[i].gameObject.SetActive(true);
            armyArray[i].GetComponent<ArmyFSM>().HP = 100;
            //병종이 뭔지 확인
            if (armyArray[i].name.Contains("Army"))
                aClass = 0;
            //새로 추가해야 하는 병사를 빼준다.
            count--;
        }

        print(count);
        //필요한 만큼 병사들을 instaiate해준다.
        for (int i = 0; i < count; i++)
        {
            //싱글톤
            ArmyManager.instance.AddArmy(aClass, armyArray[0]);
        }

        //모두 살리고 추가 했으면 이제 위치를 잡아준다.

        //갈 곳의 위치를 찾는다.
        Vector3 vt = place.position;

        ArmyFSM[] army = gameObject.GetComponentsInChildren<ArmyFSM>();

        army[0].MPOINT = place.position;

        //병사들에게 움직이라고 명령
        for (int i = 1; i < army.Length; i++)
        {
            //위치 조정
            //한 병사마다 45도 각도를 더해서 나온다.
            Quaternion ro = Quaternion.Euler(0, (360 / (army.Length - 1)) * i, 0);
            //가야할 위치가 po에서 각도별로 나눠지니
            //po를 중심으로 각보별 forward를 더해준다.
            Vector3 po = place.position;
            po += ro * (place.forward * 0.35f);
            army[i].MPOINT = po;
        }
    }

    //탈출하라고 배로 이동 명령
    public void EscapeCommand(Transform ShipMove)
    {
        ArmyFSM[] army = gameObject.GetComponentsInChildren<ArmyFSM>();

        //병사들에게 움직이라고 명령
        for (int i = 0; i < army.Length; i++)
        {
            army[i].EPOINT = ShipMove;
        }
    }
}
