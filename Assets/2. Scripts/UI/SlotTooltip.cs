using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTooltip : MonoBehaviour
{
    [SerializeField] private GameObject go_Base;

    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Text txt_ItemDesc;
    [SerializeField] private Text txt_ItemHowToUsed;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
        txt_ItemDesc.text = _item.itemDesc;

        if (_item.itemTpye == Item.ItemTpye.Equipment)
            txt_ItemHowToUsed.text = "��Ŭ��-����";
        else if (_item.itemTpye == Item.ItemTpye.Used)
            txt_ItemHowToUsed.text = "��Ŭ��-���";
        else
            txt_ItemHowToUsed.text = "";
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}