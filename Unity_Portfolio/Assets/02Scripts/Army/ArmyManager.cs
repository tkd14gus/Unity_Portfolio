using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private int maxCommander;
    public int MaxCommander
    {
        get { return maxCommander; }
    }

    private int[] index;

    private int count;

    public int[] Index
    {
        get { return index; }
        set
        {
            index = value;
            //따로 Commander을 소환하는데 for문이 돌아가지 않음
            //따라서 count로 넣어주는데
            //어차피 index를 받는다는 점에서 count를 0으로 해줘야 하기 때문에
            //같이 넣어준다.
            count = 0;
        }
    }

    void Awake()
    {
        if (instance != null) Destroy(this.gameObject);

        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        index = PlayerInfoManager.instance.ArmyManagerThrowIndex;
        maxCommander = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;

            maxCommander++;
        }

        //병사 그룹 오브젝트풀링
        armyGroupPool = new Queue<GameObject>();
        armyGroupPooling();
    }

    //병사 그룹 오브젝트풀링
    private void armyGroupPooling()
    {
        //쉽게 확인하기 위해서 사용한 것은 -1로 바꿔줘야 하는데
        //원본을 제거하면 복잡해짐
        int[] tempIndex = index;
        //오브젝트 풀을 위해 지휘관들을 넣어준다.
        for (int i = 0; i < maxCommander; i++)
        {
            GameObject armyGroup = Instantiate(armyGroupFactory);

            GameObject commander = Instantiate(commanderFactory);

            for (int j = 0; j < 4; j++)
            {
                if (tempIndex[j] != -1)
                {
                    commander.GetComponent<SelectClass>().ArmyClass = PlayerInfoManager.instance.GetacfClass(tempIndex[j]);
                    Debug.Log("class : " + PlayerInfoManager.instance.GetacfClass(tempIndex[j]));
                    tempIndex[j] = -1;
                    break;
                }
            }


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

            //SelectClass에다가 몇번째 인덱스인지 넣어준다.
            armyGroup.transform.GetChild(0).GetComponent<SelectClass>().Index = index[count];
            //그리고 카운트 증가
            count++;

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

            //SelectClass에다가 몇번째 인덱스인지 넣어준다.
            commander.GetComponent<SelectClass>().Index = index[count];
            //그리고 카운트 증가
            count++;

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
            case 0:
                army = Instantiate(armyFactory);

                //BoxMan빼고 전부 비활성화
                army.GetComponent<ArmyFSM>().enabled = true;
                army.GetComponent<ArcherFSM>().enabled = false;
                army.GetComponent<LancerFSM>().enabled = false;
                army.GetComponent<WorriorFSM>().enabled = false;

                army.transform.Find("BoxMan").gameObject.SetActive(true);
                army.transform.Find("Archer").gameObject.SetActive(false);
                army.transform.Find("Lancer").gameObject.SetActive(false);
                army.transform.Find("Worrior").gameObject.SetActive(false);

                army.GetComponent<ArmyFSM>().HP = army.GetComponent<ArmyFSM>().MaxHP;

                //상태를 Idle로 바꿔주고, anim을 받아준다.
                army.GetComponent<ArmyFSM>().SpawnState();
                army.GetComponent<ArmyFSM>().GetAnim();
                army.transform.position = armyGroup.GetComponentsInChildren<Transform>()[1].position;
                break;
            case 1:
                army = Instantiate(armyFactory);

                //Archer빼고 전부 비활성화
                army.GetComponent<ArmyFSM>().enabled = false;
                army.GetComponent<ArcherFSM>().enabled = true;
                army.GetComponent<LancerFSM>().enabled = false;
                army.GetComponent<WorriorFSM>().enabled = false;

                army.transform.Find("BoxMan").gameObject.SetActive(false);
                army.transform.Find("Archer").gameObject.SetActive(true);
                army.transform.Find("Lancer").gameObject.SetActive(false);
                army.transform.Find("Worrior").gameObject.SetActive(false);

                army.GetComponent<ArcherFSM>().HP = army.GetComponent<ArcherFSM>().MaxHP;

                //상태를 Idle로 바꿔주고, anim을 받아준다.
                army.GetComponent<ArcherFSM>().SpawnState();
                army.GetComponent<ArcherFSM>().GetAnim();
                army.transform.position = armyGroup.GetComponentsInChildren<Transform>()[1].position;
                break;
            case 2:
                army = Instantiate(armyFactory);

                //Archer빼고 전부 비활성화
                army.GetComponent<ArmyFSM>().enabled = false;
                army.GetComponent<ArcherFSM>().enabled = false;
                army.GetComponent<LancerFSM>().enabled = true;
                army.GetComponent<WorriorFSM>().enabled = false;

                army.transform.Find("BoxMan").gameObject.SetActive(false);
                army.transform.Find("Archer").gameObject.SetActive(false);
                army.transform.Find("Lancer").gameObject.SetActive(true);
                army.transform.Find("Worrior").gameObject.SetActive(false);

                army.GetComponent<LancerFSM>().HP = army.GetComponent<LancerFSM>().MaxHP;

                //상태를 Idle로 바꿔주고, anim을 받아준다.
                army.GetComponent<LancerFSM>().SpawnState();
                army.GetComponent<LancerFSM>().GetAnim();
                army.transform.position = armyGroup.GetComponentsInChildren<Transform>()[1].position;
                break;
            case 3:
                army = Instantiate(armyFactory);

                //Archer빼고 전부 비활성화
                army.GetComponent<ArmyFSM>().enabled = false;
                army.GetComponent<ArcherFSM>().enabled = false;
                army.GetComponent<LancerFSM>().enabled = false;
                army.GetComponent<WorriorFSM>().enabled = true;

                army.transform.Find("BoxMan").gameObject.SetActive(false);
                army.transform.Find("Archer").gameObject.SetActive(false);
                army.transform.Find("Lancer").gameObject.SetActive(false);
                army.transform.Find("Worrior").gameObject.SetActive(true);

                army.GetComponent<WorriorFSM>().HP = army.GetComponent<WorriorFSM>().MaxHP;

                //상태를 Idle로 바꿔주고, anim을 받아준다.
                army.GetComponent<WorriorFSM>().SpawnState();
                army.GetComponent<WorriorFSM>().GetAnim();
                army.transform.position = armyGroup.GetComponentsInChildren<Transform>()[1].position;
                break;
        }

        //네비매쉬에이전트도 켜준다.
        army.GetComponent<NavMeshAgent>().enabled = true;
        army.transform.parent = armyGroup.transform.parent;
    }
}
