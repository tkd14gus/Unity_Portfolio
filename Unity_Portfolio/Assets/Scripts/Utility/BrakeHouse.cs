using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeHouse : MonoBehaviour
{
    public int hp = 100;

    private int damage = 10;

    private GameObject[] enemy;


    void Start()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void Damaged()
    {
        hp -= damage;

        if(hp < 0)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                if (enemy[i].activeSelf == false) continue;

                EnemyFSM ef = enemy[i].GetComponent<EnemyFSM>();
                //삭제 함수 호출
                ef.TargetDelet(transform);
            }
            //본인 제거
            gameObject.SetActive(false);
        }
    }
}
