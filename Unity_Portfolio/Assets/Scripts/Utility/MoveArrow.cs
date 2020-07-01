using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private float gravity = -0.05f;
    private float speed = 2.0f;
    private float pwoer;
    public float Pwoer
    {
        set { pwoer = Mathf.Abs(value); }
    }
    //병사의 화살인지 적의 화살인지
    private bool isArmy = true;
    public bool IsArmy
    {
        get { return isArmy; }
        set { isArmy = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //5초 뒤에 Pool로 되돌리는 코루틴.
        StartCoroutine(DeletArrow());

        Move();
        CrashCheck();
    }

    private void Move()
    {
        gravity -= 0.013f;
        Vector3 vt = transform.position;
        vt += transform.forward * speed * Time.deltaTime;
        vt.y += (pwoer + gravity) * Time.deltaTime;
        transform.position = vt;
    }

    private void CrashCheck()
    {
        Collider[] nearObject;

        //병사가 쏜 화살일 때
        if (isArmy)
        {
            nearObject = Physics.OverlapSphere(transform.position, 0.3f, 1 << 9);
            
            if (nearObject.Length != 0)
            {
                if(nearObject[0].name.Contains("Archer"))
                {
                    nearObject[0].GetComponent<ArcherEnemyFSM>().HP -= 20;
                }
                else
                {
                    nearObject[0].GetComponent<EnemyFSM>().HP -= 20;
                }


                StopAllCoroutines();
                ArrowManager.instance.ArrowPool = gameObject;

            }
        }
        //적이 쏜 화살일 때
        else
        {
            nearObject = Physics.OverlapSphere(transform.position, 0.1f, 1 << 8);
            
            if (nearObject.Length != 0)
            {
                nearObject[0].transform.parent.GetComponent<HPManager>().HP -= 20;

                StopAllCoroutines();
                ArrowManager.instance.ArrowPool = gameObject;
            }
        }

        
    }

    IEnumerator DeletArrow()
    {
        yield return new WaitForSeconds(5.0f);
        
        ArrowManager.instance.ArrowPool = gameObject;
    }
}
