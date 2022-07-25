using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PositionHelper : MonoBehaviour
{
    public static Vector3 ground_world_position;
    public static bool is_ground = false;
    public static bool is_building = false;
    public static Vector3 building_world_position;
    public static RaycastHit default_ray_hit;

    public static Vector3 ui_mouse_position;
    public static RaycastHit mouse_ground_position;

    [SerializeField] Camera current_camera;
    [SerializeField] LayerMask ground_mask;
    [SerializeField] LayerMask building_mask;




    private void Awake()
    {
        /*default_ray_hit = new RaycastHit();
        default_ray_hit.transform.position = Vector3.zero;*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UiDetect();
        MouseGroundPosition();
        SimpleDetect();
        //input_controller.FindAction('Mouse').actionMap.

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                MouseGroundPosition();
                MouseDetect();
                SimpleDetect();

            }
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {

        }
    }

    void MouseGroundPosition()
    {
        //Using layermask you can select which layer the click should register.
        Ray ray = current_camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit ground_ray_hit, float.MaxValue, ground_mask))
        {
            mouse_ground_position = ground_ray_hit;
        }

    }

    void MouseDetect()
    {
        //Using layermask you can select which layer the click should register.
        Ray ray = current_camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit ground_ray_hit, float.MaxValue, ground_mask))
        {
            ground_world_position = ground_ray_hit.point;
            is_ground = true;
            is_building = false;
            //GameObject.Destroy(ray_cast_hit.transform.gameObject);
            //Debug.Log(clicked_mouse_world_position);
        }
        else if (Physics.Raycast(ray, out RaycastHit building_ray_hit, float.MaxValue, building_mask))
        {
            building_world_position = building_ray_hit.point;
            is_ground = false;
            is_building = true;
            //GameObject.Destroy(ray_cast_hit.transform.gameObject);
            //Debug.Log(clicked_mouse_world_position);
        }
        else
        {
            is_ground = false;
            is_building = false;
        }
    }

    public void SimpleDetect()
    {
        //not using layermask, the hit returns the first collision.
        Ray ray = current_camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit ray_hit, float.MaxValue))
        {
            default_ray_hit = ray_hit;
        }
    }

    public static void CorrectPosition(Vector3 world_position, float cell_size, out int x, out int y, out int z)
    {
        x = Mathf.RoundToInt(world_position.x / cell_size);
        y = Mathf.RoundToInt(world_position.y / cell_size);
        z = Mathf.RoundToInt(world_position.z / cell_size);
    }

    public static GameObject GetHitGameObject()
    {

        return mouse_ground_position.transform.gameObject;
    }

    public static void LeftClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            try
            {
                GameObject selected_object = GetHitGameObject();
                //IClickable clicked_object = selected_object.GetComponentInParent<IClickable>();
                //clicked_object.click();
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("Null Reference Exception");
            }
        }


    }

    public static void RightClick()
    {
        try
        {
            GameObject selected_object = GetHitGameObject();
            //IClickable clicked_object = selected_object.GetComponentInParent<IClickable>();
            //clicked_object.right_click();
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Null Reference Exception");
        }

    }

    public void UiDetect()
    {
        //not using layermask, the hit returns the first collision.
        Ray ray = current_camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit ray_hit, float.MaxValue))
        {
            ui_mouse_position = ray_hit.point;
        }
    }

    public static IClickable GetClickedObject()
    {
        GameObject selected_object = PositionHelper.GetHitGameObject();
        IClickable clicked_object = selected_object.GetComponentInParent<IClickable>();
        return clicked_object;
    }


    public static IDestoryable GetDestoryableObject()
    {
        GameObject selected_object = default_ray_hit.transform.gameObject;
        IDestoryable clicked_object = selected_object.GetComponentInParent<IDestoryable>();
        return clicked_object;
    }
}
