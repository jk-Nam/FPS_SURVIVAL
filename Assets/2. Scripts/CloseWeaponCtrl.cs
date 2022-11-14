using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponCtrl : MonoBehaviour
{
    [SerializeField] protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;
    [SerializeField] protected LayerMask layerMask;

    private PlayerCtrl thePlayerCtrl;

    private void Start()
    {
        thePlayerCtrl = FindObjectOfType<PlayerCtrl>();
    }

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    if (CheckObject())
                    {
                        if (currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree")
                        {
                            StartCoroutine(thePlayerCtrl.TreeLookCoroutine(hitInfo.transform.GetComponent<TreeComponent>().GetTreeCenterPos()));
                            StartCoroutine(AttackCoroutine("Chop", currentCloseWeapon.workDelayA, currentCloseWeapon.workDelayB, currentCloseWeapon.workDelay));
                            return;
                        }
                    }

                    StartCoroutine(AttackCoroutine("Attack", currentCloseWeapon.attackDelayA, currentCloseWeapon.attackDelayB, currentCloseWeapon.attackDelay));
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine(string swingType, float _delayA, float _delayB, float _delayC)
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(_delayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(_delayB);
        isSwing = false;

        yield return new WaitForSeconds(_delayC - _delayA - _delayB);

        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();
    

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }

        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponMgr.currentWeapon != null)
        {
            WeaponMgr.currentWeapon.gameObject.SetActive(false);
        }

        currentCloseWeapon = _closeWeapon;
        WeaponMgr.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponMgr.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
