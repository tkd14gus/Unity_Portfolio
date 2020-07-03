using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingSceneClickEvent : MonoBehaviour
{

    public void OnClickLevel02_01()
    {
        //첫번째 맵을 올려준다.
        SceneManager.LoadScene("Level02_01");
        SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
    }

    public void OnClickLevel02_02()
    {
        //첫번째 맵을 올려준다.
        SceneManager.LoadScene("Level02_02");
        SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
    }

    public void OnClickLevel03()
    {
        //첫번째 맵을 올려준다.
        SceneManager.LoadScene("Level03");
        SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
    }
}
