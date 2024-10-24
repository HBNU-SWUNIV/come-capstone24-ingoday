using Photon.Pun;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScriptAdapter : MonoBehaviourPunCallbacks
{
    /*
    // InMapAction
    private GameObject productionImage;  // ������ ���� ȭ��
    private GetResourceManager getResourceManager;   // �ڿ� ä�� ȭ��
    private Button actionButton;     // �׼� ��ư
    private InventorySlotGroup storageSlotGroup; // â�� �κ��丮

    // PlayerMove
    private RopeManager ropeManager; // ���� ���� ��
    Animator animator;  // ��� �ִϸ��̼�

    // SpyBeaverAction
    private InventorySlotGroup inventorySlotGroup;   // �κ��丮
    private TimerManager timerManager;   // Ÿ�̸�(Ÿ�� �Ǽ� ��)
    private Button buildComunicationButton;  // ž �Ǽ� <-> ��� ��ư
    private Transform cnavasGaugesTransform; // ��� �������� �θ� ��ġ
    private GameWinManager gameWinManager;   // �¸�(Ÿ�� ���� �� �̻� �ʵ忡 ���ÿ� ������ ���)
    private Transform towerParentTransfotm;  // Ÿ���� �θ�
    private GameObject escapePrisonButton;   // ���� Ż�� ��ư

    // SpyBoolManager
    private Button buildTowerButton;
    SpyBeaverAction spyAction;

    // DemolishTower
    private Button demolishTowerButton;  // Ÿ�� ö�� ��ư
    private GetResourceManager getResourceManager;   // Ÿ�� ö�� �� �ڿ� �����ޱ� ����
    private TimerManager timerManager;   // Ÿ�� ö�ſ� ���� �ð� ���� ����

    // PrisonManager
    private RectTransform mapImage;      // Ż�� �� ����ϴ� ����
    private TMP_Text prisonTimerText;    // ���� Ÿ�̸� ǥ���� �ؽ�Ʈ
    private InventorySlotGroup inventorySlotGroup;   // �κ��丮(���� ����)
    private Button escapePrisonButton;   // Ż�� ��ư

    // RopeManager
    private Button throwRopeButton;  // ���� ������ ��ư
    */


    private MapImages mapImages;
    private ItemEquipManager itemEquipManager;
    private PutDownItem putDownItem;
    private ThrowAwayItem throwAwayItem;
    private InventorySlotGroup inventorySlotGroup;
    private GameWinManager gameWinManager;
    private NetworkManager networkManager;
    private SoundEffectManager soundEffectManager;


    void Start()
    {
        /*
        // InMapAction
        productionImage = GameObject.Find("ProductionImage");
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        actionButton = GameObject.Find("ActionButton").GetComponent<Button>();
        storageSlotGroup = GameObject.Find("StorageSlots").GetComponent<InventorySlotGroup>();
        //actionButton.onClick.AddListener(OnClickActionButton);

        // PlayerMove
        animator = GetComponent<Animator>();
        ropeManager = this.transform.GetChild(0).GetComponent<RopeManager>();

        // SpyBeaverAction
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();
        buildComunicationButton = GameObject.Find("BuildTowerButton").GetComponent<Button>();
        cnavasGaugesTransform = GameObject.Find("Gauges").transform;
        gameWinManager = GameObject.Find("GameOverManager").GetComponent<GameWinManager>();
        towerParentTransfotm = GameObject.Find("Towers").transform;
        escapePrisonButton = GameObject.Find("EscapePrisonButton");

        // SpyBoolManager
        spyAction = GetComponent<SpyBeaverAction>();
        buildTowerButton = GameObject.Find("SpyChangeButton").GetComponent<Button>();

        // DemolishTower
        demolishTowerButton = GameObject.Find("DemolishTowerButton").GetComponent<Button>();
        getResourceManager = GameObject.Find("GetResourceBackground").GetComponent<GetResourceManager>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();

        // PrisonManager
        mapImage = GameObject.Find("MapImages").GetComponent<RectTransform>();
        prisonTimerText = GameObject.Find("PrisonTimer").GetComponent<TMP_Text>();
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        escapePrisonButton = GameObject.Find("EscapePrisonButton").GetComponent<Button>();

        // RopeManager
        throwRopeButton = GameObject.Find("ThrowRopeButton").GetComponent<Button>();
        */


        if (!this.gameObject.GetPhotonView().IsMine)
        {
            return;
        }

        mapImages = GameObject.Find("MapImagePieces").GetComponent<MapImages>();
        mapImages.player = this.gameObject;
        itemEquipManager = GameObject.Find("EquipSlots").GetComponent<ItemEquipManager>();
        itemEquipManager.player = this.gameObject;
        putDownItem = GameObject.Find("PutDownSolt").GetComponent<PutDownItem>();
        putDownItem.playerPos = this.gameObject.transform;
        throwAwayItem = GameObject.Find("ThrowAwaySlot").GetComponent<ThrowAwayItem>();
        throwAwayItem.prisonManager = this.gameObject.GetComponent<PrisonManager>();

        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        inventorySlotGroup.spyBoolManager = this.gameObject.GetComponent<SpyBoolManager>();
        gameWinManager = GameObject.Find("GameOverManager").GetComponent<GameWinManager>();
        gameWinManager.spyBoolManager = this.gameObject.GetComponent<SpyBoolManager>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.player = this.gameObject;

        soundEffectManager = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectManager>();
        soundEffectManager.playerAudioSource = GetComponent<AudioSource>();
        soundEffectManager.SetVolume();
    }

    void Update()
    {
        
    }
}
