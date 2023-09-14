using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton : MonoBehaviour
{
    private static T _instance;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();               //인스턴스 오브젝트를 찾는다
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();             //생성하고 컨포넌트 붙인다
                }
            }
            return _instance;
        }
    }
        private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance !=null)
        {
            Destroy(gameObject);
        }
    }

    
}
