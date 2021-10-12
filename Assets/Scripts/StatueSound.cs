using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class StatueSound : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audioStatue;
    Transform playerTransform;
    bool lectura = true;
    public Text[] textos;

    TextController PanelTexto;
    PlayerManager player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && lectura){
            player = other.gameObject.transform.GetComponent<PlayerManager>();
            audioStatue.Play();
            stopPlayer(true);
            Cursor.lockState = CursorLockMode.None;

            StartCoroutine(UpdateText());
            StartCoroutine(PanelTexto.seeText());
        }
    }

    void stopPlayer(bool activado){
        player.transform.GetComponent<FirstPersonController>().enabled = !activado;
        player.transform.GetComponent<PlayerManager>().enabled = !activado;

        Cursor.visible = activado;
    }

    void Start()
    {
        // playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // TextoFlotante.GetComponentInChildren<Text>();
        PanelTexto = GameObject.Find("Canvas").transform.Find("TextoFlotante").transform.Find("PanelTexto").GetComponent<TextController>();
    }

    IEnumerator UpdateText() {
        while(PanelTexto.indice < textos.Length) {
                PanelTexto.transform.GetComponentInChildren<Text>().text = textos[PanelTexto.indice].text;
                Debug.Log(lectura);
                yield return null;
        }

        lectura = false;
        stopPlayer(false);
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(PanelTexto.hideText());
    }
}
