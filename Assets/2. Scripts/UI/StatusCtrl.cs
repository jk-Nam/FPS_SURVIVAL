using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusCtrl : MonoBehaviour
{
    [SerializeField] private int hp;
    private int currentHp;

    [SerializeField] private int sp;
    private int currentSp;

    [SerializeField] private int spIncreaseSpeed;

    [SerializeField] private int spRechargeTime;
    private int currentSpRechargeTime;

    private bool spUsed;

    [SerializeField] private int dp;
    private int currentDp;

    [SerializeField] private int hungry;
    private int currentHungry;

    [SerializeField] private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    [SerializeField] private int thirsty;
    private int currentThirsty;

    [SerializeField] private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;
    

    [SerializeField] private int satisfy;
    private int currentSatisfy;

    [SerializeField] private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    private float temp;


    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }



    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime ++;
            else
                spUsed = false;
        }
    }

    private void SPRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다.");
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다.");
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    public void DecreaseHP(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("hp가 0이 되었습니다!!");
    }

    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }

    public void DecreaseSP(int _count)
    {
        if (currentSp > 0)
        {
            DecreaseSP(_count);
            return;
        }

        currentSp -= _count;

        if (currentSp <= 0)
            Debug.Log("sp가 0이 되었습니다!!");
    }

    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < hp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDP(int _count)
    {
            currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("방어력이 0이 되었습니다!!");
    }

    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;
        Debug.Log(currentSp);

        if (currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
            currentSp = 0;
    }


    public int GetCurrentSP()
    {
        return currentSp;
    }
}
