using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingSceneClickEvent : MonoBehaviour
{
    private AudioSource audio;
    private int stage;
    private Text coin;

    void Start()
    {
        stage = PlayerInfoManager.instance.Stage;

        coin = transform.Find("CoinNum").GetComponent<Text>();

        for (int i = 0; i < stage; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = false;
        }

        ChangeCoin();

        BGMMgr.Instance.PlayBGM("WaitBGM");
    }

    public void OnClickLevel02()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);

        if (PlayerInfoManager.instance.Stage == 1)
        {
            //첫번째 맵을 올려준다.
            SceneManager.LoadScene("Level02");
            SceneManager.LoadScene("WaitingCanvas Scene", LoadSceneMode.Additive);
        }
        else
        {
            //오류메시지
            transform.Find("ErrorMassage").gameObject.SetActive(true);
            StartCoroutine(MassageFalse());
        }
    }

    public void OnClickLevel03()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);

        if (PlayerInfoManager.instance.Stage == 2)
        {
            //첫번째 맵을 올려준다.
            SceneManager.LoadScene("Level03");
            SceneManager.LoadScene("WaitingCanvas Scene", LoadSceneMode.Additive);
        }
        else
        {
            transform.Find("ErrorMassage").gameObject.SetActive(true);
            StartCoroutine(MassageFalse());
        }
    }

    public void OnClickLevel04()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);

        if (PlayerInfoManager.instance.Stage == 3)
        {
            //첫번째 맵을 올려준다.
            SceneManager.LoadScene("Level04");
            SceneManager.LoadScene("WaitingCanvas Scene", LoadSceneMode.Additive);
        }
        else
        {
            transform.Find("ErrorMassage").gameObject.SetActive(true);
            StartCoroutine(MassageFalse());
        }
    }

    public void OnClicUpgrad()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);

        SceneManager.LoadScene("UpgradScene");
    }

    IEnumerator MassageFalse()
    {
        yield return new WaitForSeconds(2.5f);

        transform.Find("ErrorMassage").gameObject.SetActive(false);
    }

    public void ChangeCoin()
    {
        coin.text = PlayerInfoManager.instance.Coin.ToString();
    }
}
