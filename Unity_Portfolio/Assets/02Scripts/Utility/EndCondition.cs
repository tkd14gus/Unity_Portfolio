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
        Debug.Log(temp.Length);
        em = new List<EnemyHPManager>();
        for (int i = 0; i < temp.Length; i++)
        {
            em.Add(temp[i].GetComponent<EnemyHPManager>());
        }

        temp = GameObject.FindGameObjectsWithTag("House");
        Debug.Log(temp.Length);
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
        Debug.Log(temp.Length);
        am = new List<HPManager>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (!temp[i].name.Contains("Commander")) continue;

            am.Add(temp[i].GetComponent<HPManager>());
            Debug.Log(am[am.Count - 1]);
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
                if (bh[i].IsBreak) break;

                if(i == bh.Count - 1)
                    DefeatEnd();
            }
        }
    }

    private void DefeatEnd()
    {

        StopAllCoroutines();
        Debug.Log("짐");
        //캔버스에 있는 EndUI에다가 판 종료 이미지 출력 요청
    }

    //승리
    private void WinEnd()
    {
        StopAllCoroutines();
        Debug.Log("이김");
        //캔버스에 있는 EndUI에다가 판 종료 이미지 출력 요청
    }
}
