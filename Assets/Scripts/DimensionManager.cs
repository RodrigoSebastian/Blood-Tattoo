using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DimensionManager : MonoBehaviour
{
    [System.Serializable]
    public class NoVisibleDimension {
        public bool DX;
        public bool DZ;
        public bool NORMAL;
    }

    public Dictionary<string,bool> Dimensions = new Dictionary<string, bool>();
    public NoVisibleDimension noVisibleDimensions;
    
    void Awake(){
        StartCoroutine(SetDictionary());
    }

    public IEnumerator SetDictionary(){

        Dimensions.Add("DX",noVisibleDimensions.DX);
        Dimensions.Add("DZ",noVisibleDimensions.DZ);
        Dimensions.Add("NORMAL",noVisibleDimensions.NORMAL);

        yield return null;
    }
}
