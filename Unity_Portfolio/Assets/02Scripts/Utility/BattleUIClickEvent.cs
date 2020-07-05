using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIClickEvent : MonoBehaviour
{
    private AudioSource audio;

    //건물 안으로 들어가 치료
    public void OnClickHouse()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);
        //집을 바꾸는 것을 샌택했다고 함수 호출.
        Camera.main.GetComponent<ArmyClick>().IsClickHouse = true;
    }
    //적이 타고 온 배를 타고 탈출
    public void OnClickEscape()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);
        //탈출을 선택했다고 함수 호출.
        Camera.main.GetComponent<ArmyClick>().IsClickEscape = true;
    }
}
