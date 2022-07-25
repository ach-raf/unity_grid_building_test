using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIle : MonoBehaviour, IClickable
{
    private MeshRenderer mesh_renderer;
    private void Awake()
    {
        mesh_renderer = GetComponentInChildren<MeshRenderer>();
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public void SetColor(Color _color)
    {
        mesh_renderer.material.color = _color;
    }

    public void click()
    {

    }
    public void right_click()
    {
        Debug.Log("right_click");
    }

    /*public void DestroyObject(IClickable clicked_object)
    {
        EventManager.OnBuildableDestroy(clicked_object);
        Destroy(gameObject);
    }*/



}
