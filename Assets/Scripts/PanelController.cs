using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour, IClickable
{

    public Transform GetTransform()
    {
        return this.transform;
    }
    public void SetColor(Color _color)
    {

    }
    public void click()
    {
        Debug.Log("Process");
    }
    public void right_click()
    {
        Debug.Log("right_click");
    }
}
