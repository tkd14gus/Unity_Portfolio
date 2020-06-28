﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{

    public static ArmyManager instance = null;

    //병사 팩토리
    public GameObject armyFactory;
    //1마리의 지휘관은 8마리의 병사를 가지고 있다.
    private int maxArmy = 8;

    //지휘관 팩토리
    public GameObject commanderFactory;

    //그룹으로 묶을 오브젝트 팩토리
    public GameObject armyGroupFactory;
    //오브젝트풀링을 위한 큐
    private Queue<GameObject> armyGroupPool;
    //시작은 2마리
    private int maxCommander = 2;

    void Awake()
    {
        if (instance != null) Destroy(this.gameObject);

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        armyGroupPool = new Queue<GameObject>();

        armyGroupPooling();
        
    }

    //지휘관 오브젝트풀링
    private void armyGroupPooling()
    {
        //오브젝트 풀을 위해 지휘관들을 넣어준다.
        for (int i = 0; i < maxCommander; i++)
        {
            GameObject armyGroup = Instantiate(armyGroupFactory);

            GameObject commander = Instantiate(commanderFactory);

            commander.transform.parent = armyGroup.transform;

            commander.SetActive(false);

            //armyGroup 안에 병사들을 넣어준다.
            for (int j = 0; j < maxArmy; j++)
            {
                GameObject army = Instantiate(armyFactory);

                //armyGroup의 자식으로 붙여준다.
                army.transform.parent = armyGroup.transform;

                army.SetActive(false);
            }

            //병사들 컴포넌트
            ArmySpawn ArSp = armyGroup.GetComponent<ArmySpawn>();
            ArSp.ArmyComponerts();
            
            //command에 병사가 몇마리인지(지휘관까지 포함) 저장해둔다.
            armyGroup.GetComponent<Command>().Num = maxArmy + 1;

            armyGroup.SetActive(false);
            armyGroupPool.Enqueue(armyGroup);
        }
    }

    //지휘관 스폰 위치에 스폰
    public void CommanderSpawn(Transform spawnPoint)
    {

        if(armyGroupPool.Count != 0)
        {
            GameObject armyGroup = armyGroupPool.Dequeue();
            //활성화 해준다.
            armyGroup.SetActive(true);
            
            //병사들 스폰
            ArmySpawn ArSp = armyGroup.GetComponent<ArmySpawn>();
            ArSp.AwakeArmy(spawnPoint.position);
        }
        else
        {
            GameObject armyGroup = Instantiate(armyGroupFactory);

            GameObject commander = Instantiate(commanderFactory);

            commander.transform.parent = armyGroup.transform;

            //지휘관 안에 병사들을 넣어준다.
            for (int j = 0; j < maxArmy; j++)
            {
                GameObject army = Instantiate(armyFactory);

                //지휘관의 자식으로 붙여준다.
                army.transform.parent = armyGroup.transform;

                army.SetActive(false);
            }

            //병사들 컴포넌트
            ArmySpawn ArSp = armyGroup.GetComponent<ArmySpawn>();
            ArSp.ArmyComponerts();

            //위치 조정해준다.
            //y값이 0.15만큼 차이가 나서 그 부분 수정해준다.
            //Vector3 po = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            //armyGroup.transform.Translate(po);

            //병사들 스폰
            ArSp.AwakeArmy(spawnPoint.position);
        }
    }

    //숫자로 정해 몇번째 공장을 가동할건지 선택
    public void AddArmy(int num, Transform armyGroup)
    {
        GameObject army = null;
        switch (num)
        {
            //일단 하나밖에 없어서 하나만 케이스에 넣는다.
            case 0:
                army = Instantiate(armyFactory);
                army.transform.position = armyGroup.GetComponentsInChildren<Transform>()[1].position;
                break;
        }

        army.transform.parent = armyGroup.transform;
    }
}