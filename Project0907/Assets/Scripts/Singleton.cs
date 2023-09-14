using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance { get; private set; }      //인스턴스를 전역에 선언

    private void Awake()
    {
        if(instance == null)                               //instance 가 NULL일때
        {               
            instance = this;
            DontDestroyOnLoad(gameObject);                      //게임 오브젝트가 Scene이 전환되고 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);                                //1개로 유지시키기 위해 생성된 객체를 파괴한다.
        }
    }

    public int playerScore = 0;                                 //관리할 플레이어 스코어

    public void inscreaseScore(int amount)
    {
        playerScore += amount;                              //함수를 통해서 스코어를 증가시킨다.
    }
}
