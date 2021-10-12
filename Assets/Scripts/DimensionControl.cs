using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

public class DimensionControl : MonoBehaviour
{   
    public static Dictionary<string,int> LAYERS = new Dictionary<string, int>();
    public static Dictionary<int,string> _LAYERS = new Dictionary<int,string>();
    public static Dictionary<string,PostProcessVolume> ppVolume = new Dictionary<string, PostProcessVolume>();
    public DimensionManager[] DC;

    void Awake(){
        // StartCoroutine(SetDictionary());
        // StartCoroutine(findObjectsOfType());
        // StartCoroutine(SetPostProcess());
    }

    void Start(){
        StartCoroutine(SetDictionary());
        StartCoroutine(findObjectsOfType());
    }

    public IEnumerator findObjectsOfType(){
        DC = FindObjectsOfType<DimensionManager>();
        yield return null;
    }

    public void SearchDC(PlayerManager player){
        StopCoroutine("Action");
        DC = FindObjectsOfType<DimensionManager>();
        StartCoroutine(Action(player));
    }
    
    IEnumerator iAction(PlayerManager player, GameObject game) {
        game.layer = LAYERS[player.ActualDimension.ToString()];
        for(int i=0;i<game.transform.childCount;i++){
            StartCoroutine(iAction(player,game.transform.GetChild(i).gameObject));
            yield return null;
        }
        yield return null;
    }

    public IEnumerator Action(PlayerManager player){

        foreach (DimensionManager dm in DC)
        {
            try{
                if (dm.Dimensions[player.ActualDimension.ToString()])
                {
                    dm.gameObject.layer = LAYERS[player.ActualDimension.ToString()];
                    player.MainCamera.transform.GetComponent<PostProcessLayer>().volumeLayer = 
                    LayerMask.GetMask(DimensionControl._LAYERS[DimensionControl.LAYERS[player.ActualDimension.ToString()]]);
                }
            }
            catch (MissingReferenceException e){
                Debug.Log("Error " + e);
                SearchDC(player);
            }

            yield return null;
        }
    }

    public IEnumerator SetDictionary(){
        LAYERS.Add("DX",8);
        LAYERS.Add("DZ",9);
        LAYERS.Add("NORMAL",10);
        _LAYERS.Add(8,"DX");
        _LAYERS.Add(9,"DZ");
        _LAYERS.Add(10,"NORMAL");
        yield return null;
    }

    public IEnumerator SetPostProcess(){
        PostProcessVolume[] ppV = FindObjectsOfType<PostProcessVolume>();
        foreach(PostProcessVolume p in ppV){
            ppVolume.Add(p.name,p);
        yield return null;
        }
    }
}