using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel1Script : MonoBehaviour
{
    public Animator animator;
    public bool up = false;
    public int id;
    public static int actions = 0;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    //COMBINACIONES 
    // 1 5
    // 3 9
    // 2 8 6

    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKeyDown(KeyCode.E) && !up){

                up = !up;
                actions++;

                animator.SetBool("LeverUp", up);
            }
        }
    }
}
