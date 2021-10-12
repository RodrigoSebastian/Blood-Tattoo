using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    Text subtitulo;
    Text titulo;
    GameObject play;
    GameObject exit;

    public GameObject loading;
    Image fadeOut;
    // Start is called before the first frame update
    void Start()
    {
        subtitulo = this.transform.Find("Subtitulo").GetComponent<Text>();
        titulo = this.transform.Find("Titulo").GetComponent<Text>();
        play = GameObject.Find("PlayButton");
        exit = GameObject.Find("ExitButton");
        loading = this.transform.Find("Loading").gameObject;
        fadeOut = this.transform.Find("FadeOut").GetComponent<Image>();

        play.transform.GetComponent<Button>().enabled = false;
        exit.transform.GetComponent<Button>().enabled = false;
        StartCoroutine("Blink");
    }
    IEnumerator Blink()
    {
        while (true)
        {
            subtitulo.color = Color.Lerp(Color.clear, Color.black,Mathf.PingPong(Time.time, 1));
            yield return null;
        }
    
    }

    IEnumerator ShowOptions(){
        play.transform.GetComponent<Button>().enabled = true;
        exit.transform.GetComponent<Button>().enabled = true;

        Color actualColor = play.GetComponentInChildren<Text>().color;
        while(true){
            play.GetComponentInChildren<Text>().color = Color.Lerp(actualColor, Color.black,.1f);
            exit.GetComponentInChildren<Text>().color = Color.Lerp(actualColor, Color.black,.05f);
            actualColor = play.GetComponentInChildren<Text>().color;
            yield return null;
        }
    }
    IEnumerator MoveTitle(){
        Vector3 destino = new Vector3(this.transform.position.x, this.transform.position.y + 105,0);
        Vector3 inicio = titulo.transform.position;
        Vector3 distancia = destino - inicio;
        while(distancia.magnitude > .5f){
            inicio = Vector3.Lerp(inicio,destino,.05f);
            titulo.transform.position = inicio;
            distancia = destino - inicio;
            yield return null;
        }
        StartCoroutine(ShowOptions());
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey) {
            subtitulo.color = Color.clear;
            StopCoroutine("Blink");
            StartCoroutine(MoveTitle());
        }
    }

    public void ExitGame(){
        Debug.Log("Exit");
    }

    public void GoToScene(string sceneName){
        loading.SetActive(true);
        fadeOut.gameObject.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName){
        for(float t=0.0f;t<1.0f;){
            fadeOut.color = Color.Lerp(fadeOut.color,Color.black,t);
            t = Mathf.Clamp(t + Time.deltaTime,0.0f,1.0f);
            yield return null;
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while(!asyncOperation.isDone) yield return null;
    }
}
