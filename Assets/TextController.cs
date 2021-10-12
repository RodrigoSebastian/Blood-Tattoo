using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    // Start is called before the first frame updatepublic GameObject PanelTexto;
    public int indice = 0;
    // public IEnumerator UpdateText() {
    //     indice = 0;
    //     while(true){
    //         switch (indice){
    //         case 0:
    //             GetComponentInChildren<Text>().text = "a partir de ahora\nestas maldito";
    //             break;
    //         case 1:
    //             GetComponentInChildren<Text>().text = "tu cuerpo esta presente en\n2 dimensiones";
    //             break;
    //         case 2:
    //             GetComponentInChildren<Text>().text = "presiona 'q' para cambiar\nentre ellas";
    //             break;
    //         }
    //         yield return null;
    //     }
    // }

    // public void Update(){
    //     Debug.Log(indice);
    // }

    public void changeText(){
        indice++;
        // Debug.Log(indice);
    }
    
    public IEnumerator seeText() {
        indice = 0;
        Vector3 newScale = new Vector3(0,0,1);
        for (float t = 0; t < 1.0f; t += 0.05f)
        {
            newScale.x = Mathf.Lerp(newScale.x, 1, t);
            newScale.y = Mathf.Lerp(newScale.y, 1, t);
            transform.localScale = newScale;
            yield return null;
        }
    }

    public IEnumerator hideText() {
        Vector3 newScale = new Vector3(1,1,1);
        indice = 0;
        for (float t = 0; t < 1.0f; t += 0.05f)
        {
            // Debug.Log(newScale);
            newScale.x = Mathf.Lerp(newScale.x, 0, t);
            newScale.y = Mathf.Lerp(newScale.y, 0, t);
            transform.localScale = newScale;
            yield return null;
        }
    }
}
