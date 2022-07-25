using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    public GameObject building_canvas;

    private DefaultInputActions default_input_actions;
    private InputAction ui_input;

    private GraphicRaycaster ui_raycaster;
    private PointerEventData click_data;
    private List<RaycastResult> click_results;

    //private static bool _canvas_state = true;
    private void Awake()
    {
        default_input_actions = new DefaultInputActions();
    }

    private void Start()
    {
        ui_raycaster = building_canvas.GetComponent<GraphicRaycaster>();
        click_data = new PointerEventData(EventSystem.current);
        click_results = new List<RaycastResult>();
    }
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {

            //building_canvas.SetActive(true);

            //Debug.Log("UI click");
            GetUiElementsClicked();

        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            /*building_canvas.SetActive(_canvas_state);
            _canvas_state = !_canvas_state;*/
        }
    }

    public void GetUiElementsClicked()
    {
        click_data.position = Mouse.current.position.ReadValue();
        click_results.Clear(); ui_raycaster.Raycast(click_data, click_results);
        foreach (RaycastResult result in click_results)
        {
            GameObject ui_element = result.gameObject;
            Debug.Log($"UI Manager: {ui_element.name}");
            if (ui_element.TryGetComponent<IClickable>(out IClickable clicked_object))
            {

                clicked_object?.click();
            }
        }
    }
}
