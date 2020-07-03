using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    //화살메니저도 싱글톤으로
    public static ArrowManager instance = null;

    //화살 오브젝트풀
    public GameObject arrowFatory;
    private Queue<GameObject> arrowPool;
    public GameObject ArrowPool
    {
        get
        {
            if(arrowPool.Count != 0)
            {
                return arrowPool.Dequeue();
            }
            else
            {
                GameObject arrow = Instantiate(arrowFatory);
                return arrow;
            }
        }
        set
        {
            value.SetActive(false);
            arrowPool.Enqueue(value);
        }
    }
    //최대 화살 개수
    private int maxArrow = 50;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //큐 안에 넣어준다.
        AddArrow();
    }

    private void AddArrow()
    {
        arrowPool = new Queue<GameObject>();

        for (int i = 0; i < maxArrow; i++)
        {
            GameObject arrow = Instantiate(arrowFatory);
            arrow.SetActive(false);
            arrowPool.Enqueue(arrow);
        }
    }
}
