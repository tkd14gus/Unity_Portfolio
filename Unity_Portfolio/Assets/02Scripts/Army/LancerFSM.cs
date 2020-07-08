using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerFSM : ArmyFSM
{
    private bool isChangeAttack = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //랜서는 공격속도가 빠르다.
        attackTime = 0.2f;
        attackDis = 2.0f;

        //랜서일 경우 여기서 anim재할당
        anim = transform.GetChild(2).GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        StartCoroutine(ChangeAttack());
        base.Update();
    }

    //이동중일 땐 공격이 안되므로 여기서 따로 확인해준다.
    protected override IEnumerator ChangeAttack()
    {
        //while (true)
        //{
            yield return new WaitForSeconds(0.001f);
            //랜서는 Idle상태여야만 공격할 수 있다.
            //근데 자식 클래스에선 enum문을 가져올 수 없다.
            //부모 클래스에서 확인해주자.
            if (!CheckIdle())
                yield return null;
            
            //주변에 가장 가까운 Enemy찾기
            //주변에 Enemy가 있는지를 확인할 때 사용하는 변수
            Collider[] nearEnemy = Physics.OverlapSphere(transform.position, attackDis, 1 << 9);
            if (nearEnemy.Length != 0)
            {
                //가까이에 있는 적 확인
                targetEnemy = nearEnemy[0].transform;

                for (int i = 0; i < nearEnemy.Length; i++)
                {
                    //만일 nearEnemy가 죽어있는 상태면 다음 nearEnemy를 받아준다.
                    if (targetEnemy.GetComponent<EnemyHPManager>().HP <= 0)
                    {
                        targetEnemy = nearEnemy[i].transform;
                        continue;
                    }
                    if (Vector3.Distance(targetEnemy.position, transform.position) > Vector3.Distance(nearEnemy[i].transform.position, transform.position))
                    {
                        targetEnemy = nearEnemy[i].transform;
                    }
                }
                //확인 했으면 공격
                //정지하고
                //일단 바로 공격
                //curTime = attackTime;
                //agent.isStopped = true;
                ChangeLancerAttack();
                //랜서는 언제나 가장 가까이에 있는 적을 공격해야 한다.
                //골랐으면 녀석이 죽거나 다른 명령이 내려지기 전까지 바뀌지 않는다.
                //break;
        }
        //}
    }

    //공격중일때 뒤로 물러나면 안된다.
    protected override void Attack()
    {
        StartCoroutine(ChangeAttack());
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
        if (targetEnemy.GetComponent<EnemyHPManager>().HP <= 0)
        {
            //NavMeshAgent를 다시 쓸 수 있게 해준다.
            agent.isStopped = false;
            //Idle로 바꿔준다.
            ChangeLAIdle();
            //다시 적 찾기 코루틴
            StartCoroutine(ChangeAttack());
            agent.SetDestination(transform.position);
            anim.SetTrigger("Idle");
            return;
        }

        //만일 배를 타고 있으면
        if (targetEnemy.parent != null)
        {
            anim.SetTrigger("Move");

            //사운드
            audio.clip = FindSoundClip("Move");
            if (!audio.isPlaying)
                audio.Play();

            transform.position += targetEnemy.parent.forward * 0.6f * Time.deltaTime;
            return;
        }
        
        //거리가 짧다면
        if (Vector3.Distance(targetEnemy.position, transform.position) < 0.3f)
        {
            anim.SetTrigger("Move");

            //사운드
            audio.clip = FindSoundClip("Move");
            if (!audio.isPlaying)
                audio.Play();

            transform.position += transform.forward * -0.1f * Time.deltaTime;
            return;
        }

        //transform.rotation = Quaternion.Euler((targetEnemy.position - transform.position).normalized);
        //크다면 공격
        if (curTime >= attackTime)
        {
            //랜서는 적을 향해 다가가지 않는다.

            //랜서는 워리어보다 사정거리가 멀다.
            //대신 가까워도 공격 못한다.
            if (Vector3.Distance(targetEnemy.position, transform.position) <= 0.5f &&
                Vector3.Distance(targetEnemy.position, transform.position) >= 0.3f)
            {
                anim.SetTrigger("Attack");

                //사운드
                audio.clip = FindSoundClip("Attack");
                if (!audio.isPlaying)
                    audio.Play();

                targetEnemy.GetComponent<EnemyHPManager>().HP -= 5;
                targetEnemy.GetComponent<EnemyHPManager>().StartPushed(transform);
                curTime = 0.0f;
                return;
            }

        }
    }

    IEnumerator ExitAttack()
    {
        yield return new WaitForSeconds(0.15f);
        anim.SetTrigger("Idle");
    }
}
