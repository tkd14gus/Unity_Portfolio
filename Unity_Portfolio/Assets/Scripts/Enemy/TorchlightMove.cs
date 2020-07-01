using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchlightMove : MonoBehaviour
{
    //중력에 매 프레임 더해줄 힘
    public float downPower = 4.0f;
    //앞으로 나가는 속도
    public float speed = 2.0f;

    //건물과 거리계산할 때 쓸것
    private Transform target;

    public Transform Target
    {
        set { target = value.GetChild(0); }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //
    //}

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //중력 작용
        downPower -= 0.25f;
        Vector3 g = new Vector3(0, 1 + downPower, 0);

        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position += g * Time.deltaTime;

        StartCoroutine(Fa());

        if (Vector3.Distance(target.position, transform.position) <= 0.8f)
        {
            //바로 사라지게
            downPower = 4.0f;
            StopAllCoroutines();
            gameObject.SetActive(false);
            //그리고 집에 데미지를 준다.
            BrakeHouse bh = target.parent.GetComponent<BrakeHouse>();
            bh.Damaged();
        }
    }

    //안 맞고 쭉 내려가면 강제로 삭제
    IEnumerator Fa()
    {
        yield return new WaitForSeconds(1.5f);
        downPower = 4.0f;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
