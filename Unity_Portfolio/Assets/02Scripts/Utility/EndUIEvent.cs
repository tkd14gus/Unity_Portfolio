using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUIEvent : MonoBehaviour
{

    //나가기를 눌렀을 떄
    public void OnClickEscape()
    {
        //시간을 원래대로 맞춰준다.
        Time.timeScale = 1;
        //스테이지를 1 올려준다.
        PlayerInfoManager.instance.Stage += 1;
        //나간다.
        SceneManager.LoadScene("WaitingScene");
    }

    public void Calculation(bool result, int _coin, int[] _aIndex, bool[] _isDie)
    {

        //먼저 피로도를 0으로 해준다.
        //먼저 리스트에서 삭제하면 한칸씩 밀려 이상해진다.
        for (int i = 0; i < 4; i++)
        {
            PlayerInfoManager.instance.SetacfFatige(_aIndex[i], 0);
        }

            //죽었으면 list에서 삭제해준다.
            for (int i = 0; i < 4; i++)
        {
            PlayerInfoManager.instance.SetacfFatige(_aIndex[i], 0);
            if (_isDie[i] == false) continue;
            PlayerInfoManager.instance.RemoveCommander(_aIndex[i]);

        }

        //이겼으면 win을, 졌을면 lose를 띄어준다.
        if (result)
            transform.GetChild(1).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(true);


        //코인 몇개 얻었는지
        //2번째 자식의 2번째 자식에게 있다.
        Text num = transform.GetChild(2).GetChild(2).GetComponent<Text>();
        num.text = _coin.ToString();
        PlayerInfoManager.instance.Coin += _coin;
        //text와 이미지 뭉치를 켜준다.
        transform.GetChild(2).gameObject.SetActive(true);

        //나가기 버튼
        transform.GetChild(3).gameObject.SetActive(true);
    }
}
