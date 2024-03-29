using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownTower : MonoBehaviour
{
    private Tower thisTower;
    // Start is called before the first frame update
    void Start()
    {
        thisTower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisTower.enemiesUpdate)            //적 리스트가 갱신 되었을 때
        {
            if (thisTower.enemiesinRange.Count > 0)     //범위 안에 적이 들어왔을 때
            {
                foreach(EnemyController enemy in thisTower.enemiesinRange)
                {
                    enemy.SetMode(thisTower.fireRate);          //슬로우 적용
                }
            }
        }
    }
}
