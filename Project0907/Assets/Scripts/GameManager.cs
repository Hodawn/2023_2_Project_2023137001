using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenricSingleton<GameManager>
{    public int playerScore = 0;                                 //������ �÷��̾� ���ھ�

    public void inscreaseScore(int amount)
    {
        playerScore += amount;                              //�Լ��� ���ؼ� ���ھ ������Ų��.
    }
}
