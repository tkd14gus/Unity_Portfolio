using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButton : MonoBehaviour
{
    public void OnClickSatrt()
    {
        //첫번째 맵을 올려준다.
        SceneManager.LoadScene("Level01");
        SceneManager.LoadScene("CanvasScene", LoadSceneMode.Additive);
    }

    public void OnClickEscape()
    {
        //종료한다.
        Application.Quit();
    }
}
