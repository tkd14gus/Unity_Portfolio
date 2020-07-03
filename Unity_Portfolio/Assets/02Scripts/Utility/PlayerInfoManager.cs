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
        //index가 -1이면 들어가지 않았다는 말이다.
        if (index == -1) return;

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
    //받았다가 바로 armyManager한테 넘겨줄 것
    private int[] armyManagerThrowIndex;
    public int[] ArmyManagerThrowIndex
    {
        get { return armyManagerThrowIndex; }
        set { armyManagerThrowIndex = value; }
    }

    //현재 몇 스테이인지(피로도 대신 사용함)
    private int stage = 0;
    public int Stage
    {
        get { return stage; }
        set { stage = value; }
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
        //처음 시작할 때 지휘관은 둘이다.
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

    public void RemoveCommander(int _index)
    {
        acf.Remove(acf[_index]);
    }

    public void AddCommander(int _aClass, int _aFatige)
    {

        armyClassFatigue a;

        //시작은 박스맨
        a.aClass = _aClass;
        //시작할 때 피로도가 존재
        a.aFatige = _aFatige;

        acf.Add(a);
    }
}
