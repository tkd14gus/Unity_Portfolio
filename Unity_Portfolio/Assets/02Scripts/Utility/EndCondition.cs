
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCondition : MonoBehaviour
{
    List<EnemyHPManager> em;
    List<HPManager> am;
    List<BrakeHouse> bh;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        em = new List<EnemyHPManager>();
        for (int i = 0; i < temp.Length; i++)
        {
            em.Add(temp[i].GetComponent<EnemyHPManager>());
        }

        temp = GameObject.FindGameObjectsWithTag("House");
        bh = new List<BrakeHouse>();
        for (int i = 0; i < temp.Length; i++)
        {
            bh.Add(temp[i].GetComponent<BrakeHouse>());
        }
        //병사는 늦게 소환되니 조금 더 있다 시작한다.
        StartCoroutine(CheckArmy());

    }

    IEnumerator CheckArmy()
    {
        yield return new WaitForSeconds(2.0f);

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        am = new List<HPManager>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (!temp[i].name.Contains("Commander")) continue;

            am.Add(temp[i].GetComponent<HPManager>());
        }

        //여기다 CheckEnd를 시작해줘야 한다.
        StartCoroutine(CheckEnd());
    }

    IEnumerator CheckEnd()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < em.Count; i++)
            {
                if (em[i].HP > 0) break;

                if (i == em.Count - 1)
                    WinEnd();
            }

            for (int i = 0; i < am.Count; i++)
            {
                //하나라도 탈출을 안하고 살아있다면 그냥 브레이크
                if (!am[i].IsEscape && am[i].HP > 0) break;

                if (i == am.Count - 1)
                    DefeatEnd();
            }

            for (int i = 0; i < bh.Count; i++)
            {
                //아직 살아있는 집이 없으면 짐
                if (!bh[i].IsBreak) break;

                if (i == bh.Count - 1)
                    DefeatEnd();
            }
        }
    }

    private void DefeatEnd()
    {
        int[] index = ArmyManager.instance.Index;
        bool[] isDie = { false, false, false, false };
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;
            //죽었으면
            if(am[count].HP <= 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (index[j] != am[i].gameObject.GetComponent<SelectClass>().Index) continue;
                    //죽었다고 표시
                    isDie[j] = false;
                    break;

                }
                count++;
            }
        }

        StopAllCoroutines();
        Debug.Log("짐");
        GameObject.Find("EndUI").GetComponent<EndUIEvent>().Calculation(false, 0, index, isDie);
        //캔버스에 있는 EndUI에다가 판 종료 이미지 출력 요청
    }

    //승리
    private void WinEnd()
    {
        int coin = 0;
        for (int i = 0; i < bh.Count; i++)
        {
            //집이 안 부서졌다면
            if (!bh[i].IsBreak)
            {
                //이름에 big이 들어갔다면
                if (bh[i].gameObject.name.Contains("Big"))
                    coin += 2;
                else
                    coin += 3;
            }
        }
        int[] index = ArmyManager.instance.Index;
        bool[] isDie = { false, false, false, false };
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (index[i] == -1) continue;
            //죽었으면
            if (am[count].HP <= 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (index[j] != am[i].gameObject.GetComponent<SelectClass>().Index) continue;
                    //죽었다고 표시
                    isDie[j] = false;
                    break;

                }
                count++;
            }
        }

        StopAllCoroutines();
        Debug.Log("이김");
        GameObject.Find("EndUI").GetComponent<EndUIEvent>().Calculation(true, coin, index, isDie);
        //캔버스에 있는 EndUI에다가 판 종료 이미지 출력 요청
    }
}
