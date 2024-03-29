using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제너릭 형식으로 Class 생성
public class GenericContainer<T>
{
    private T[] items;                  //커스텀 배열
    private int currentidnex = 0;       //item 지금 번호

    public GenericContainer(int capacity)   //생성될때 배열 갯수 설정
    {
        items = new T[capacity];        //함수를 통해서 받아와서 배열 선언
    }
    public void Add(T item)
    {
        if(currentidnex < items.Length)      
        {
            items[currentidnex] = item;     //받은 아이템을 번호를 통해서 배열에 넣는다
            currentidnex++;                 //아이템 번호를 증가시킨다.
        }
    }

    public T[] Getitems()
    {//아이템 배열을 Return 함
        return items;
    }
}
