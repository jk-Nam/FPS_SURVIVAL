using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeCtrl : CloseWeaponCtrl
{
    public static bool isActivate = true;


    private void Start()
    {
        WeaponMgr.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponMgr.currentWeaponAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundMgr.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }
                //else if (hitInfo.transform.tag == "StrongAnimal")
                //{
                //    SoundMgr.instance.PlaySE("Animal_Hit");
                //    hitInfo.transform.GetComponent<StrongAnimal>().Damage(1, transform.position);
                //}
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
