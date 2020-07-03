﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCUIClickEvent : MonoBehaviour
{
    public void OnClickReturn()
    {
        //ESCUI가 실행되면 Time.timeScale = 0이되면서 멈춤
        //그것을 풀어준다.
        Time.timeScale = 1;
        //그리고 ESCUI를 비활성화 해준다.
        gameObject.SetActive(false);
    }

    public void OnClickEscape()
    {
        //종료한다.
        Application.Quit();
    }
}
