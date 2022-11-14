using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject go_InventoryBase;
    [SerializeField] private GameObject go_SlotsParent;
    [SerializeField] private GameObject go_QuickSlotParent;

    [SerializeField] private QuickSlotCtrl theQuickSlot;

    private Slot[] slots;
    private Slot[] quickSlots;
    private bool isNotPut;

    private int SlotNumber;

    [SerializeField] private Item[] items;

    public Slot[] GetSlots()
    {
        return slots;
    }

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        PutSlot(quickSlots, _item, _count);
        if (!isNotPut)
            theQuickSlot.IsActivatedQuickSlot(SlotNumber);

        if (isNotPut)
            PutSlot(slots, _item, _count);

        if (isNotPut)
            Debug.Log("Äü½½·Ô°ú ÀÎº¥Åä¸®°¡ ²ËÃ¡½À´Ï´Ù.");
    }

    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (Item.ItemTpye.Equipment != _item.itemTpye)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        SlotNumber = i;
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }
}
