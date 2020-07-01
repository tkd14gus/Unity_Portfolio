using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private float Angle = 45.0f; // ?? 앵글이 45도인데 왜 45도로 셋팅해서 하는지는 모르겠다.
    private float gravity = 0.1f;
    private float speed = 2.0f;
    public float Speed
    {
        set { speed = value + Random.Range(-0.5f, 0.5f); }
    }
    //병사의 화살인지 적의 화살인지
    private bool isArmy = true;
    public bool IsArmy
    {
        get { return isArmy; }
        set
        {
            isArmy = value;
            //누가 쐈는지를 받으면 바로 코루틴 시작
            //충돌 확인용 코루틴 되돌리는 코루틴.
            StartCoroutine(DeletArrow());
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        gravity = -(1.0f * Time.deltaTime * Time.deltaTime / 2.0f);//고정 중력값
    }

    // Update is called once per frame
    void Update()
    {
        

        Move();
        CrashCheck();
    }

    private void Move()
    {
        //Vector3 vt = transform.position;
        //vt += transform.forward * speed * Time.deltaTime;
        //vt.y += (pwoer + gravity) * Time.deltaTime;
        //transform.position = vt;
        //gravity += 0.25f;
        Vector3 v1 = Vector3.zero;
        v1.z = Mathf.Cos(Angle * Mathf.PI / 180.0f) * speed * (Time.deltaTime * 2);//핵심코드
        v1.y = Mathf.Sin(Angle * Mathf.PI / 180.0f) * speed * (Time.deltaTime * 2) + gravity;//핵심코드
        transform.Translate(v1);

        transform.Rotate(new Vector3(Mathf.Cos(Angle * Mathf.PI / 180.0f), 0, 0));//핵심코드
    }

    private void CrashCheck()
    {
        Collider[] nearObject = Physics.OverlapSphere(transform.position, 0.2f);

        if (nearObject.Length != 0)
        {
            //병사가 쏜 화살일 때
            if (isArmy)
            {
                for (int i = 0; i < nearObject.Length; i++)
                {
                    //1 << 9는 적임
                    if (nearObject[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        if (nearObject[i].name.Contains("Archer"))
                        {
                            nearObject[i].GetComponent<ArcherEnemyFSM>().HP -= 20;
                        }
                        else
                        {
                            nearObject[i].GetComponent<EnemyFSM>().HP -= 20;
                        }

                        StopAllCoroutines();
                        ArrowManager.instance.ArrowPool = gameObject;
                    }
                    return;
                }
            }
            //적이 쏜 화살일 때
            else
            {
                for (int i = 0; i < nearObject.Length; i++)
                {
                    //1 << 8은 병사임
                    if (nearObject[i].gameObject.layer == LayerMask.NameToLayer("Army"))
                    {              
                        nearObject[i].transform.parent.GetComponent<HPManager>().HP -= 20;

                        StopAllCoroutines();
                        ArrowManager.instance.ArrowPool = gameObject;
                    }
                    return;
                }
            }
            //무엇과 부딪혔든 코루틴은 멈추고 오브젝트풀로 돌아감
            //StopAllCoroutines();
            //ArrowManager.instance.ArrowPool = gameObject;
            //일단 사물하고 부딫혔으면 2초뒤에 사라짐
            StartCoroutine(DeletArrow());
        }
        
    }

    IEnumerator DeletArrow()
    {
        yield return new WaitForSeconds(2.0f);

        ArrowManager.instance.ArrowPool = gameObject;
    }
}
