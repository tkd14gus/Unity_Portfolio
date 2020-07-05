﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    //부셔야 할 집 위치
    //private Transform[] target;
    private List<Transform> target;
    protected NavMeshAgent agent;
    private CharacterController cc;
    protected Animator anim;
    protected AudioSource audio;

    private int nearTargetIndex = 0;
    private int nearArmyIndex = 0;
    protected Transform targetArmy;
    //횃불
    public GameObject torchlightFactory;
    private GameObject torchlight;

    //공격과 공성을 위한 시간
    protected float curTime = 0.0f;
    private float siegeTime = 2.0f;
    protected float attackTime = 2.5f;

    //공격과 공성 거리
    protected float attackDis = 1.5f;
    private float siegeDis = 1.0f;


    enum EnemyState
    {
        Inship, Move, Attack, Defence, Siege, Damaged, Die
    }

    EnemyState es = EnemyState.Inship;

    private int maxHP;
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    private int hp = 100;
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (maxHP != value)
            {
                if (es == EnemyState.Die)
                    return;

                if (hp <= 0)
                {
                    es = EnemyState.Die;
                    Die();
                    print("All -> 다이");
                    anim.SetTrigger("Die");

                    //사운드
                    audio.clip = FindSoundClip("Die");
                    if (!audio.isPlaying)
                        audio.Play();
                }
                else
                {
                    es = EnemyState.Damaged;
                    Damaged();
                    print("All -> 데미지드");
                    anim.SetTrigger("Damaged");

                    //사운드
                    audio.clip = FindSoundClip("Damaged");
                    if (!audio.isPlaying)
                        audio.Play();
                }
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //동적 할당
        target = new List<Transform>();
        //target에 넣을 트랜스폼 받기
        Transform[] tr = GameObject.Find("House").GetComponentsInChildren<Transform>();
        //target에 넣기
        //0번재 인덱스는 House들을 모으고 있는 오브젝트
        for (int i = 0; i < tr.Length; i++)
        {
            if (tr[i].name.Contains("HouseTarget"))
                target.Add(tr[i]);
        }
        //네비메쉬에이전트 컴포넌트
        agent = GetComponent<NavMeshAgent>();
        //캐릭터컨트롤러 컴포넌트
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();

        //횃불
        torchlight = Instantiate(torchlightFactory);
        torchlight.SetActive(false);


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //타임 스케일에 맞춰 속도 조절
        audio.pitch = Time.timeScale;

        if (hp <= 0)
            es = EnemyState.Die;
        switch (es)
        {
            case EnemyState.Inship:
                Inship();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Defence:
                Defence();
                break;
            case EnemyState.Siege:
                Siege();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        curTime += Time.deltaTime;
    }

    protected IEnumerator ChangeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //걸을때 마다 주변에 가장 가까운 Army찾기
            //주변에 Army가 있는지를 확인할 때 사용하는 변수
            Collider[] nearArmy = Physics.OverlapSphere(transform.position, attackDis, 1 << 8);
            if (nearArmy.Length != 0)
            {
                //가까이에 있는 병사 확인

                targetArmy = nearArmy[0].transform;

                for (int i = 1; i < nearArmy.Length; i++)
                {
                    //만일 targetArmy가 죽어있는 상태면 다음 targetArmy를 받아준다.
                    if (targetArmy.parent.GetComponent<HPManager>().HP <= 0)
                    {
                        targetArmy = nearArmy[i].transform;
                        continue;
                    }

                    if (Vector3.Distance(targetArmy.position, transform.position) > Vector3.Distance(nearArmy[i].transform.position, transform.position))
                    {
                        targetArmy = nearArmy[i].transform;
                        break;
                    }
                }

                //확인 했으면 공격
                //정지하고
                //일단 바로 공격
                //curTime = attackTime;
                //agent.isStopped = true;
                agent.SetDestination(transform.position);
                es = EnemyState.Attack;
                Debug.Log("enemy : ALL -> 어택");
                StopAllCoroutines();
                //아처일 때 코루틴을 한개 더 실행해준다.
                if (transform.name.Contains("Archer"))
                {
                    agent.SetDestination(transform.position);
                    StartCoroutine(MoveTarget());
                }
                //골랐으면 녀석이 죽거나 다른 명령이 내려지기 전까지 바뀌지 않는다.
                
                break;
            }
        }
    }

    private void Inship()
    {
        anim.SetTrigger("InShip");
    }

    private void Move()
    {
        anim.SetTrigger("Move");

        //사운드
        audio.clip = FindSoundClip("Move");
        if (!audio.isPlaying)
            audio.Play();

        //위치까지 이동하고
        agent.SetDestination(target[nearTargetIndex].position);

        //Debug.Log(Vector3.Distance(target[nearTargetIndex].position, transform.position));
        //다 이동했으면 Siege로 바꿔준다.
        if (Vector3.Distance(target[nearTargetIndex].position, transform.position) <= siegeDis)
        {
            //일단 바로 횃불
            curTime = 2.0f;
            agent.SetDestination(transform.position);
            StopAllCoroutines();
            es = EnemyState.Siege;
            Debug.Log("enemy : Move -> Siege");
        }
    }

    private void OnDrawGizmos()
    {
        //공격범위, 공성 범위를 눈으로 보여주자
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDis);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, siegeDis);

        //자꾸 오류나서 주석처리
        //if (target.Count > 0)
        //{
        //    Gizmos.color = Color.black;
        //    Gizmos.DrawWireSphere(target[nearTargetIndex].position, 1.0f);
        //}
    }

    protected virtual void Attack()
    {
        //내가 적을 바라보는 방향
        //적을 내려다보거나 올려보 달때 회전하면서 캐릭터가 함께 움직임.
        //그것을 방지
        Vector3 e = targetArmy.position;
        e.y = 0;
        Vector3 a = transform.position;
        a.y = 0;
        Vector3 dir = (e - a).normalized;
        //시선처리
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = q;

        //병사 비어있다면 Move 상태로 돌아간다.
        if (targetArmy.parent.GetComponent<HPManager>().HP <= 0)
        {
            StartCoroutine(ChangeAttack());
            es = EnemyState.Move;
            anim.SetTrigger("Move");

            //사운드
            audio.clip = FindSoundClip("Move");
            if (!audio.isPlaying)
                audio.Play();

            return;
        }

        //크다면 공격
        if (curTime >= attackTime)
        {
            //속도 조절은 나중에 하자
            agent.SetDestination(targetArmy.position);
            
            if (Vector3.Distance(targetArmy.position, transform.position) <= 0.2)
            {
                anim.SetTrigger("Attack");

                //사운드
                audio.clip = FindSoundClip("Attack");
                if (!audio.isPlaying)
                    audio.Play();

                Debug.Log("Enemy : 공겨어어어억! ");
                
                //공격력 나중에 처리
                targetArmy.parent.GetComponent<HPManager>().HP -= 10;
                curTime = 0.0f;
                //그리고 뒤로 슬금슬금 물러나 준다.
                StartCoroutine(MoveOff());
                return;
            }

        }

        //curTime += Time.deltaTime;
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
            Vector3 vt = transform.forward * -0.2f;
            //이동
            agent.SetDestination(vt);
            anim.SetTrigger("Move");

            //사운드
            audio.clip = FindSoundClip("Move");
            if (!audio.isPlaying)
                audio.Play();

            //0.4초 후
            yield return new WaitForSeconds(0.3f);
            //0.4초동안 움직임을 막아준다.
            agent.SetDestination(transform.position);
            yield return new WaitForSeconds(0.4f);
            i++;

        }
        
    }

    private void Defence()
    {

    }

    private void Siege()
    {
        //curTime += Time.deltaTime;
        if (curTime >= siegeTime)
        {
            anim.SetTrigger("Siege");
            torchlight.SetActive(true);
            torchlight.transform.position = transform.position;
            torchlight.transform.LookAt(target[nearTargetIndex].position);
            curTime = 0.0f;
            StartCoroutine(abc());
        }
    }

    IEnumerator abc()
    {
        yield return new WaitForSeconds(0.5f);

        anim.ResetTrigger("Siege");
    }

    private void Damaged()
    {
        if (hp > 0)
        {
            StartCoroutine(DamagedMotion());
        }
        else
            es = EnemyState.Die;
    }

    IEnumerator DamagedMotion()
    {
        anim.SetTrigger("Damaged");
        //애니메이션 시간.
        yield return new WaitForSeconds(0.3f);
        if (es != EnemyState.Die)
        {
            //아직 배에 타고 있다면 그냥 계속 배에 타고 있어라.
            if (transform.parent != null)
            {
                es = EnemyState.Inship;
                print("데미지드 -> 인쉽");
                anim.SetTrigger("Inship");
            }
            else
            {
                //범위가 맞지 않는 화살공격일 수 있으니 무브로 바꿔준다.
                es = EnemyState.Move;
                //적을 찾는 코루틴도 함께 실행해준다.
                StartCoroutine(ChangeAttack());
                print("데미지드 -> 무브");
                anim.SetTrigger("Move");

                //사운드
                audio.clip = FindSoundClip("Move");
                if (!audio.isPlaying)
                    audio.Play();
            }
        }
    }

    private void Die()
    {
        
        anim.SetTrigger("Die");
        StartCoroutine(DieLater());
    }
    IEnumerator DieLater()
    {
        yield return new WaitForSeconds(0.1f);
        //스크립트 종료시키고, 컬리더와 네비를 종료한다.
        if (transform.name.Contains("Archer"))
        {
            transform.GetComponent<ArcherEnemyFSM>().enabled = false;
        }
        else
        {
            transform.GetComponent<EnemyFSM>().enabled = false;
        }

        cc.enabled = false;
        agent.enabled = false;
    }

    public void CutParent()
    {
        NearTarget();

        StartCoroutine(Disembarkation());
    }

    //가장 가까운 타겟 확인
    private void NearTarget()
    {
        //만일 타겟이 이제 없다면
        if (target.Count == 0)
        {
            //그냥 나간다.
            es = EnemyState.Inship;
            anim.SetTrigger("Inship");
            return;
        }

        //만약 사라진 타겟이 마지막 인덱스였다면
        //인덱스 초과로 오류가 남.
        //때문에 무조건 0으로 초기화 해준다.
        nearTargetIndex = 0;

        //가장 가까운 타겟 인덱스
        for (int i = 0; i < target.Count; i++)
        {
            if (!target[i].gameObject.activeSelf) continue;

            Vector3 standard = target[nearTargetIndex].position - transform.position;
            Vector3 Check = target[i].position - transform.position;

            if (Vector3.SqrMagnitude(standard) > Vector3.SqrMagnitude(Check))
            {
                nearTargetIndex = i;
            }
        }

        TorchlightMove bh = torchlight.GetComponent<TorchlightMove>();
        bh.Target = target[nearTargetIndex];
    }

    IEnumerator Disembarkation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            cc.Move(transform.forward * 1.5f * Time.deltaTime);

            if (DisembarkationF())
                break;
        }
        
        //걸을 수 있는 곳으로 나와야지만
        //부모관계를 끊어준다.
        transform.parent = null;

        //시간마다 Army찾기
        StartCoroutine(ChangeAttack());
    }

    //배에서 내리는 함수
    public bool DisembarkationF()
    {
        RaycastHit[] hitInfo;
        
        hitInfo = Physics.RaycastAll(transform.position, transform.up * -1, 2.0f);
        //바닥이 걸을 수 있는 곳이어야지만 나갈 수 있다.
        if (hitInfo.Length > 0 && hitInfo[0].transform.parent.name.Contains("Grass Block Move"))
        {
            agent.enabled = true;
            //상태를 Move로 바꿔준다. 
            es = EnemyState.Move;
            anim.SetTrigger("Move");

            //사운드
            audio.clip = FindSoundClip("Move");
            if (!audio.isPlaying)
                audio.Play();

            return true;
        }
        
        return false;
    }


    //건물이 무너지면 건물 정보를 삭제한다.
    public void TargetDelet(Transform tr)
    {
        //타겟 삭제
        for (int i = 0; i < target.Count; i++)
        {
            //다르면 통과
            if (target[i] != tr) continue;

            //같다면 삭제
            target.Remove(target[i]);

            //가까운 타겟 리타겟팅
            NearTarget();

            break;
        }

        //만일 타겟이 이제 없다면
        if (target.Count == 0)
        {
            //임시로 Inship로 바꿔주고
            es = EnemyState.Inship;
            anim.SetTrigger("InShip");
            //그냥 나간다.
            return;
        }

        //이동할 수 있게 스탑을 풀어준다.
        agent.isStopped = false;
        //다시 이동
        es = EnemyState.Move;
        anim.SetTrigger("Move");

        //사운드
        audio.clip = FindSoundClip("Move");
        if (!audio.isPlaying)
            audio.Play();

        Debug.Log("시즈 -> 무브");
        //적을 발견하면 싸우도록
        StartCoroutine(ChangeAttack());
    }

    public void StartPushed(Transform lancer)
    {
        Vector3 vt = transform.position;
        vt += lancer.forward * 0.3f; //뒤로 밀리기

        transform.position = vt;
    }

    //랜서 공격에 맞으면 뒤로 밀린다.
    IEnumerator Pushed(Transform lancer)
    {
        //3번에 걸쳐 밀린다.
        for (int i = 0; i < 3; i++)
        {
            Vector3 vt = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            vt += lancer.forward * 2.0f * Time.deltaTime; //뒤로 밀리기

            transform.position = vt;

            yield return new WaitForSeconds(0.01f);
        }
    }

    protected void ChangeMove()
    {
        es = EnemyState.Move;
        anim.SetTrigger("Move");

        //사운드
        audio.clip = FindSoundClip("Move");
        if (!audio.isPlaying)
            audio.Play();

        Debug.Log("어택 -> 무브");
        StartCoroutine(ChangeAttack());
    }

    //아처가 슬금슬금 목적지로 가는 코루틴
    IEnumerator MoveTarget()
    {
        while (true)
        {
            //일단 Siege로 바꿀 수 있는지 확인.
            if (Vector3.Distance(target[nearTargetIndex].position, transform.position) <= siegeDis)
            {
                //일단 바로 횃불
                curTime = 2.0f;
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                es = EnemyState.Siege;
            }

            //2초에 한번씩
            yield return new WaitForSeconds(2.0f);
            //목적지로 조금씩 움직인다.
            agent.SetDestination(target[nearTargetIndex].position);
            anim.SetTrigger("Move");

            //사운드
            audio.clip = FindSoundClip("Move");
            if (!audio.isPlaying)
                audio.Play();

            //0.2초동안
            yield return new WaitForSeconds(0.3f);
            //그 후엔 자기자리
            agent.SetDestination(transform.position);

            if (Vector3.Distance(transform.position, targetArmy.position) < 2.0f)
                break;
        }
    }

    //일단 같은 사운드
    protected AudioClip FindSoundClip(String animState)
    {
        AudioClip Sound = null;
        switch (animState)
        {
            case "Move":
                Sound = (AudioClip)Resources.Load("Sound/" + "Move");
                break;
            case "Attack":
                if (transform.name.Contains("Archer"))
                    Sound = (AudioClip)Resources.Load("Sound/" + "ArcherAttack");
                else
                    Sound = (AudioClip)Resources.Load("Sound/" + "Attack");
                break;
            case "Damaged":
                Sound = (AudioClip)Resources.Load("Sound/" + "Damaged");
                break;
            case "Die":
                Sound = (AudioClip)Resources.Load("Sound/" + "Die");
                break;
            case "Shoot":
                Sound = (AudioClip)Resources.Load("Sound/" + "ArrowShoot");
                break;
        }

        return Sound;
    }
}
