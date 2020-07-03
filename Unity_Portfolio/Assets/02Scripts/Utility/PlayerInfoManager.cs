using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    //싱글톤
    public static PlayerInfoManager instance = null;

    //피로도와 클래스를 묶기위한 구조체
    struct armyClassFatigue
    {
        public int aClass;
        //0이면 싸울 수 없음
        //1이면 싸울 수 있음
        public int aFatige;
    }
    
    //클래스와 피로도 리스트
    private List<armyClassFatigue> acf;
    public int GetacfClass(int index)
    {
        return acf[index].aClass;
    }
    public int GetacfFatige(int index)
    {
        return acf[index].aFatige;
    }
    public void SetacfClass(int index, int _aClass)
    {
        armyClassFatigue a = acf[index];
        a.aClass = _aClass;
        acf[index] = a;
    }
    public void SetacfFatige(int index, int _aFatige)
    {
        armyClassFatigue a = acf[index];
        a.aFatige = _aFatige;
        acf[index] = a;
    }

    private int coin;
    public int Coin
    {
        get { return coin; }
        set { coin = value; }
    }

    //싱글톤 작업 완료
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        acf = new List<armyClassFatigue>();
        for (int i = 0; i < 2; i++)
        {
            armyClassFatigue a;

            //시작은 박스맨
            a.aClass = 0;
            //시작할 때 피로도가 존재
            a.aFatige = 1;

            acf.Add(a);
        }
    }
}
