using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel2Script : MonoBehaviour
{
    public static bool Terminado = false;
    public static int puestas = 0;
    public GameObject puerta;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(puestas == 2) {
            Destroy(puerta);
            // Terminado = true;
            // Debug.Log("win");
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if(other.gameObject.GetComponent<PlayerManager>().HasItem){
                if(other.gameObject.GetComponent<PlayerManager>().haveTablilla != null) {
                    ItemPickUp itemPick = other.gameObject.GetComponent<PlayerManager>().haveTablilla;
                    Debug.Log("El usuario tiene la tablilla " + itemPick.name);
                    if(Input.GetKeyDown(KeyCode.E) && itemPick.id == 1) {
                        itemPick.transform.position = transform.position + transform.up * 5;
                        itemPick.transform.rotation = transform.rotation;
                        itemPick.transform.GetComponent<Rigidbody>().isKinematic = true;
                        other.gameObject.GetComponent<PlayerManager>().haveTablilla = null;
                        other.gameObject.GetComponent<PlayerManager>().HasItem = false;

                        puestas++;
                    }
                }
                else Debug.Log("El usuario tiene algo pero no es tablilla");
            }
        }
    }

}
