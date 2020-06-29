using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanccerFSM : ArmyFSM
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //랜서는 공격속도가 빠르다.
        attackTime = 1.5f;
        attackDis = 3.0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    //이동중일 땐 공격이 안되므로 여기서 따로 확인해준다.
    protected override IEnumerator ChangeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            //랜서는 Idle상태여야만 공격할 수 있다.
            //근데 자식 클래스에선 enum문을 가져올 수 없다.
            //부모 클래스에서 확인해주자.
            if (!CheckIdle()) continue;
            
            //주변에 가장 가까운 Enemy찾기
            //주변에 Enemy가 있는지를 확인할 때 사용하는 변수
            Collider[] nearEnemy = Physics.OverlapSphere(transform.position, attackDis, 1 << 9);
            if (nearEnemy.Length != 0)
            {
                //가까이에 있는 적 확인
                targetEnemy = nearEnemy[0].transform;
                for (int i = 0; i < nearEnemy.Length; i++)
                {
                    if (Vector3.Distance(targetEnemy.position, transform.position) > Vector3.Distance(nearEnemy[i].transform.position, transform.position))
                    {
                        targetEnemy = nearEnemy[i].transform;
                    }
                }
                //확인 했으면 공격
                //정지하고
                //일단 바로 공격
                curTime = attackTime;
                //agent.isStopped = true;
                ChangeLancerAttack();
                
                //골랐으면 녀석이 죽거나 다른 명령이 내려지기 전까지 바뀌지 않는다.
                break;
            }
        }
    }

    //공격중일때 뒤로 물러나면 안된다.
    protected override void Attack()
    {
        //Vector3 now = transform.position;
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
            //NavMeshAgent를 다시 쓸 수 있게 해준다.
            agent.isStopped = false;
            //Idle로 바꿔준다.
            ChangeLancerIdle();
            //다시 적 찾기 코루틴
            StartCoroutine(ChangeAttack());
            agent.SetDestination(transform.position);
            return;
        }

        //만일 배를 타고 있으면
        if (targetEnemy.parent != null)
        {
            transform.position += targetEnemy.parent.forward * 0.6f * Time.deltaTime;
            return;
        }
        
        //거리가 짧다면
        if (Vector3.Distance(targetEnemy.position, transform.position) < 1.0f)
        {
            transform.position += transform.forward * -0.5f * Time.deltaTime;
            return;
        }
        //넓다면 움직이지 않음
        else
        {

        }

        //transform.rotation = Quaternion.Euler((targetEnemy.position - transform.position).normalized);
        //크다면 공격
        if (curTime >= attackTime)
        {
            //랜서는 적을 향해 다가가지 않는다.

            //랜서는 워리어보다 사정거리가 멀다.
            //대신 가까워도 공격 못한다.
            if (Vector3.Distance(targetEnemy.position, transform.position) <= 1.5f &&
                Vector3.Distance(targetEnemy.position, transform.position) >= 0.9f)
            {
                Debug.Log("Army : 공겨어어어억!");
                //공격력 나중에 처리
                targetEnemy.GetComponent<EnemyFSM>().HP -= 10;
                targetEnemy.GetComponent<EnemyFSM>().StartPushed(transform);
                curTime = 0.0f;
                return;
            }

        }

        curTime += Time.deltaTime;
    }
}
