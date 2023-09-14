using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public Slot parentSlot;

    public void init(int id, Slot slot)
    {
        this.id = id;
        this.parentSlot = slot;
    }
}
