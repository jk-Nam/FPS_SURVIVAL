using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private string fireName;
    [SerializeField] private int Damage;
    [SerializeField] private float damageTime;
    private float currentDamageTime;

    [SerializeField] private float durationTime;
    private float currentDurationTime;

    [SerializeField] private ParticleSystem ps_Flame;

    private bool isFire = true;

    private StatusCtrl thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<StatusCtrl>();
        currentDurationTime = durationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        currentDurationTime -= Time.deltaTime;

        if (currentDamageTime >0)
        {
            currentDamageTime -= Time.deltaTime;
        }

        if (currentDurationTime <= 0)
        {
            Off();
        }
    }

    private void Off()
    {
        ps_Flame.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFire && other.transform.tag == "Player")
        {
            if (currentDamageTime <= 0)
            {
                other.GetComponent<Burn>().StartBurning();
                thePlayer.DecreaseHP(Damage);
                currentDamageTime = damageTime;
            }            
        }
    }

    public bool GetisFire()
    {
        return isFire;
    }
}
