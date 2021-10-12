using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SpawnMaze : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject otherPilar;

    [SyncVar(hook = "OnChangeCount")]
    public bool haveKey;

    public GameObject keyPrefab;
    public Animator laberinto;
    void Start()
    {

    }

    public void OnChangeCount(bool nuevo){
        haveKey = nuevo;
    }

    // Update is called once per frame
    void Update()
    {
        if(haveKey && otherPilar.GetComponent<SpawnMaze>().haveKey) laberinto.SetBool("Up",true);

        if(Input.GetKeyDown(KeyCode.M)) laberinto.SetBool("Up",true);
    }
    

    [Command]
    public void CmdSpawnLlave() {

    }

    void OnTriggerStay(Collider other){
        if(!isServer) return;
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Hay un jugador");
            if(Input.GetKeyDown(KeyCode.E) && !haveKey && other.gameObject.GetComponent<PlayerManager>().inventario.haveKey){
                other.gameObject.GetComponent<PlayerManager>().inventario.Remove(other.gameObject.GetComponent<PlayerManager>().inventario.key);

                GameObject go = Instantiate(other.gameObject.GetComponent<PlayerManager>().inventario.key.item3D,
                transform.position + transform.up * 5, transform.rotation, transform.Find("Llave").transform ) as GameObject;
                go.GetComponent<Rigidbody>().isKinematic = true;
                go.transform.localScale = new Vector3(.3f,.3f,.3f);
                
                NetworkServer.Spawn(go);

                Debug.Log("El jugador " + other.gameObject.name +" ha colocado una llave");

                // keyCount++;

                haveKey = true;
            }
        }
    }
}
