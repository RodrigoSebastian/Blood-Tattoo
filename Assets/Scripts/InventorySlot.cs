using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    Item item;
    PlayerManager player;

    public void setPlayer(PlayerManager _player){
        player = _player;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        // Debug.Log("Drop " + item.name + " del jugador " + player.name + "ha sido tirado");
        GameObject temp = Instantiate(item.item3D,player.transform.position + player.transform.forward,player.transform.rotation) as GameObject;
        temp.transform.Rotate(-45, 0, 0);
        temp.GetComponent<Rigidbody>().AddForce(player.transform.forward * 200);
        player.inventario.Remove(item);
        StartCoroutine(player.UpdateUI());
    }

    public void UseItem()
    {   
        if(item != null)
        {
            item.Use();
            player.useItem(item);
            player.inventario.Remove(item);
            StartCoroutine(player.UpdateUI());
        }
    }
}
