using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public float zoomSpeed = 500.0f;
    private Transform CamCenter;

    // Start is called before the first frame update
    void Start()
    {
        CamCenter = GameObject.Find("CamCenter").GetComponent<Transform>();
        gameObject.GetComponent<Camera>().fieldOfView = 35.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //오론쪽 키를 누른 상태라면
        if(Input.GetMouseButton(1))
        {
            RotationCam();
        }

        //줌 인 아웃
        Zoom();
    }

    private void Zoom()
    {
        //마우스 휠 정보를 가져온다.
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        gameObject.GetComponent<Camera>().fieldOfView += -wheel * zoomSpeed * Time.deltaTime;

        //최소 최대 제한
        if (gameObject.GetComponent<Camera>().fieldOfView < 0.0f/*15.0f*/)
            gameObject.GetComponent<Camera>().fieldOfView = 0.0f/*15.0f*/;
        if (gameObject.GetComponent<Camera>().fieldOfView > 45.0f)
            gameObject.GetComponent<Camera>().fieldOfView = 45.0f;
    }

    //오른쪽 마우스를 누른 상태로 화면 전환
    //CamCenter를 중심으로 중앙을 바라보며 이동함
    private void RotationCam()
    {
        //마우스 이동에 따른변화
        float h = Input.GetAxis("Mouse X");

        //이동부터
        transform.RotateAround(CamCenter.position, Vector3.up * h, 100.0f * Time.deltaTime);
        
    }
}
