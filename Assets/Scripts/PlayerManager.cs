using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerManager : NetworkBehaviour
{
    [System.Serializable]
    public class Inventory
    {
        // #region Singleton
        // public static Inventory instance;

        // public void Awake()
        // {
        //     if(instance != null)
        //     {
        //         Debug.Log("Mas de 1 inventario");
        //         return;
        //     }
        //     instance = this;
        // }
        // #endregion 

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;

        public int space = 5;
        public List<Item> items = new List<Item>();
        public bool haveKey = false;
        public Item key;

        public bool Add(Item item)
        {
            if(items.Count >= space)
            {
                Debug.Log("No hay espacio");
                return false;
            }
            else
            {
                items.Add(item);
                if(item.name == "Key"){
                     haveKey = true;
                     key = item;
                }
            }
            
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();

            }
            return true;
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();

            }
        }
    }

    Camera mainCamera;
    public enum Dimension {DX, DZ, NORMAL}
    public enum Player {Player_1, Player_2}
    public Player player;
    public bool SecondaryDimension;
    public bool Lock;
    public Inventory inventario;
    DimensionControl dimensionControl;
    bool change = false;
    bool pickItem = false;
    // // public GameObject invto;
    // [SyncVar (hook = "OnChangeLayer")] public string actualLayer;
    string lastLayer;
    GameObject _canvas;
    KeyCode key = KeyCode.Q;
    Dimension actualDimension = Dimension.NORMAL;
    Dimension secondaryDimension;
    private NetworkInstanceId ni;
    Color defaultColor;
    bool dropItem;
    bool hasItem;
    public ItemPickUp haveTablilla;
    public float vida = 1f;
    Animator animator;
    Rigidbody rBody;
    private int fixes = 5;

    // public void OnChangeLayer(string layer){
    //     if(!isServer) return;
    //     actualLayer = layer;
    //     this.gameObject.layer = DimensionControl.LAYERS[actualLayer];
    // }

    public override void OnStartLocalPlayer() {
        GetComponent<FirstPersonController>().enabled = true;
        transform.Find("Camera").GetComponent<Camera>().enabled = true;
        transform.Find("Camera").GetComponent<AudioListener>().enabled = true;
        // GetComponent<CharacterController>().enabled = true;
        transform.Find("Ethan").Find("EthanBody").GetComponent<SkinnedMeshRenderer>().enabled =false;
        transform.Find("Ethan").Find("EthanGlasses").GetComponent<SkinnedMeshRenderer>().enabled =false;

        Cursor.visible = false;

        GameObject.Find("Canvas").SetActive(true);

        PlayerManager[] actualPlayers = FindObjectsOfType<PlayerManager>();
        if(actualPlayers.Length == 1)
            player = Player.Player_1;
        else
            player = Player.Player_2;
             
        // if(GetComponent<NetworkIdentity>().netId.ToString() ==  "1")
        //     player = Player.Player_1;
        // else
        //     player = Player.Player_2;
    }

    public void Start()
    {
        if(player == Player.Player_1) {
            // key = KeyCode.Q;
            secondaryDimension = Dimension.DX;
            defaultColor = Color.red;
        }
        else if(player == Player.Player_2) {
            // key = KeyCode.q;
            secondaryDimension = Dimension.DZ;
            defaultColor = Color.blue;
        }

        StartCoroutine(SetSlots());

        if(SecondaryDimension) actualDimension = secondaryDimension;

        gameObject.layer = DimensionControl.LAYERS[actualDimension.ToString()];
        dimensionControl = GameObject.Find("DimensionControl").GetComponent<DimensionControl>();
        mainCamera = transform.Find("Camera").GetComponent<Camera>();
        _canvas = GameObject.Find("Canvas");
        _canvas.gameObject.GetComponent<InventoryUI>().setPlayer(this);

        _canvas.transform.Find("Curse").Find("CurseBackground").GetComponent<Image>().enabled = true;
        _canvas.transform.Find("Curse").Find("CurseAmount").GetComponent<Image>().enabled = true;
        _canvas.transform.Find("BV").Find("BarraVida").GetComponent<Image>().enabled = true;
        _canvas.transform.Find("BV").Find("VidaTotal").GetComponent<Image>().enabled = true;

        rBody = GetComponent<Rigidbody>();

        StartCoroutine(dimensionControl.findObjectsOfType());

        animator = transform.Find("Ethan").GetComponent<Animator>();

        GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().enabled = false;

        // mainCamera.transform.GetComponent<PostProcessLayer>().volumeLayer = LayerMask.GetMask(DimensionControl._LAYERS[DimensionControl.LAYERS[secondaryDimension.ToString()]]);

        StartCoroutine(dimensionControl.Action(this));
        StartCoroutine(IniDimension());
        StartCoroutine(maldicion());
    }

    public Animator maaze;
    public void Update(){
        if(!isLocalPlayer) return;
        if(Input.GetKeyDown(key) && !Lock)
            StartCoroutine(CambiarDimension());

        if(Input.GetKeyDown(KeyCode.C)) _canvas.GetComponent<Canvas>().enabled = false;
        if(Input.GetKeyDown(KeyCode.X)) _canvas.GetComponent<Canvas>().enabled = true;
        if(Input.GetKeyDown(KeyCode.M)) maaze.SetBool("Up", true);

        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));

            if (Physics.Raycast(r, out hit) && !hasItem) {
                if(hit.collider.CompareTag("Item")){
                    pickItem = true;
                }
            
                if(hit.collider.CompareTag("ItemTablilla")){
                    pickItem = true;
                }
            }
            if (hasItem) dropItem = true;
        }
        else pickItem = false;
        if(Input.GetKeyDown(KeyCode.I)) {
            _canvas.gameObject.GetComponent<InventoryUI>().SeeUI(this);
        }

        if(Input.GetKeyDown(KeyCode.F)) {
            Damage(.13f);
        }


        // Debug.Log("Tiene item en mano: " + hasItem);
        // Debug.Log("Lanzar: " + dropItem);
        UpdateAnimator();
    }

    public IEnumerator IniDimension(){
        maaze = GameObject.Find("FinalMazeV2").GetComponent<Animator>();
        mainCamera.cullingMask ^= 1 << LayerMask.NameToLayer(actualDimension.ToString());
        // actualLayer = DimensionControl._LAYERS[gameObject.layer];
        if(actualDimension == Dimension.NORMAL) change = false;
        else change = true;
        yield return null;
    }

    public IEnumerator CambiarDimension(){
        if(fixes > 0) {StartCoroutine(dimensionControl.findObjectsOfType()); fixes--;}

        lastLayer = actualDimension.ToString();
        if(!change){
            change = true;
            actualDimension = secondaryDimension;

        } else if(change) {
            change = false;
            actualDimension = Dimension.NORMAL;
        }
        
        gameObject.layer = DimensionControl.LAYERS[actualDimension.ToString()];
        StartCoroutine(dimensionControl.Action(this));
        mainCamera.cullingMask ^= 1 << LayerMask.NameToLayer(actualDimension.ToString());
        mainCamera.cullingMask ^= 1 << LayerMask.NameToLayer(lastLayer);
        // actualLayer = DimensionControl._LAYERS[gameObject.layer];

        yield return null;
    }

    public void Damage(float damage){
        vida -= damage;
        StartCoroutine(actualizarVida());
    }

    public IEnumerator maldicion(){
        while(true) {
            if(actualDimension == secondaryDimension)
                Damage(.001f);

            yield return null;
        }
    }

    public Camera MainCamera{
        get {return mainCamera;}
        set {mainCamera = value;}
    }

    public IEnumerator actualizarVida(){
        Image healthBar = _canvas.transform.Find("BV").Find("VidaTotal").GetComponent<Image>();
        for (float t = 0; t < 1.0f; t += 0.05f)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, vida, t);
            // Debug.Log("Vida Actual: " + vida);
            yield return null;
        }
    }

    public Dimension ActualDimension{get {return actualDimension;}}
    public bool PickItem {
        get {return pickItem;}
        set {pickItem = value;}
    }
    public bool HasItem {
        get {return hasItem;}
        set {hasItem = value;}
    }
    public bool DropItem {
        get {return dropItem;}
        set {dropItem = value;}
    }

