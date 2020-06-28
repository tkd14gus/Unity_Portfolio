using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmyFSM : MonoBehaviour
{
    private float attackDis = 1.5f;
    private float curTime = 0.0f;
    private float attackTime = 2.5f;

    private NavMeshAgent agent;
    private Transform targetEnemy;

    //움직일 곳
    private Vector3 mPoint;
    public Vector3 MPOINT
    {
        get { return mPoint; }
        set
        {
            mPoint = value;
            ArSt = ArmyState.Move;
            print("All -> 무브");
        }
    }
    //치료하러 가는 곳
    private Transform hPoint;
    public Transform HPOINT
    {
        get { return hPoint; }
        set
        {
            hPoint = value;
            //모든 코루틴(체인지어택만 스탑해도 안됨.) 스탑.
            StopAllCoroutines();
            ArSt = ArmyState.Heal;
            print("All -> 힐");
        }
    }

    //치료하러 가는 곳
    private Transform ePoint;
    public Transform EPOINT
    {
        get { return ePoint; }
        set
        {
            ePoint = value;
            //모든 코루틴(체인지어택만 스탑해도 안됨.) 스탑.
            StopAllCoroutines();
            ArSt = ArmyState.Escape;
            print("All -> 탈출");
        }
    }

    private int hp = 100;
    public int HP
    {
        get { return hp; }
        set
        {
            if (value == 100)
            {
                hp = value;
            }
            else
            {
                if (ArSt == ArmyState.Heal || ArSt == ArmyState.Escape)
                {
                    hp = value;
                    print("Heal || Escape -> 데미지드");
                }
                else
                {
                    hp = value;
                    ArSt = ArmyState.Damaged;
                    print("All -> 데미지드");
                }
            }
        }
    }

    private CharacterController cc;

    enum ArmyState
    {
        Idle, Move, Attack, Damaged, Die, Defence, Heal, Escape
    }

    ArmyState ArSt = ArmyState.Idle;
    
    // Start is called before the first frame update
    void Start()
    {
        //이동속도 랜덤으로 준다.
        //speed = Random.Range(2.3f, 2.8f);
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(ChangeAttack());
    }

    // Update is called once per frame
    void Update()
    {
        switch (ArSt)
        {
            case ArmyState.Idle:
                Idle();
                break;
            case ArmyState.Move:
                Move();
                break;
            case ArmyState.Attack:
                Attack();
                break;
            case ArmyState.Damaged:
                Damaged();
                break;
            case ArmyState.Die:
                Die();
                break;
            case ArmyState.Defence:
                Defence();
                break;
            case ArmyState.Heal:
                Heal();
                break;
            case ArmyState.Escape:
                Escape();
                break;
        }
    }

    IEnumerator ChangeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //주변에 가장 가까운 Enemy찾기
            //주변에 Enemy가 있는지를 확인할 때 사용하는 변수
            Collider[] nearEnemy = Physics.OverlapSphere(transform.position, attackDis, 1 << 9);
            if (nearEnemy.Length != 0)
            {
                //가까이에 있는 병사 확인
                targetEnemy = nearEnemy[0].transform;
                for (int i = 1; i < nearEnemy.Length; i++)
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
                agent.velocity = Vector3.zero;
                ArSt = ArmyState.Attack;
                Debug.Log("All -> 어택");

                //골랐으면 녀석이 죽거나 다른 명령이 내려지기 전까지 바뀌지 않는다.
                break;
            }
        }
    }

    private void Idle()
    {

    }

    private void Move()
    {
        //위치까지 이동하고
        //cc.Move(dir * speed * Time.deltaTime);
        agent.SetDestination(mPoint);


        //다 이동했으면 다시 IDLE로
        if (Vector3.Distance(mPoint, transform.position) <= 0.7f)
            ArSt = ArmyState.Idle;
    }
    
    private void Attack()
    {
        //적이 비어있다면 대기 상태로 돌아간다.
        if(targetEnemy == null || targetEnemy.gameObject.activeSelf == false)
        {
            StartCoroutine(ChangeAttack());
            ArSt = ArmyState.Idle;
            agent.SetDestination(transform.position);
            return;
        }

        //만일 배를 타고 있다면 뒤로 물러난다.
        if(targetEnemy.parent != null)
        {
            agent.velocity = Vector3.zero;
            transform.position += targetEnemy.parent.forward * 0.6f * Time.deltaTime;
            return;
        }

        //transform.rotation = Quaternion.Euler((targetEnemy.position - transform.position).normalized);
        //크다면 공격
        if (curTime >= attackTime)
        {
           //속도 조절은 나중에 하자
           agent.SetDestination(targetEnemy.position);
           
           if (Vector3.Distance(targetEnemy.position, transform.position) <= 0.2)
           {
             Debug.Log("Army : 공겨어어어억!");
             //공격력 나중에 처리
             targetEnemy.GetComponent<EnemyFSM>().HP -= 10;
             curTime = 0.0f;
             return;
           }

        }
        //작다면 슬금슬금 물러선다.
        else
        {
            if (curTime == 0.0f)
            {
                //켜져있을지도 모르니까 꺼주고
                //StopCoroutine(MoveOff());
                //다시 실행
                StartCoroutine(MoveOff());
            }
        }
        
        curTime += Time.deltaTime;
    }

    //물러서는 코루틴
    IEnumerator MoveOff()
    {
        //0.3초 0.4초 있으므로 합이 0.7초임
        //따라서 공격 대기시간인 attackTime을 0.7로 나눈만큼
        //뒤로 조금씩 빠져준다.
        int count = (int)(attackTime / 0.7f);

        for (int i = 0; i < count; i++)
        {
            
            //뒤로 이동을 위한 캐스팅
            Vector3 vt = transform.position - (transform.position - targetEnemy.position).normalized * 0.2f;
            //이동
            agent.SetDestination(vt);
            //0.4초 후
            yield return new WaitForSeconds(0.3f);
            //0.4초동안 움직임을 막아준다.
            agent.isStopped = true;
            yield return new WaitForSeconds(0.4f);
            i++;
            agent.isStopped = false;
        }
        
    }

    private void Damaged()
    {
        if (hp > 0)
        {
            StartCoroutine(DamagedMotion());
        }
        else
            ArSt = ArmyState.Die;
    }

    IEnumerator DamagedMotion()
    {
        //애니메이션 시간.
        yield return new WaitForSeconds(0.3f);

        ArSt = ArmyState.Attack;
        print("데미지드 -> 어택");
    }

    private void Die()
    {
        //일단 간단하게
        print("주금");
        //죽으면 부모에서 떨어트리고
        transform.parent = null;
        //스크립트를 종료시켜준다
        //후에 코루틴 사용하면 코루틴 후에
        gameObject.GetComponent<ArmyFSM>().enabled = false;
        ArSt = ArmyState.Die;
    }

    private void Defence()
    {

    }

    private void Heal()
    {
        agent.SetDestination(hPoint.position);


        //다 이동했으면 일단 비활성화 해준다.
        if (Vector3.Distance(hPoint.position, transform.position) <= 0.5f)
        {
            hPoint.parent.GetComponent<Heal>().ArmyHeal(transform);
        }
    }

    private void Escape()
    {
        agent.SetDestination(ePoint.position);

        //배에 태워준다.
        if (Vector3.Distance(ePoint.position, transform.position) <= 0.7f)
        {
            //충돌처리 발생하지 않도록 취소
            transform.GetComponent<CharacterController>().enabled = false;
            transform.GetComponent<NavMeshAgent>().enabled = false;

            ePoint.GetComponent<ShipMove>().InShip(transform);
            //상태를 Idle로 바꿔준다.(이 함수를 더이상 호출하지 않기 위해서)
            ArSt = ArmyState.Idle;
        }
    }

    public void SpawnState()
    {
        //스폰하면 무조건 상태는 IDLE
        ArSt = ArmyState.Idle;
    }
}
