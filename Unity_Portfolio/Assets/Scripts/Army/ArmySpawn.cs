using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmySpawn : MonoBehaviour
{
    //나중에 4마리 더 추가되는 아이템 만들것을 대비한 변수
    public int armyNum = 9;
    //병사 오브젝트
    private GameObject[] army;

    public void ArmyComponerts()
    {
        //c++에선 안됐었는데...
        //좀더 살펴보고 List<>로 바꾸던가 해 봅시다.
        army = new GameObject[armyNum];

        //병사들 오브젝트 컴포넌트
        for (int i = 0; i < armyNum; i++)
        {
            army[i] = transform.GetChild(i).gameObject;
        }
        
    }

    
    //병사들을 깨우는 함수
    public void AwakeArmy(Vector3 point)
    {
        //병사들의 직업 선택
        SelectArmyClass();

        //먼저 지휘관 부터 생성
        army[0].SetActive(true);

        //위치 설정
        army[0].transform.position = point;

        army[0].GetComponent<NavMeshAgent>().enabled = true;

        //상태를 IDLE로 바꿔준다.
        IDLE(0);

        //Y값 조정
        //point.y += 0.1f;

        StartCoroutine(AwakeArmyCoroutine());

    }

    IEnumerator AwakeArmyCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 1; i < armyNum; i++)
        {
            //깨워주고
            army[i].SetActive(true);

            army[i].GetComponent<NavMeshAgent>().enabled = true;

            //상태를 IDLE로 바꿔준다.
            IDLE(i);

            //먼저 지휘관 근처로 위치조정을 한번 해준다.
            army[i].transform.Translate(army[0].transform.position);

            //위치 조정
            //한 병사마다 45도 각도를 더해서 나온다.
            Quaternion ro = Quaternion.Euler(0, (360 / (armyNum - 1))/*지휘관 제외*/ * i, 0);
            //크기가 작아 그만큼 거리를 작게 계산해준다.
            Vector3 po = ro * (army[0].transform.forward * 0.25f);

            army[i].transform.Translate(po);
    
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void IDLE(int index)
    {
        ArmyFSM af;
        switch (army[0].GetComponent<SelectClass>().ArmyClass)
        {
            //박스맨
            case 0:
                af = army[index].GetComponent<ArmyFSM>();
                af.SpawnState();
                break;
            //아처
            case 1:
                af = army[index].GetComponent<ArcherFSM>();
                af.SpawnState();
                break;
            //랜서
            case 2:
                af = army[index].GetComponent<LancerFSM>();
                af.SpawnState();
                break;
            //워리어
            case 3:
                af = army[index].GetComponent<WorriorFSM>();
                af.SpawnState();
                break;
        }
    }

    private void SelectArmyClass()
    {
        switch (army[0].GetComponent<SelectClass>().ArmyClass)
        {
            //박스맨
            case 0:
                BoxMan();
                break;
            //아처
            case 1:
                Archer();
                break;
            //랜서
            case 2:
                Lancer();
                break;
            //워리어
            case 3:
                Worrior();
                break;
        }
    }

    private void BoxMan()
    {
        for (int i = 0; i < army.Length; i++)
        {
            //BoxMan빼고 전부 비활성화
            army[i].GetComponent<ArmyFSM>().enabled = true;
            army[i].GetComponent<ArcherFSM>().enabled = false;
            army[i].GetComponent<LancerFSM>().enabled = false;
            army[i].GetComponent<WorriorFSM>().enabled = false;
            
            army[i].transform.Find("BoxMan").gameObject.SetActive(true);
            army[i].transform.Find("Archer").gameObject.SetActive(false);
            army[i].transform.Find("Lancer").gameObject.SetActive(false);
            army[i].transform.Find("Worrior").gameObject.SetActive(false);
        }
    }

    private void Archer()
    {
        for (int i = 0; i < army.Length; i++)
        {
            //Archer빼고 전부 비활성화
            army[i].GetComponent<ArmyFSM>().enabled = false;
            army[i].GetComponent<ArcherFSM>().enabled = true;
            army[i].GetComponent<LancerFSM>().enabled = false;
            army[i].GetComponent<WorriorFSM>().enabled = false;

            army[i].transform.Find("BoxMan").gameObject.SetActive(false);
            army[i].transform.Find("Archer").gameObject.SetActive(true);
            army[i].transform.Find("Lancer").gameObject.SetActive(false);
            army[i].transform.Find("Worrior").gameObject.SetActive(false);
        }
    }

    private void Lancer()
    {
        for (int i = 0; i < army.Length; i++)
        {
            //Lancer빼고 전부 비활성화
            army[i].GetComponent<ArmyFSM>().enabled = false;
            army[i].GetComponent<ArcherFSM>().enabled = false;
            army[i].GetComponent<LancerFSM>().enabled = true;
            army[i].GetComponent<WorriorFSM>().enabled = false;

            army[i].transform.Find("BoxMan").gameObject.SetActive(false);
            army[i].transform.Find("Archer").gameObject.SetActive(false);
            army[i].transform.Find("Lancer").gameObject.SetActive(true);
            army[i].transform.Find("Worrior").gameObject.SetActive(false);
        }
    }

    private void Worrior()
    {
        for (int i = 0; i < army.Length; i++)
        {
            //Worrior빼고 전부 비활성화
            army[i].GetComponent<ArmyFSM>().enabled = false;
            army[i].GetComponent<ArcherFSM>().enabled = false;
            army[i].GetComponent<LancerFSM>().enabled = false;
            army[i].GetComponent<WorriorFSM>().enabled = true;

            army[i].transform.Find("BoxMan").gameObject.SetActive(false);
            army[i].transform.Find("Archer").gameObject.SetActive(false);
            army[i].transform.Find("Lancer").gameObject.SetActive(false);
            army[i].transform.Find("Worrior").gameObject.SetActive(true);
        }
    }
}
