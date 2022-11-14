using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public Building.Type needType;
    private bool needTypeFlag;

    private List<Collider> colList = new List<Collider>();

    [SerializeField] private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (needType == Building.Type.Normal)
        {
            if (colList.Count > 0)
                SetColor(red);
            else
                SetColor(green);
        }

        else
        {
            if (colList.Count > 0  || !needTypeFlag)
                SetColor(red);
            else
                SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {
        foreach(Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Structure")
        {
            if (other.GetComponent<Building>().type != needType)
                colList.Add(other);
            else
                needTypeFlag = true;
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
                colList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Structure")
        {
            if (other.GetComponent<Building>().type != needType)
                colList.Remove(other);
            else
                needTypeFlag = false;
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
                colList.Remove(other);
        }
    }

    public bool isBuildable()
    {
        if (needType == Building.Type.Normal)
            return colList.Count == 0;
        else
            return colList.Count == 0 && needTypeFlag;
    }
}
