using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;

    public static bool isOpenInventory = false;
    public static bool isOpenCraftManual = false;

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false;

    private WeaponMgr theWeaponMgr;

    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        theWeaponMgr = FindObjectOfType<WeaponMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWeaponMgr.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
             {
                flag = false;
                theWeaponMgr.WeaponOut();
             }
        }            
    }

}
