using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveCode : MonoBehaviour
{
    public int nivelActual = 1;
    public int id;
    bool final = false;
    public static Dictionary<int,Nivel1Script> n1S = new Dictionary<int, Nivel1Script>();
    public bool main = false;
    public GameObject Entrada;
    public GameObject Salida;
    public GameObject NuevoCamino;
    
    void Start()
    {
        if(main)
            StartCoroutine(SetIds());
    }

    public bool pass = false;
    //COMBINACIONES 
    // 1 5
    // 3 9
    // 2 8 6
    void Update()
    {
        switch(nivelActual){
            case 1:
                if( Nivel1Script.actions == 2 )
                    if( n1S[1].up && n1S[5].up) {
                        pass = true;
                        winCode();
                        nivelActual ++;
                    }
                        else StartCoroutine(resetPalancas());
            break;
            case 2:
                if( Nivel1Script.actions == 2 )
                    if( n1S[3].up && n1S[9].up){
                        pass = true;
                        winCode();

                        nivelActual ++;
                    }
                    else StartCoroutine(resetPalancas());
            break;
            case 3:
                if( Nivel1Script.actions == 3 )
                    if( n1S[2].up && n1S[8].up && n1S[6].up){
                        pass = true;
                        winCode();

                        nivelActual ++;
                        final = true;
                    }
                        else StartCoroutine(resetPalancas());
            break;
        }

        if(final) {
            Destroy(Salida);
            NuevoCamino.SetActive(true);
        }
    }

    IEnumerator resetPalancas(){
        Nivel1Script.actions = 0;
        foreach(KeyValuePair<int, Nivel1Script> kvp in n1S )
        {
            kvp.Value.up = false;
            kvp.Value.animator.SetBool("LeverUp", false);
            yield return null;
        }
    }

    public IEnumerator SetIds(){
        Nivel1Script[] _n1S = FindObjectsOfType<Nivel1Script>();
        foreach (Nivel1Script item in _n1S)
        {
            n1S.Add(item.id,item);
            yield return null;
        }
    }


    public void winCode() {
        if(id == nivelActual && pass){
            Debug.Log("Gano");
            gameObject.SetActive(false);
            pass = false;
        }

        // StartCoroutine(resetPalancas());
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player") && !final){
            if(id == 66) {
                Debug.Log("mNo");
                StartCoroutine(blockDimension(other.gameObject.GetComponent<PlayerManager>(),true));
            }
        }
    }

    void OnTriggerExit(Collider other){
         if(other.gameObject.CompareTag("Player") && final){
            if(id == 66) StartCoroutine(blockDimension(other.gameObject.GetComponent<PlayerManager>(),false));
        }
    }

    IEnumerator blockDimension(PlayerManager player, bool bolun){
        player.Lock = bolun;
        Entrada.GetComponent<DimensionManager>().enabled = false;
        Entrada.layer = 0;
        yield return null;
    }
}
