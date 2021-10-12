using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnMazeV2 : NetworkBehaviour
{

    
    // b int activados = 0;
    Animator animator;
    [SyncVar (hook = "OnChangeLaver")]
    public bool up =false;

    public GameObject otherLaver;
    public GameObject maze;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnChangeLaver(bool nuevo){
        up = nuevo;
    }

    // Update is called once per frame
    void Update()
    {
        if(up && otherLaver.GetComponent<SpawnMazeV2>().up) maze.GetComponent<Animator>().SetBool("Up",true);
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKeyDown(KeyCode.E) && !up){

                up = !up;
                // activados++;

                animator.SetBool("LeverUp", up);
            }
        }
    }
}
