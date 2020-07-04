using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradManager : MonoBehaviour
{
    //현재 지휘관이 몇인지.
    private int commanderNum;
    private GameObject ca;
    //누른 인덱스 저장
    private int ClickIndex;
    private Text coin;

    void Start()
    {
        commanderNum = PlayerInfoManager.instance.GetacfCount();

        ca = GameObject.Find("Canvas");

        coin = GameObject.Find("CoinNum").GetComponent<Text>();

        //버튼을 필요한 만큼만 켜준다.
        OpenButton();

        ChangeCoin();

    }

    void Update()
    {
        //왼쪽 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            //버튼을 누른것이 아니라면
            if (EventSystem.current.currentSelectedGameObject == null)
                CloseSlot();
        }
    }

    private void OpenButton()
    {
        //필요한 만큼 켜준다.
        for (int i = 0; i < commanderNum; i++)
        {
            //0은 배경, 1은 뒤로가기, 2는 코인 이미지, 3은 코인 개수 텍스트이다.
            ca.transform.GetChild(i + 4).gameObject.SetActive(true);

            //함께 정보도 넣어준다.
            //텍스트 부분임
            ca.transform.GetChild(i + 4).GetChild(0).GetComponent<Text>().text = aClass(i);
        }
    }

    //들어가야 할 이름을 정해준다.
    private string aClass(int index)
    {
        int aClass = PlayerInfoManager.instance.GetacfClass(index);
        string r = "";
        switch (aClass)
        {
            case 0:
                r = "Boxman";
                break;
            case 1:
                r = "Archer";
                break;
            case 2:
                r = "Lancer";
                break;
            case 3:
                r = "Worrior";
                break;
        }
        return r;
    }

    //돌아가기 버튼을 눌렀을 때
    public void OnClickBack()
    {
        //나간다.
        SceneManager.LoadScene("WaitingScene");
    }

    public void OnClickCommander()
    {
        for (int i = 0; i < commanderNum; i++)
        {
            //뭘 눌렀는지 확인
            if (ca.transform.GetChild(i + 4) == EventSystem.current.currentSelectedGameObject.transform)
            {
                //박스맨만 업그레이드 시킬 수 있다.
                if (ca.transform.GetChild(i + 4).GetChild(0).GetComponent<Text>().text != "Boxman")
                    return;

                ClickIndex = i;

                for (int j = 0; j < 3; j++)
                {
                    //자식 버튼들을 모두 활성화 해준다.
                    ca.transform.GetChild(i + 4).GetChild(j + 1).gameObject.SetActive(true);
                }
                break;
            }
        }
    }

    public void OnClickClass()
    {
        //돈이 없으면 업그레이드를 못한다.
        if (PlayerInfoManager.instance.Coin < 5)
            return;

        for (int i = 0; i < 3; i++)
        {
            //뭘 눌렀는지 확인
            if (ca.transform.GetChild(ClickIndex + 4).GetChild(i + 1) == EventSystem.current.currentSelectedGameObject.transform)
            {
                PlayerInfoManager.instance.Coin -= 5;
                //0은 박스맨 본인다.
                PlayerInfoManager.instance.SetacfClass(ClickIndex, i + 1);
                OpenButton();
                ChangeCoin();
                CloseSlot();
                break;
            }
        }
    }

    private void CloseSlot()
    {
        for (int i = 0; i < 3; i++)
        {
            ca.transform.GetChild(ClickIndex + 4).GetChild(i + 1).gameObject.SetActive(false);
        }
    }

    public void ChangeCoin()
    {
        coin.text = PlayerInfoManager.instance.Coin.ToString();
    }

}
