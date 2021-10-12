using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
    Image cooldown;
	float waitTime = 30.0f;
    float fill = 0.0f;
    void Start(){
        cooldown = this.transform.GetComponent<Image>();
    }
    void Update()
    {
        cooldown.fillAmount = fill;
        fill = Mathf.Lerp(fill,1.0f,.01f);
        Debug.Log(fill);
        if(fill >= .98f) fill = 0.0f;
    }
}
