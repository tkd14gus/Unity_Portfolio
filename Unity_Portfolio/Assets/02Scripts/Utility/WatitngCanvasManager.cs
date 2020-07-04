using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WatitngCanvasManager : MonoBehaviour
{
    private Canvas ca;
    //출전할 지휘관의 인덱스
    private int[] index;
    private List<int> tempIndex;
    //무슨 버튼을 눌렀는지 확인
    private GameObject tempBtn;
    //몇번째 슬롯을 바꿔야 하는지 확인
    private int slotNum;
    private int stage;

    // Start is called before the first frame update
    void Start()
    {
        tempIndex = new List<int>();
        index = new int[4];
        stage = PlayerInfoManager.instance.Stage;

        ca = GameObject.Find("WaitingCanvas").GetComponent<Canvas>();

        //PlayerInfoManager에서 정보를 받아와 텍스트를 교체해준다.
        for (int i = 0; i < 4; i++)
        {
            int aClass = PlayerInfoManager.instance.GetacfClass(i);


            switch (aClass)
            {
                case -1:
                    ca.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Empty";
                    index[i] = -1;
                    break;
                case 0:
                    ca.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Boxman";
                    index[i] = i;
                    break;
                case 1:
                    ca.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Archer";
                    index[i] = i;
                    break;
                case 2:
                    ca.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Lancer";
                    index[i] = i;
                    break;
                case 3:
                    ca.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Worrior";
                    index[i] = i;
                    break;
            }


            //스테이지가 3이면 마지막에 지휘관 추가
            //선택 안되도록 해준다.
            if (stage == 2 && i == 3)
            {
                ca.transform.GetChild(i).GetComponent<Button>().interactable = false;
                //텍스트는 무조건 박스맨으로
                ca.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "Boxman";
            }
        }
    }

    void Update()
    {
        //왼쪽 클릭했을 때
        if (Input.GetMouseButtonDown(0))
        {
            //버튼을 누른것이 아니라면
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                CloseSlot();
            }
        }
    }

    public void OnClickSlot01()
    {
        //일단 전부 닫아준다.
        CloseSlot();

        int iCount = PlayerInfoManager.instance.GetacfCount();

        tempIndex.Clear();

        //SubSlot을 몇개 열어야 하는지
        int openSlot = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;

            openSlot++;
        }
        
        openSlot = (iCount - openSlot) + 1;

        if (openSlot != 1)
        {
            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == index[j]) break;
                    if (j == 3)
                        tempIndex.Add(i);
                }
            }
        }
        for (int i = 0; i < openSlot; i++)
        {
            //마지막이면 비우기
            if(i == openSlot - 1)
            {
                ca.transform.GetChild(0).GetChild(i + 1).GetChild(0).GetComponent<Text>().text = "Empty";
                ca.transform.GetChild(0).GetChild(i + 1).gameObject.SetActive(true);
                break;
            }
            //01번 슬롯의 첫번째 슬롯부터
            //(0번은 text이기 때문에 1번부터 슬록이다.)
            ca.transform.GetChild(0).GetChild(i + 1).gameObject.SetActive(true);
            ca.transform.GetChild(0).GetChild(i + 1).GetChild(0).GetComponent<Text>().text
                = aClass(tempIndex[i]);
            Debug.Log("i : " + i + " " + "temp : " + tempIndex[i]);
        }
        slotNum = 0;
    }

    public void OnClickSlot02()
    {
        //일단 전부 닫아준다.
        CloseSlot();

        int iCount = PlayerInfoManager.instance.GetacfCount();
        tempIndex.Clear();
        //SubSlot을 몇개 열어야 하는지
        int openSlot = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;

            openSlot++;
        }

        openSlot = (iCount - openSlot) + 1;
        if (openSlot != 1)
        {
            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == index[j]) break;
                    if (j == 3)
                        tempIndex.Add(i);
                }
            }
        }
        for (int i = 0; i < openSlot; i++)
        {
            //마지막이면 비우기
            if (i == openSlot - 1)
            {
                ca.transform.GetChild(1).GetChild(i + 1).GetChild(0).GetComponent<Text>().text = "Empty";
                ca.transform.GetChild(1).GetChild(i + 1).gameObject.SetActive(true);
                break;
            }
            //01번 슬롯의 첫번째 슬롯부터
            //(0번은 text이기 때문에 1번부터 슬록이다.)
            ca.transform.GetChild(1).GetChild(i + 1).gameObject.SetActive(true);
            ca.transform.GetChild(1).GetChild(i + 1).GetChild(0).GetComponent<Text>().text
                = aClass(tempIndex[i]);
            Debug.Log("i : " + i + " " + "temp : " + tempIndex[i]);
        }
        slotNum = 1;
    }

    public void OnClickSlot03()
    {
        //일단 전부 닫아준다.
        CloseSlot();

        int iCount = PlayerInfoManager.instance.GetacfCount();
        tempIndex.Clear();
        //SubSlot을 몇개 열어야 하는지
        int openSlot = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;

            openSlot++;
        }

        openSlot = (iCount - openSlot) + 1;
        if (openSlot != 1)
        {
            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == index[j]) break;
                    if (j == 3)
                        tempIndex.Add(i);
                }
            }
        }
        for (int i = 0; i < openSlot; i++)
        {
            //마지막이면 비우기
            if (i == openSlot - 1)
            {
                ca.transform.GetChild(2).GetChild(i + 1).GetChild(0).GetComponent<Text>().text = "Empty";
                ca.transform.GetChild(2).GetChild(i + 1).gameObject.SetActive(true);
                break;
            }
            //01번 슬롯의 첫번째 슬롯부터
            //(0번은 text이기 때문에 1번부터 슬록이다.)
            ca.transform.GetChild(2).GetChild(i + 1).gameObject.SetActive(true);
            ca.transform.GetChild(2).GetChild(i + 1).GetChild(0).GetComponent<Text>().text
                = aClass(tempIndex[i]);
            Debug.Log("i : " + i + " " + "temp : " + tempIndex[i]);
        }
        slotNum = 2;
    }

    public void OnClickSlot04()
    {

        //일단 전부 닫아준다.
        CloseSlot();

        int iCount = PlayerInfoManager.instance.GetacfCount();
        tempIndex.Clear();
        //SubSlot을 몇개 열어야 하는지
        int openSlot = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;

            openSlot++;
        }

        openSlot = (iCount - openSlot) + 1;
        if (openSlot != 1)
        {
            for (int i = 0; i < iCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == index[j]) break;
                    if (j == 3)
                        tempIndex.Add(i);
                }
            }
        }
        for (int i = 0; i < openSlot; i++)
        {
            //마지막이면 비우기
            if (i == openSlot - 1)
            {
                ca.transform.GetChild(3).GetChild(i + 1).GetChild(0).GetComponent<Text>().text = "Empty";
                ca.transform.GetChild(3).GetChild(i + 1).gameObject.SetActive(true);
                break;
            }
            //01번 슬롯의 첫번째 슬롯부터
            //(0번은 text이기 때문에 1번부터 슬록이다.)
            ca.transform.GetChild(3).GetChild(i + 1).gameObject.SetActive(true);
            ca.transform.GetChild(3).GetChild(i + 1).GetChild(0).GetComponent<Text>().text
                = aClass(tempIndex[i]);
            Debug.Log("i : " + i + " " + "temp : " + tempIndex[i]);
        }
        slotNum = 3;
    }

    public void OnClickSubSlot()
    {
        int clickNum = 0;
        tempBtn = EventSystem.current.currentSelectedGameObject;
        int i = 0;
        for (i = 0; i < tempIndex.Count; i++)
        {
            if (ca.transform.GetChild(slotNum).GetChild(i + 1).gameObject == tempBtn)
            {
                clickNum = tempIndex[i];
                break;
            }
        }
        Debug.Log("i : " + i + " " + "clickNum : " + clickNum);
        //어떤 번트을 눌렀는지 확인 후
        //일단 텍스트 받아온다.
        string tempCalss = tempBtn.transform.GetChild(0).GetComponent<Text>().text;
        switch (tempCalss)
        {
            case "Empty":
                tempBtn.transform.parent.GetChild(0).GetComponent<Text>().text = "Empty";
                index[slotNum] = -1;
                break;
            case "Boxman":
                tempBtn.transform.parent.GetChild(0).GetComponent<Text>().text = "Boxman";
                index[slotNum] = clickNum;
                break;
            case "Archer":
                tempBtn.transform.parent.GetChild(0).GetComponent<Text>().text = "Archer";
                index[slotNum] = clickNum;
                break;
            case "Lancer":
                tempBtn.transform.parent.GetChild(0).GetComponent<Text>().text = "Lancer";
                index[slotNum] = clickNum;
                break;
            case "Worrior":
                tempBtn.transform.parent.GetChild(0).GetComponent<Text>().text = "Worrior";
                index[slotNum] = clickNum;
                break;
        }
        CloseSlot();
    }

    private void CloseSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            ca.transform.GetChild(slotNum).GetChild(i + 1).gameObject.SetActive(false);
        }
    }

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

    public void OnClickStart()
    {
        //스테이지가 3이면 PlayerInfoManager에도 지휘관 추가
        if (stage == 2)
        {
            //가장 끄트머리
            index[3] = PlayerInfoManager.instance.GetacfCount();
            //0번 병종(Baxman)에 피로도(지금 안쓰임) 0이라는 말이다.
            PlayerInfoManager.instance.AddCommander(0, 0);
            ca.transform.GetChild(3).GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < 4; i++)
        {
            //1개 이상이 출전했다는 것이므로 바로 시작해도 됨
            if (index[i] != -1) break;
            //없다면 시작 안됨
            if (i == 3)
                return;
        }

        //여기다 보내면 판이 시작할 때 ArmyManager로 보내준다.
        PlayerInfoManager.instance.ArmyManagerThrowIndex = index;


        //현재 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);

    }
}
