using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private float gravity = -4.0f;
    private float speed = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CrashCheck();
        //5초 뒤에 Pool로 되돌리는 코루틴.
        StartCoroutine(DeletArrow());
    }

    private void Move()
    {
        Vector3 vt = transform.position;
        vt += transform.forward * speed * Time.deltaTime;
        vt.y += (gravity + speed) * Time.deltaTime;
        transform.position = vt;
    }

    private void CrashCheck()
    {
        Collider[] nearEnemy = Physics.OverlapSphere(transform.position, 0.3f, 1 << 9);

        if(nearEnemy.Length != 0)
        {
            nearEnemy[0].GetComponent<EnemyFSM>().HP -= 20;
            StopAllCoroutines();
            ArrowManager.instance.ArrowPool = gameObject;
        }
    }

    IEnumerator DeletArrow()
    {
        yield return new WaitForSeconds(5.0f);

        ArrowManager.instance.ArrowPool = gameObject;
    }
}
