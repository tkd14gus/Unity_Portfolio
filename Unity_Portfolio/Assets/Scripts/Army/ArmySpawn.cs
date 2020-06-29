using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //먼저 지휘관 부터 생성
        army[0].SetActive(true);

        //상태를 IDLE로 바꿔준다.
        ArmyFSM af = army[0].GetComponent<ArmyFSM>();
        af.SpawnState();

        //Y값 조정
        point.y += 0.15f;
        //위치 설정
        army[0].transform.Translate(point);

        StartCoroutine(AwakeArmyCoroutine());
    }

    IEnumerator AwakeArmyCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 1; i < armyNum; i++)
        {
            //깨워주고
            army[i].SetActive(true);

            //상태를 IDLE로 바꿔준다.
            ArmyFSM af = army[i].GetComponent<ArmyFSM>();
            af.SpawnState();

            //먼저 지휘관 근처로 위치조정을 한번 해준다.
            army[i].transform.Translate(army[0].transform.position);

            //위치 조정
            //한 병사마다 45도 각도를 더해서 나온다.
            Quaternion ro = Quaternion.Euler(0, (360 / (armyNum - 1))/*지휘관 제외*/ * i, 0);
            //크기가 작아 그만큼 거리를 작게 계산해준다.
            Vector3 po = ro * (army[0].transform.forward * 0.35f);

            army[i].transform.Translate(po);
    
            yield return new WaitForSeconds(0.1f);
        }
    }
}
