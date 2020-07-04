using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderSpawn : MonoBehaviour
{
    //몇개의 군대가 들어왔는지
    public int army = 0;

    //스폰 위치
    Transform[] spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        //위치 할당
        spawnPoint = gameObject.GetComponentsInChildren<Transform>();

        //코루틴을 실행해준다.
        StartCoroutine(Spawn());
        
    }

    //스폰하기 위한 코루틴
    //시작 1초 뒤에 지휘관 소환
    //그리고 1초 후에 병사들 소환
    IEnumerator Spawn()
    {
        //1초 뒤에
        yield return new WaitForSeconds(1.0f);

        //몇개의 집단을 스폰해야 하는지 확인
        army = ArmyManager.instance.MaxCommander;

        //0번째 인덱스에 본인의 transform이 들어감
        //때문에 0을 제외하고 실행
        for (int i = 1; i < army + 1; i++)
        {
            //들어온 군대 수만큼 스폰 포인트에 장군들을 소환해준다.
            ArmyManager.instance.CommanderSpawn(spawnPoint[i]);
        }
    }
}
