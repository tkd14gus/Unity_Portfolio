using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeHouse : MonoBehaviour
{

    private bool isBreak = false;
    public bool IsBreak
    {
        get { return isBreak; }
    }

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
            EnemyFSM ef;
            for (int i = 0; i < enemy.Length; i++)
            {
                if (enemy[i].activeSelf == false) continue;

                if (enemy[i].name.Contains("Archer"))
                    ef = enemy[i].GetComponent<ArcherEnemyFSM>();
                else
                    ef = enemy[i].GetComponent<EnemyFSM>();
                //삭제 함수 호출
                ef.TargetDelet(transform);
            }
            isBreak = true;
            //색상 변경
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
        }
    }
}
