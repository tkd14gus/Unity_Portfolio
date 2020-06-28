using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour
{
    public float speed = 2.0f;

    private Transform[] target;
    private Vector3 point;
    private Vector3 dir;

    private Transform armyGroup = null;
    private Transform[] armyArray;

    private Color c; 

    private bool isAnchorage = false;
    public bool IsAnchorage
    {
        get { return isAnchorage; }
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponentsInChildren<Transform>();
        //타겟의 위치
        point = target[2].position;
        //방향
        dir = (point - transform.position).normalized;

        StartCoroutine(Move());

        c = transform.GetComponentsInChildren<Transform>()[1].GetComponent<MeshRenderer>().material.color;
    }

    IEnumerator Move()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            //도착하지 않았다면 계속 앞으로
            transform.position += dir * speed * Time.deltaTime;

            //원하는 위치에 도착 했는지 확인
            if (Vector3.Distance(transform.position, point) < 0.5f)
                break;

        }

        //했다면 잠심 멈추고
        yield return new WaitForSeconds(0.5f);
        //앞으로 살짝 밀리는 효과
        transform.position += dir * speed * Time.deltaTime * 10.0f;

        //0은 자기자신
        //1은 메쉬
        //2는 포인트 오브젝트
        //3번부터 ship과 관계를 끊어준다.
        for (int i = 3; i < target.Length; i++)
        {
            EnemyFSM ef = target[i].GetComponent<EnemyFSM>();
            ef.CutParent();
        }

        yield return new WaitForSeconds(0.3f);
        isAnchorage = true;
    }

    public void InShip(Transform army)
    {
        //부모(ArmyGroup)을 가져온다.
        armyGroup = army.parent.transform;
        armyArray = armyGroup.GetComponentsInChildren<Transform>();

        army.parent = transform;

        //배 위에 승선할 때 랜덤값을 주어 조금 자유분방하게 위치시켜준다.
        Vector3 vt = new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 0.15f, transform.position.z + Random.Range(-0.1f, 0.1f));

        army.position = vt;

        //본인만 남게 됐으면
        if (armyGroup.GetComponentsInChildren<Transform>().Length == 1)
            //코루틴 시작
            StartCoroutine(BackMove());

    }

    IEnumerator BackMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            transform.position -= dir * speed * Time.deltaTime;
        }
    }

    public void UnColor()
    {
        transform.GetComponentInChildren<MeshRenderer>().material.color = c;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
        //아직 정착이 아닐 때
        if(!isAnchorage)
        {
            if(collision.transform.tag == "Player")
            {
                Vector3 dir = collision.transform.forward * -1;
                dir = collision.transform.position - dir;
                collision.transform.position = dir;
            }
        }
    }
}
