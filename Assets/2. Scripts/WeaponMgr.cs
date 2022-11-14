using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunCtrl))]
public class WeaponMgr : MonoBehaviour
{
    public static bool isChangeWeapon = false;

    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    [SerializeField] private float changeWeaponDelayTime;
    [SerializeField] private float chachangeWeaponEndDelayTimen;

    [SerializeField] private Gun[] guns;
    [SerializeField] private CloseWeapon[] hands;
    [SerializeField] private CloseWeapon[] axes;
    [SerializeField] private CloseWeapon[] pickaxes;
    [SerializeField] private string currentWeaponType;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    [SerializeField] private GunCtrl theGunCtrl;
    [SerializeField] private HandCtrl theHandCtrl;
    [SerializeField] private AxeCtrl theAxeCtrl;
    [SerializeField] private PickaxeCtrl thePickaxeCtrl;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(chachangeWeaponEndDelayTimen);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunCtrl.CancelFineSight();
                theGunCtrl.CancelReload();
                GunCtrl.isActivate = false;
                break;
            case "HAND":
                HandCtrl.isActivate = false;
                if (QuickSlotCtrl.go_HandItem != null)
                    Destroy(QuickSlotCtrl.go_HandItem);
                break;
            case "AXE":
                AxeCtrl.isActivate = false;
                break;
            case "PICKAXE":
                AxeCtrl.isActivate = false;
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunCtrl.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND")
        {
            theHandCtrl.CloseWeaponChange(handDictionary[_name]);
        }
        else if (_type == "AXE")
        {
            theAxeCtrl.CloseWeaponChange(axeDictionary[_name]);
        }
        else if (_type == "PICKAXE")
        {
            thePickaxeCtrl.CloseWeaponChange(pickaxeDictionary[_name]);
        }
    }

    public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }
}
