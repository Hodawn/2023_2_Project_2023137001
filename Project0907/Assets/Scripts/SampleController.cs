using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    
    void Start()
    {
        Singleton.instance.inscreaseScore(10);
        GameManager.instance.inscreaseScore(15);
    }

   
}
