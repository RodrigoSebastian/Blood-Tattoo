using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InitPlayer : NetworkBehaviour
{
    public override void OnStartLocalPlayer(){
        GetComponent<PlayerManager>().enabled = true;
        GameObject.Find("TempCamera").SetActive(false);
        Time.timeScale = 1;
    }
}
