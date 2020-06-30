using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFSM : ArmyFSM
{
    public Transform ShootPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        attackDis = 3.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    //궁수는 물러서지 않고 제자리에서 계속 발사한다.
    protected override void Attack()
    {
        //내가 적을 바라보는 방향
        //적을 내려다보거나 올려보 달때 회전하면서 캐릭터가 함께 움직임.
        //그것을 방지
        Vector3 e = targetEnemy.position;
        e.y = 0;
        Vector3 a = transform.position;
        a.y = 0;
        Vector3 dir = (e - a).normalized;
        //시선처리
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = q;

        //적이 비어있다면 대기 상태로 돌아간다.
        if (targetEnemy == null || targetEnemy.gameObject.activeSelf == false)
        {
            StartCoroutine(ChangeAttack());
            ChangeLAIdle();
            Debug.Log("어택 -> 대기");
            agent.SetDestination(transform.position);
            return;
        }

        //만일 배를 타고 있다면 뒤로 물러난다.
        if (targetEnemy.parent != null)
        {
            agent.velocity = Vector3.zero;
            transform.position += targetEnemy.parent.forward * 0.6f * Time.deltaTime;
            return;
        }
        
        //크다면 공격
        if (curTime >= attackTime)
        {
            //아처는 탐색 범위와 공격 범위가 같다.

            if (Vector3.Distance(targetEnemy.position, transform.position) <= attackDis)
            {
                Debug.Log("Archer : 공겨어어어억!");
                //공격력 나중에 처리
                //아처는 화살을 쏘아냄
                //targetEnemy.GetComponent<EnemyFSM>().HP -= 20;

                ShootArrow();
                curTime = 0.0f;
                return;
            }

        }

        curTime += Time.deltaTime;
    }

    private void ShootArrow()
    {
        GameObject arrow = ArrowManager.instance.ArrowPool;
        arrow.SetActive(true);
        arrow.transform.position = ShootPos.position;
        arrow.transform.forward = transform.forward;
        //명중률이 그렇게 좋지는 않음
        arrow.transform.rotation = Quaternion.Euler(arrow.transform.rotation.x + Random.Range(-0.1f, 0.1f),
                                                    arrow.transform.rotation.y + Random.Range(0.0f, 0.1f),
                                                    arrow.transform.rotation.z + Random.Range(-0.1f, 0.1f));
    }
}
