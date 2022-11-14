using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField] private float time;
    private float currentTime;

    private bool done;

    [SerializeField] GameObject go_CookedItem_prefab;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Fire" && !done)
        {
            currentTime += Time.deltaTime;

            if ( currentTime >= time)
            {
                done = true;
                Instantiate(go_CookedItem_prefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                Destroy(gameObject);
            }
        }
    }

}
