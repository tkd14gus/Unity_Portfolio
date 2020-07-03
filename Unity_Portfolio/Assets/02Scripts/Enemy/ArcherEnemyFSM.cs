using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemyFSM : EnemyFSM
{
    public Transform ShootPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   
        MaxHP = 80;
        HP = 80;
        attackDis = 4.0f;
        attackTime = 4.0f;
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
        //적을 내려다보거나 올려볼 때 회전하면서 캐릭터가 함께 움직임.
        //그것을 방지
        Vector3 e = targetArmy.position;
        e.y = 0;
        Vector3 a = transform.position;
        a.y = 0;
        Vector3 dir = (e - a).normalized;
        //시선처리
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = q;


        //적이 비어있다면 무브 상태로 돌아간다.
        if (targetArmy == null || targetArmy.parent.GetComponent<HPManager>().HP <= 0)
        {
            ChangeMove();
            agent.SetDestination(transform.position);
            return;
        }

        //크다면 공격
        if (curTime >= attackTime)
        {
            //아처는 탐색 범위와 공격 범위가 같다.
            if (Vector3.Distance(targetArmy.position, transform.position) <= attackDis)
            {
                Debug.Log("rhdrur!");
                anim.SetTrigger("Attack");
                //공격력 나중에 처리
                //아처는 화살을 쏘아냄
                ShootArrow();

                curTime = 0.0f;
                return;
            }
            //작다면 다시 목표를 향해 이동
            else
            {
                ChangeMove();
                return;
            }

        }
    }

    private void ShootArrow()
    {
        //높이 던질 힘 계산
        //음수가 나오면 타겟의 위치가 더 높음
        //양수가 나오면 본인의 위치가 더 높음
        //float height = targetArmy.position.y - ShootPos.position.y;

        ShootPos.LookAt(targetArmy);
        GameObject arrow = ArrowManager.instance.ArrowPool;
        arrow.SetActive(true);
        arrow.GetComponent<MoveArrow>().IsArmy = false;
        //이정도 파워가 딱 적당함
        arrow.GetComponent<MoveArrow>().Speed = Vector3.Distance(targetArmy.position, ShootPos.position) / 2.5f;
        arrow.transform.position = ShootPos.position;
        arrow.transform.forward = ShootPos.forward;
        //명중률이 그렇게 좋지는 않음
        //arrow.transform.rotation = Quaternion.Euler(ShootPos.rotation.x + Random.Range(-0.1f, 0.1f),
        //                                            ShootPos.rotation.y + Random.Range(0.0f, 0.1f),
        //                                            ShootPos.rotation.z + Random.Range(-0.1f, 0.1f));
        Vector3 ro = targetArmy.position - ShootPos.position;
        //ro.x += Random.Range(-1.0f, 1.0f);
        //ro.y += Random.Range(5.0f, 10.0f);
        //ro.z += Random.Range(-1.0f, 1.0f);
        arrow.transform.Rotate(ro);
    }
}
