using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorriorFSM : ArmyFSM
{
    //디팬스 상태를 bool문으로 처리해준다.
    //매인 상태가 아니라 서브 상태 개념
    //워리어만 존재
    [SerializeField] private bool isDefence = false;
    private GameObject shild;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        shild = transform.Find("skill01").Find("polySurface29").gameObject;
    }

    protected override void Update()
    {
        base.Update();
        FindArcher();
    }

    private void FindArcher()
    {
        int i = 1;

        Collider[] nearEnemy = Physics.OverlapSphere(transform.position, 2.0f, 1 << 9);
        if (nearEnemy.Length != 0)
        {
            for (i = 0; i < nearEnemy.Length; i++)
            {
                if (nearEnemy[i].name.Contains("EnemyArcher"))
                {
                    isDefence = true;
                    Defence();
                    //디팬스로 바꿔준다.
                    Debug.Log("디팬스 On!");
                    break;
                }
            }
        }

        //주변에 적이 없거나 길이가 주변에 Archer가 없다면 디팬스 off
        if (nearEnemy.Length == 0 || nearEnemy.Length == i)
        {
            isDefence = false;
            Defence();
            //디팬스로 바꿔준다.
            Debug.Log("디팬스 Off!");
        }
    }

    public void Defence()
    {
        shild.SetActive(isDefence);
    }
}
