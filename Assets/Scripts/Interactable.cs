using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius;
    bool hasInteracted = false;
    PlayerManager _player;
    GameObject jugador;
    private GameObject cubo;
    public bool itemInventory = true;
    public int id;

    public enum TipoDeItem { Inventario, Tablilla,Otro}
    public TipoDeItem tipoDeItem = TipoDeItem.Inventario;

    public virtual void Interact(PlayerManager player)
    {
        // Debug.Log("Interacting " + transform.name + " with: " + player.transform.name);
    }

    public virtual void Start()
    {
        BoxCollider sc = gameObject.AddComponent<BoxCollider>();
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<BoxCollider>().size = new Vector3(radius + 1,radius + 1 ,radius + 1);
    }

    private void Awake() {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            Interact(other.transform.GetComponent<PlayerManager>());
            // Debug.Log("Alguien cerca");
        }
    }
}
