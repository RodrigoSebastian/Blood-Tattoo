using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Item item;
    GameObject itemGameObject;
    DimensionControl dimensionControl;
    // NetworkIdentity ni;
    // NetworkTransform nt;


    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        if (player.PickItem && !player.HasItem)
        {
            if(tipoDeItem == TipoDeItem.Inventario){
            PickUp(player);
            StartCoroutine(player.UpdateUI());
            }
            else {
                if(!player.HasItem){
                    // GetComponent<Rigidbody>().isKinematic = true;
                    player.HasItem = true;
                    StartCoroutine(setPos(player));
                    player.haveTablilla = this;
                    // Debug.Log(player.transform.Find("Camera").transform.Find("Mano").transform.position);
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }
    
    public void DropItem(PlayerManager player){
        player.HasItem = false;
        GetComponent<Rigidbody>().AddForce(player.transform.Find("Camera").transform.Find("Mano").forward * 1700);
        GetComponent<Rigidbody>().AddForce(player.transform.Find("Camera").transform.Find("Mano").up * 800);
        GetComponent<Rigidbody>().AddForce(player.transform.Find("Camera").transform.Find("Mano").right * -100);
        GetComponent<BoxCollider>().enabled = true;
        player.haveTablilla = null;
        player.DropItem = false;
    }

    public IEnumerator setPos(PlayerManager player){
        while(player.HasItem){
            transform.position = player.transform.Find("Camera").transform.Find("Mano").transform.position;
            transform.rotation = player.transform.Find("Camera").transform.Find("Mano").transform.rotation;

            if(player.DropItem) DropItem(player);
            yield return null;
        }
    }

    public override void Start()
    {
        base.Start();
        dimensionControl = GameObject.Find("DimensionControl").GetComponent<DimensionControl>(); 
        itemGameObject = this.gameObject;
        if(tipoDeItem == TipoDeItem.Inventario)
            this.tag = "Item";
        else if(tipoDeItem == TipoDeItem.Tablilla)
            this.tag = "ItemTablilla";
        else
            this.tag = "Untagged"; 
            
        // ni = gameObject.AddComponent<NetworkIdentity>();
        // nt = gameObject.AddComponent<NetworkTransform>();
    }

    void PickUp(PlayerManager player)
    {
        bool wasPickedUp = player.inventario.Add(item);
        if(wasPickedUp)
        {
            // Debug.Log (itemGameObject);
            Destroy(itemGameObject);
            dimensionControl.SearchDC(player);
            // NetworkServer.Destroy(itemGameObject);
        }
    }
}
