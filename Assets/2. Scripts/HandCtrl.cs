using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCtrl : CloseWeaponCtrl
{
    public static bool isActivate = false;

    [SerializeField] private QuickSlotCtrl theQuickSlotCtrl;



    void Update()
    {
        if (isActivate && !Inventory.inventoryActivated)
        {
            if (QuickSlotCtrl.go_HandItem == null)
                TryAttack();
            else
                TryEating();
        }
    }

    private void TryEating()
    {
        if (Input.GetButtonDown("Fire2") && !theQuickSlotCtrl.GetIsCoolTime())
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            theQuickSlotCtrl.DecreaseSelectedItem();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
