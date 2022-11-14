using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotCtrl : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Image[] img_CoolTime;
    [SerializeField] private Transform tf_parent;

    [SerializeField] private Transform tf_ItemPos;
    public static GameObject go_HandItem;

    [SerializeField] private float coolTime;
    private float currentCoolTime;
    private bool isCoolTime;

    [SerializeField] private float appearTime;
    private float currentAppearTime;
    private bool isAppear;

    private int selectedSlot;
    [SerializeField] private GameObject go_SelectedImage;

    [SerializeField] private WeaponMgr theWeaponMgr;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        anim = GetComponent<Animator>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
        AppearCalc();
    }

    private void AppearCalc()
    {
        if (Inventory.inventoryActivated)
            AppearReset();
        else
        {
            if (isAppear)
            {
                currentAppearTime -= Time.deltaTime;
                if (currentAppearTime <= 0)
                {
                    isAppear = false;
                    anim.SetBool("Appear", isAppear);
                }
            }
        }
    }

    private void AppearReset()
    {
        currentAppearTime = appearTime;
        isAppear = true;
        anim.SetBool("Appear", isAppear);
    }

    private void CoolTimeReset()
    {
        currentCoolTime = coolTime;
        isCoolTime = true;
    }

    private void CoolTimeCalc()
    {
        if (isCoolTime)
        {
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < img_CoolTime.Length; i++)
            {
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime;
            }

            if (currentCoolTime <= 0)
                isCoolTime = false;
        }
    }

    private void TryInputNumber()
    {
        if (!isCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ChangSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                ChangSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                ChangSlot(5);
        }        
    }

    public void IsActivatedQuickSlot(int _num)
    {
        if (selectedSlot == _num)
        {
            Execute();
            return;
        }

        if (DragSlot.instance != null)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Execute();
                    return;
                }
            }
        }
    }


    private void ChangSlot(int _num)
    {
        SelectedSlot(_num);
        Execute();
    }

    private void SelectedSlot(int _num)
    {
        selectedSlot = _num;

        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void Execute()
    {
        CoolTimeReset();
        AppearReset();

        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemTpye == Item.ItemTpye.Equipment)
                StartCoroutine(theWeaponMgr.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            else if (quickSlots[selectedSlot].item.itemTpye == Item.ItemTpye.Used)
                ChangeHand(quickSlots[selectedSlot].item);
            else
                ChangeHand();
        }
        else
            ChangeHand();
    }

    private void ChangeHand(Item _item = null)
    {
        StartCoroutine(theWeaponMgr.ChangeWeaponCoroutine("HAND", "¸Ç¼Õ"));

        if (_item != null)
            StartCoroutine(HandItemCoroutine());
    }

    IEnumerator HandItemCoroutine()
    {
        HandCtrl.isActivate = false;
        yield return new WaitUntil(() => HandCtrl.isActivate);

        go_HandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tf_ItemPos.position, tf_ItemPos.rotation);
        go_HandItem.GetComponent<Rigidbody>().isKinematic = true;
        go_HandItem.GetComponent<BoxCollider>().enabled = false;
        go_HandItem.tag = "Untagged";
        go_HandItem.layer = 9;
        go_HandItem.transform.SetParent(tf_ItemPos);
    }

    public void DecreaseSelectedItem()
    {
        CoolTimeReset();
        AppearReset();
        quickSlots[selectedSlot].SetSlotCount(-1);

        if (quickSlots[selectedSlot].itemCount <= 0)
            Destroy(go_HandItem);
    }

    public bool GetIsCoolTime()
    {
        return isCoolTime;
    }

    public Slot GetSelectedSlot()
    {
        return quickSlots[selectedSlot];
    }
}
