using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New item", menuName ="Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public GameObject item3D;
    public float Curacion = 0;
    
    public virtual void Use()
    {
        Debug.Log("Using " + name);

    }
}
