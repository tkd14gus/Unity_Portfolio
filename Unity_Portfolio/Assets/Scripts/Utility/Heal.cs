using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private Transform armyGroup = null;
    private Transform[] armyArray;
    private float healTime;
    //몇 마리가 들어왔는가.
    private int enterNum;

    //코루틴이 돌아가고 있는지
    private bool isHealing = false;

    private Color c;

    void Start()
    {
        c = transform.GetComponentsInChildren<Transform>()[1].GetComponent<MeshRenderer>().material.color;

    }

    //병사들 치료 함수
    public void ArmyHeal(Transform army)
    {
        //만일 처음 들어온 녀석이면
        if(armyGroup == null)
        {
            //부모(ArmyGroup)을 가져온다.
            armyGroup = army.parent.transform;
            armyArray = armyGroup.GetComponentsInChildren<Transform>();
            //healTime은 치료해야할 병사들 숫자 * 1초다.
            //죽더라도 setActive를 false로 해줄것이 아니라 스트립트를 비활성화 하여
            //시체를 남겨둘거기 때문에 원래의 숫자가 나온다.
            healTime = armyGroup.GetComponent<Command>().Num;

            //몇마리가 들어와야 하는지 확인
            //처음 1은 부모고, 자기자신도 합쳐 2를 빼준다.
            enterNum = armyArray.Length - 2;
        }
        //처음이 아니면
        else
        {
            //하나 더 들어왔으니 빼주고
            enterNum--;
        }

        if (enterNum == 0)
        {
            StartCoroutine(Healing());
        }


        //여기서 안해주고 ArmyFSM에서 해주면
        //한개가 빠지고 들어오기 때문에
        //armyGroup.GetComponentsInChildren<Transform>();할 때 본인이 빠지고 들어감
        army.gameObject.SetActive(false);
    }

    IEnumerator Healing()
    {
        isHealing = true;

        float persent = 0;
        while(true)
        {
            yield return new WaitForSeconds((healTime / healTime) / 2.0f); // 0.5초에 한번씩 눈으로 보여준다.
            persent += healTime / 2.0f;
            print("+" + healTime / 2.0f);

            if (persent >= 100)
                break;
        }
        Command am = armyGroup.GetComponent<Command>();
        Transform[] p = gameObject.GetComponentsInChildren<Transform>();
        am.HealComeOut((int)healTime, p[2], ref armyArray);
    }

    public void UnColor()
    {
        transform.GetComponentInChildren<MeshRenderer>().material.color = c;
    }
}