// CONTROL DE INVENTARIO

    IEnumerator SetSlots() {
        InventorySlot[] iS = FindObjectsOfType<InventorySlot>();
        foreach(InventorySlot iSS in iS){
            iSS.setPlayer(this);
            yield return null;
        }

        _canvas.transform.Find("Inventory").gameObject.SetActive(false);
        _canvas.GetComponent<Canvas>().enabled = true;
    }

    public IEnumerator UpdateUI() {
        _canvas.GetComponent<InventoryUI>().UpdateUI(this);
        yield return null;
    }

    public void useItem(Item health){
        vida += health.Curacion;
        if(vida > 1) vida=1;
        StartCoroutine(actualizarVida());
    }


// CONTROLADOR DE ANIMACIÓN

    public void UpdateAnimator() {
        float velocidadX = CrossPlatformInputManager.GetAxis("Vertical");
        float velocidadY = CrossPlatformInputManager.GetAxis("Horizontal");

        if(Input.GetKey(KeyCode.LeftShift) && velocidadX > 0)
            animator.SetFloat("VelocidadX",1.5f);
        else animator.SetFloat("VelocidadX",velocidadX);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)){
            animator.SetFloat("Altura",hit.distance);
            animator.SetBool("isGround",isGround(hit));
        }
    }

    public bool isGround(RaycastHit hit) {
        if(hit.distance < 2.2) return true;
        return false;
    }

}