using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    Transform GetTransform();
    void SetColor(Color _color);
    void click();

    void right_click();


    //void DestroyObject(IClickable clicked_object);
}
