using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyClick : MonoBehaviour
{
    //클릭했을 때 해당 오브젝트
    private GameObject target;
    //지휘관 및 병사 트렌스폼 배열
    //private Transform[] all;
    //지휘관 오브젝트
    private GameObject anamyGroup;
    //배틀 중 필요한 ui
    private GameObject battleUI;

    //지휘관과 병사사 선택되었는지.
    private bool armyClick = false;

    //병사를 클릭하고 난 뒤 등장하는 UI를 사용할 때 필요한 변수
    private List<Transform> houses;
    private List<Transform> ships;

    //Ui중 House를 클릭했을 때
    private bool isClickHouse = false;
    public bool IsClickHouse
    {
        set
        {
            //같으면 오류
            //달라야만 실행
            if(isClickHouse != value)
            {
                isClickHouse = value;
                ClickHouse();
            }
            else
            {
                Debug.Log("House오류");
            }
        }
    }

    //UI중 Escape를 클릭했을 때
    private bool isClickEscape = false;
    public bool IsClickEscape
    {
        set
        {
            //같으면 오류
            //달라야만 실행
            if (isClickEscape != value)
            {
                isClickEscape = value;
                ClickEscape();
            }
            else
            {
                Debug.Log("Escape오류");
            }
        }
    }

    void Start()
    {
        //컴포넌트 해준 후
        battleUI = GameObject.Find("BattleUI");
        //바로 꺼버린다.
        battleUI.SetActive(false);

        //UI사용하기 위한 컴포넌트
        //일단 House관련 오브젝트들을 관리하는 House를 찾은 뒤
        //모든 GameObject들을 가져온다. 
        Transform[] houseArry = GameObject.Find("House").GetComponentsInChildren<Transform>();
        //List동적 할당 후
        houses = new List<Transform>();
        //이름이 HouseTarget인것들만 가져온다.
        for (int i = 0; i < houseArry.Length; i++)
        {
            if (houseArry[i].name.Contains("HouseTarget"))
                houses.Add(houseArry[i]);
        }

        Transform[] shipsArray = GameObject.Find("ShipManager").GetComponentsInChildren<Transform>();
        ships = new List<Transform>();
        //이름이 ShipBody 가져온다.
        for (int i = 0; i < shipsArray.Length; i++)
        {
            if (shipsArray[i].name.Contains("ShipBody"))
                ships.Add(shipsArray[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //왼쪽 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            //레이저
            //마우스로 클릭한 곳
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //레이케스트로 맞은 녀석 정보 담을 변수
            RaycastHit hitInfo;

            //아무것도 안 눌린 상태라면
            if (!armyClick && !isClickHouse)
            {
                //병사를 눌렀을 떄
                //8번째가 Army임
                if (Physics.Raycast(ray, out hitInfo, 100, 1 << 8))
                {
                    Army(hitInfo);
                    armyClick = true;
                    //시간 늦추기
                    SlowTime();
                }
            }
            //병사와 집이 눌린 상태라면
            else if (armyClick && isClickHouse)
            {
                //집을 누른다면
                //11번째가 House임
                if (Physics.Raycast(ray, out hitInfo, 100, 1 << 11))
                {
                    //지휘 스크립트에서 이동하라고 명령내린다.
                    Command co = anamyGroup.GetComponent<Command>();
                    co.HealCommand(hitInfo.transform);

                    //전부 false
                    armyClick = false;
                    isClickHouse = false;
                    isClickEscape = false;
                    //시간 다시 되돌리기
                    ReturnTime();
                    //건물 색깔 빼기
                    UnClickHouse();
                }
            }
            //병사와 탈출이 눌린 상태라면
            else if (armyClick && isClickEscape)
            {
                //배를 누른다면
                //11번째가 Ship임
                if (Physics.Raycast(ray, out hitInfo, 100, 1 << 12))
                {
                    //다시 한번 정박한 배인지 확인
                    if (hitInfo.transform.parent.GetComponent<ShipMove>().IsAnchorage)
                    {
                        //지휘 스크립트에서 이동하라고 명령내린다.
                        Command co = anamyGroup.GetComponent<Command>();
                        co.EscapeCommand(hitInfo.transform.parent);

                        //전부 false
                        armyClick = false;
                        isClickHouse = false;
                        isClickEscape = false;
                        //시간 다시 되돌리기
                        ReturnTime();
                        //배 색깔 빼기
                        UnClickEscape();
                    }
                    
                }
            }

        }
        

        //만일 오른쪽 키가 눌렸을 때
        if(Input.GetMouseButtonDown(1))
        {
            //지휘관과 병사가 눌려있다면
            if(armyClick && !isClickHouse)
            {
                //레이저
                //마우스로 클릭한 곳
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //레이케스트로 맞은 녀석 정보 담을 변수
                RaycastHit hitInfo;
                
                if (Physics.Raycast(ray, out hitInfo, 100, 1 << 10))
                {
                    //지휘관 스크립트에서 이동하라고 명령내린다.
                    Command co = anamyGroup.GetComponent<Command>();
                    co.MoveCommand(hitInfo.transform);
                }
            }
            //그 외에 경우에 오른쪽 키를 누르면 아무일도 안 일어남.
            else
            {

            }

            //일단 오른쪽 키 누르면 false로
            armyClick = false;
            isClickHouse = false;
            isClickEscape = false;
            //시간 다시 되돌리기
            ReturnTime();
            //건물 색깔 빼기
            UnClickHouse();
            //배 색깔 빼기
            UnClickEscape();
        }


    }

    private void ReturnTime()
    {
        //시간 원상복귀
        Time.timeScale = 1.0f;
        //배틀 UI를 재워준다.
        battleUI.SetActive(false);
    }

    private void SlowTime()
    {
        //시간 느리게
        Time.timeScale = 0.1f;
        //배틀 UI를 꺠워준다.
        battleUI.SetActive(true);
    }

    //일단 모두 선택되는거 까지 함
    private void Army(RaycastHit hitInfo)
    {
        //먼저 그룹을 선택해준다.
        anamyGroup = hitInfo.transform.parent.gameObject;
    }

    //모든 house오브젝트들 색깔 바꾸기
    private void ClickHouse()
    {
        for (int i = 0; i < houses.Count; i++)
        {
            //본인이 들고있는 것이 아니라 자식객체가 들고있음
            Transform[] ch = houses[i].GetComponentsInChildren<Transform>();
            //자식의 색을 바꿔준다.
            //본인은 0, 자식은 1번 인덱스
            ch[1].GetComponent<MeshRenderer>().material.color = new Color(0, 255, 0);
        }
    }

    private void UnClickHouse()
    {
        for (int i = 0; i < houses.Count; i++)
        {
            ////본인이 들고있는 것이 아니라 자식객체가 들고있음
            //Transform[] ch = houses[i].GetComponentsInChildren<Transform>();
            ////자식의 색을 바꿔준다.
            ////본인은 0, 자식은 1번 인덱스
            //ch[1].GetComponent<Heal>().UnColor();
            houses[i].GetComponent<Heal>().UnColor();
        }
    }

    private void ClickEscape()
    {
        for (int i = 0; i < ships.Count; i++)
        {
            if (!ships[i].GetComponent<ShipMove>().IsAnchorage) continue;
            //본인이 들고있는 것이 아니라 자식객체가 들고있음
            Transform[] ch = ships[i].GetComponentsInChildren<Transform>();
            //자식의 색을 바꿔준다.
            //본인은 0, 자식은 1번 인덱스
            ch[1].GetComponent<MeshRenderer>().material.color = new Color(0, 0, 255);
        }
    }

    private void UnClickEscape()
    {
        for (int i = 0; i < ships.Count; i++)
        {
            ////이건 정박 한 것이든 아니든 그냥 전부 흰색으로 바꿔준다.
            ////본인이 들고있는 것이 아니라 자식객체가 들고있음
            //Transform[] ch = ships[i].GetComponentsInChildren<Transform>();
            ////자식의 색을 바꿔준다.
            ////본인은 0, 자식은 1번 인덱스
            //ch[1].GetComponent<ShipMove>().UnColor();
            ships[i].GetComponent<ShipMove>().UnColor();
        }
    }
}
