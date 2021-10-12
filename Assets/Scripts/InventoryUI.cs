using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    InventorySlot[] slots;
    PlayerManager _player;

    void Start()
    {
        // _player = GameObject.Find("PruebasLAN").transform.GetComponent<PlayerManager>();
        // _player.inventario.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void setPlayer(PlayerManager player) {
        _player = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeeUI(PlayerManager player){
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            UpdateUI(player);
            if (inventoryUI.activeInHierarchy)
            {
                // PauseGame();
                player.gameObject.GetComponent<FirstPersonController>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // ContinueGame();
                player.gameObject.GetComponent<FirstPersonController>().enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

   public void UpdateUI(PlayerManager _player)
    {
        for (int i = 0; i<slots.Length; i++)
        {
            if (i < _player.inventario.items.Count)
            {
                slots[i].AddItem(_player.inventario.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

}