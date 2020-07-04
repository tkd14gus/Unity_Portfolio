using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindLastEnemy : MonoBehaviour
{
    private float delayTime = 0.0f;
    private GameObject massage;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //가장 큰 수를 찾아야 함.
            if(transform.GetChild(i).GetComponent<ShipMove>().DelayTime > delayTime )
            {
                delayTime = transform.GetChild(i).GetComponent<ShipMove>().DelayTime;
            }
        }

        massage = GameObject.Find("Massage");
        //Text오브젝트는 꺼주고, 본인은 투명하게
        massage.transform.GetChild(0).gameObject.SetActive(false);
        massage.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        StartCoroutine(SendMassage());
    }

    IEnumerator SendMassage()
    {
        yield return new WaitForSeconds(delayTime);
        //Text오브젝트는 켜주고, 본인은 뚜렷명하게
        massage.transform.GetChild(0).gameObject.SetActive(true);
        massage.transform.GetComponent<Image>().color = new Color(0, 0, 0, 255);

        yield return new WaitForSeconds(1.5f);
        //다시 꺼준다.
        massage.transform.GetChild(0).gameObject.SetActive(false);
        massage.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);

    }
}
