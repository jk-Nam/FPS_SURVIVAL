using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler ,IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField] private bool isQuickSlot;
    [SerializeField] private int quickSlotNum;

    [SerializeField] private Text text_Count;
    [SerializeField] private GameObject go_CountImage;

    private ItemEffectDB theItemEffectDB;

    [SerializeField] private RectTransform baseRect;
    [SerializeField] RectTransform quickSlotBaseRect;

    private InputNumber theInputNumber;
    private WeaponMgr theWeaponMgr;

    void Start()
    {

        theWeaponMgr = GetComponent<WeaponMgr>();
        theItemEffectDB = FindObjectOfType<ItemEffectDB>();
        theInputNumber = FindObjectOfType<InputNumber>();
    }

    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemTpye != Item.ItemTpye.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        go_CountImage.SetActive(false);
        text_Count.text = "0";
    }

    public int GetQuickSlotNumber()
    {
        return quickSlotNum;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                theItemEffectDB.UseItem(item);
                if (item.itemTpye == Item.ItemTpye.Used)
                    SetSlotCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null && Inventory.inventoryActivated)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin
            && DragSlot.instance.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin
            && DragSlot.instance.transform.localPosition.y < baseRect.rect.yMax)
            ||
            (DragSlot.instance.transform.localPosition.x > quickSlotBaseRect.rect.xMin
            && DragSlot.instance.transform.localPosition.x < quickSlotBaseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y > quickSlotBaseRect.rect.yMin + quickSlotBaseRect.transform.localPosition.y
            && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y < quickSlotBaseRect.rect.yMax + quickSlotBaseRect.transform.localPosition.y)
            ))
        {
            if (DragSlot.instance.dragSlot != null)
            {
                Debug.Log(quickSlotBaseRect.rect.xMin);
                Debug.Log(quickSlotBaseRect.rect.xMax);
                Debug.Log(quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMin);
                Debug.Log(quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMax);
                theInputNumber.Call();
            }
        }

        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();        

            if (isQuickSlot)
            {
                theItemEffectDB.IsActivatedQuickSlot(quickSlotNum);
            }
            else
            {
                if (DragSlot.instance.dragSlot.isQuickSlot)
                {
                    theItemEffectDB.IsActivatedQuickSlot(DragSlot.instance.dragSlot.quickSlotNum);
                }
            }
        }
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)        
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else        
            DragSlot.instance.dragSlot.ClearSlot();        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            theItemEffectDB.ShowToolTip(item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDB.HideToolTip();
    }
}
