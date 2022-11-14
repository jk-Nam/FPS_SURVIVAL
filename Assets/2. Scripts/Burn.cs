using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    private bool isBunning = false;

    [SerializeField] private int damage;

    [SerializeField] private float damageTime;
    private float currentDamageTime;

    [SerializeField] private float durationTime;
    private float currentDurationTime;

    [SerializeField] private GameObject flame_prefab;
    private GameObject go_tempFlame;

    public void StartBurning()
    {
        if (!isBunning)
        {
            go_tempFlame = Instantiate(flame_prefab, transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
            go_tempFlame.transform.SetParent(transform);
        }
        isBunning = true;
        currentDurationTime = durationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBunning)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        if (isBunning)
        {
            currentDurationTime -= Time.deltaTime;

            if (currentDamageTime <= 0)
            {
                Off();
            }

            if (currentDamageTime > 0)
                currentDamageTime -= Time.deltaTime;

            if (currentDamageTime <= 0)
            {
                Damage();
            }

        }
    }

    private void Damage()
    {
        currentDamageTime = damageTime;
        GetComponent<StatusCtrl>().DecreaseHP(damage);
    }

    private void Off()
    {
        isBunning = false;
        Destroy(go_tempFlame);
    }
}
