using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingSceneClickEvent : MonoBehaviour
{

    private int stage;

    void Start()
    {
        stage = PlayerInfoManager.instance.Stage;

        for (int i = 0; i < stage; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    public void OnClickLevel02()
    {
        if (PlayerInfoManager.instance.Stage == 1)
        {
            //첫번째 맵을 올려준다.
            SceneManager.LoadScene("Level02");
            SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
        }
        else
        {
            //오류메시지
            transform.Find("ErrorMassage").gameObject.SetActive(true);
        }
    }

    public void OnClickLevel03()
    {
        if (PlayerInfoManager.instance.Stage == 2)
        {
            //첫번째 맵을 올려준다.
            SceneManager.LoadScene("Level03");
            SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
        }
        else
        {
            transform.Find("ErrorMassage").gameObject.SetActive(true);
        }
    }

    public void OnClickLevel04()
    {
        if (PlayerInfoManager.instance.Stage == 2)
        {
            //첫번째 맵을 올려준다.
            SceneManager.LoadScene("Level04");
            SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
        }
        else
        {
            transform.Find("ErrorMassage").gameObject.SetActive(true);
        }
    }

    IEnumerator MassageFalse()
    {
        yield return new WaitForSeconds(2.5f);

        transform.Find("ErrorMassage").gameObject.SetActive(false);
    }
}
