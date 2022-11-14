using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, SP, DP, HUNGTY, THIRSTY, STISFY만 가능합니다.")]
    public string[] part;
    public int[] num;
}
public class ItemEffectDB : MonoBehaviour
{
    [SerializeField] private ItemEffect[] itemEffects;

    [SerializeField] private StatusCtrl thePlayerStatus;
    [SerializeField] private WeaponMgr theWeaponMgr;
    [SerializeField] private SlotTooltip theSlotToolTip;
    [SerializeField] private QuickSlotCtrl theQuickSlotCtrl;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void IsActivatedQuickSlot(int _num)
    {
        theQuickSlotCtrl.IsActivatedQuickSlot(_num);
    }


    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }

    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)
    {
        if (_item.itemTpye == Item.ItemTpye.Equipment)
        {
            StartCoroutine(theWeaponMgr.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }

        else  if (_item.itemTpye == Item.ItemTpye.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위, HP, SP, DP, HUNGTY, THIRSTY, STISFY만 가능합니다.");
                                break;
                        }
                        Debug.Log(_item.itemName + "을/를 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDB에 일치하는 itemName이 없습니다.");
        }
    }
}
