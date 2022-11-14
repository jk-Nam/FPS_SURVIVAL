using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab;
    public GameObject go_PreviewPrefab;
}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI;

    private int tabNum = 0;
    private int page = 1;
    private int selectedSlotNum;
    private Craft[] craft_SelectedTab;

    [SerializeField] private Craft[] craft_fire;
    [SerializeField] private Craft[] craft_build;

    private GameObject go_Preview;
    private GameObject go_Prefab;

    [SerializeField] private Transform tf_Player;

    private RaycastHit hitInfo;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    [SerializeField] private GameObject[] go_Slots;
    [SerializeField] private Image[] image_Slot;
    [SerializeField] private Text[] text_SlotName;
    [SerializeField] private Text[] text_SlotDesc;
 
    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

     void Start()
    {
        tabNum = 0;
        page = 1;
    }

    public void TabSetting(int _tabNum)
    {
        tabNum = _tabNum;
        page = 1;

        switch(tabNum)
        {
            case 0:
                TabSlotSetting(craft_fire);
                break;
            case 1:
                TabSlotSetting(craft_build);
                break;
        }
    }

    private void TabSlotSetting(Craft[] _craft_tab)
    {
        craft_SelectedTab = _craft_tab;

        int startSlotNum = (page - 1) * go_Slots.Length;

        for (int i = startSlotNum; i < craft_SelectedTab.Length; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            Window();

        if (isPreviewActivated)
            PreviewPosUpdate();

        if (Input.GetButtonDown("Fire1"))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPosUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;

                if (Input.GetKeyDown(KeyCode.Q))
                    go_Preview.transform.Rotate(0, -90f, 0);
                else if (Input.GetKeyDown(KeyCode.E))
                    go_Preview.transform.Rotate(0, 90f, 0);

                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
}
