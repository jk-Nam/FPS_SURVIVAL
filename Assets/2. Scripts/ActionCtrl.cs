using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionCtrl : MonoBehaviour
{
    [SerializeField] private float range;

    private bool pickupAtivated = false;
    private bool dissolveActivated = false;
    private bool isDissolving = false;

    private bool fireLookActivated = false;

    private RaycastHit hitInfo;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Text actionText;

    [SerializeField] private Inventory theInventory;
    [SerializeField] private WeaponMgr theWeaponMgr;
    [SerializeField] private QuickSlotCtrl theQuickSlot;

    [SerializeField] private Transform tf_MeatDissolveTool;

    [SerializeField] string sound_meat;

    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckAction();
            CanPickUp();
            CanMeat();
            CanDropFire();
        }
    }

    private void CanMeat()
    {
        if (dissolveActivated)
        {
            if ((hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal") && hitInfo.transform.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;
                InfoDisappear();
                StartCoroutine(MeatCoroutine());
            }
                
        }
    }

    private void CanDropFire()
    {
        if (fireLookActivated)
        {
            if (hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetisFire())
            {
                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();
                if (_selectedSlot.item != null)
                {
                    DropAnItem(_selectedSlot);
                }
            }
        }
    }

    private void DropAnItem(Slot _selectedSlot)
    {
        switch(_selectedSlot.item.itemTpye)
        {
            case Item.ItemTpye.Used:
                if (_selectedSlot.item.itemName.Contains("고기"))
                {
                    Instantiate(_selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);
                    theQuickSlot.DecreaseSelectedItem();
                }
                break;
            case Item.ItemTpye.Ingredient:
                break;
        }
    }

    IEnumerator MeatCoroutine()
    {
        WeaponMgr.isChangeWeapon = true;
        WeaponMgr.currentWeaponAnim.SetTrigger("Weapon_Out");
        PlayerCtrl.isActivated = false;
        WeaponSway.isActivated = false;

        yield return new WaitForSeconds(0.2f);

        WeaponMgr.currentWeapon.gameObject.SetActive(false);
        tf_MeatDissolveTool.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        SoundMgr.instance.PlaySE(sound_meat);
        yield return new WaitForSeconds(1.8f);

        theInventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().itemNumber);

        WeaponMgr.currentWeapon.gameObject.SetActive(true);
        tf_MeatDissolveTool.gameObject.SetActive(false);

        PlayerCtrl.isActivated = true;
        WeaponSway.isActivated = true;
        WeaponMgr.isChangeWeapon = false;
        isDissolving = false;
    }

    private void CanPickUp()
    {
        if (pickupAtivated)
        {
            if (hitInfo.transform != null)
            {
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다.");
            }
        }
    }

    private void CheckAction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
                ItemInfoAppear();
            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")
                MeatInfoAppear();
            else if (hitInfo.transform.tag == "Fire")
                FireInfoAppear();
            else
                InfoDisappear();
        }
        else
            InfoDisappear();
    }

    private void Reset()
    {
        pickupAtivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
    }

    private void ItemInfoAppear()
    {
        Reset();
        pickupAtivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void MeatInfoAppear()
    {
        if (hitInfo.transform.GetComponent<Animal>().isDead)
        {
            Reset();
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + "해체하기" + "<color=yellow>" + "(E)" + "</color>";
        }        
    }

    private void FireInfoAppear()
    {
        Reset();
        fireLookActivated = true;        
        if (hitInfo.transform.GetComponent<Fire>().GetisFire())
        {
            actionText.gameObject.SetActive(true);
            actionText.text = "선택된 아이템 불에 넣기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        pickupAtivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
