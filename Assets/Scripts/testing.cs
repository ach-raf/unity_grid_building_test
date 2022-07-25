using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

public class testing : MonoBehaviour
{
    public BuildingScriptableObject[] building_objects;
    private BuildingScriptableObject selected_building_object;

    private static int selected_building_index = 0;
    [SerializeField] public GameObject canvas;
    [SerializeField] public Camera current_camera;


    public GridScriptableObject gridsystem_object;
    public GridObject tile_object;

    public int width;
    public int height;
    public float cell_size = 5f;
    public Vector3 origin_postiion = new Vector3(0, 0, 0);
    public Material preview_material;
    private GameObject preview_object;

    private void OnEnable()
    {
        EventManager.BuildableDestroyEvent += BuildableDestoryed;
    }
    private void OnDisable()
    {
        EventManager.BuildableDestroyEvent -= BuildableDestoryed;
    }
    void Start()
    {

        gridsystem_object.grid_system = new GridSys<GridObject>(width, height, cell_size, origin_postiion, (GridSys<GridObject> grid, int x, int y, int z) => CreateGridObject(new Vector3(x, y, z), grid));
        InstantiateGridObjects();
    }

    private GridObject CreateGridObject(Vector3 _postinion, GridSys<GridObject> _grid)
    {
        tile_object = ScriptableObject.Instantiate(tile_object);
        return tile_object.Init(_postinion, gridsystem_object.grid_system);
    }

    void LateUpdate()
    {
        if (preview_object)
        {
            preview_object.transform.position = PositionHelper.mouse_ground_position.point;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {

                //ChangeObjectColor();
                InstantiateBuilding();
            }
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                //GetClickedObject().SetColor(Color.black);
                GetClickedObject().right_click();
                //GetDestoryableObject()?.DestroyObject();
            }
        }

    }

    void InstantiateGridObjects()
    {
        GridObject grid_object;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject spawn_object = Instantiate(tile_object.game_object);
                spawn_object.transform.position = gridsystem_object.grid_system.GetWorldPosition(x, z);
                spawn_object.transform.SetParent(transform);
                spawn_object.name = $"{x}, {z}";

                grid_object = gridsystem_object.grid_system.GetGridObject(x, z);
                grid_object.SetGameObject(spawn_object);
                grid_object.SetGrid(gridsystem_object.grid_system);
                bool is_offset = (x + z) % 2 == 0;
                if (is_offset)
                {
                    grid_object.SetColor(Color.green);
                }
                grid_object.original_color = grid_object.GetColor();
                gridsystem_object.grid_system.SetValue(x, z, grid_object);
            }
        }
    }

    void InstantiateBuilding()
    {
        Vector3 pos = Vector3.zero;
        bool can_build = true;
        if (preview_object)
        {

            pos = preview_object.transform.position;
            //IClickable clicked_object = grid_object;
        }
        else
        {
            IClickable clicked_object = GetClickedObject();
            if (clicked_object != null)
            {
                pos = clicked_object.GetTransform().position;

            }
        }
        gridsystem_object.grid_system.GetXZ(pos, out int x, out int z);
        GridObject grid_object = gridsystem_object.grid_system.GetGridObject(x, z);
        Debug.Log($"{x}, {z}");
        pos = grid_object.GetTransformPosition();
        Debug.Log($"{pos}");
        if (grid_object.GetOccupied())
        {
            Debug.Log("Cant Build Here");
        }
        else
        {
            DestroyPreview();
            BuildingScriptableObject building_ScriptableObject = ScriptableObject.Instantiate(selected_building_object);
            List<(int, int)> _list = building_ScriptableObject.GetBuildingAreaList(pos);
            if (x + building_ScriptableObject.width > width || z + building_ScriptableObject.height > height)
            {
                Debug.Log("Cant Build Here");
                return;
            }

            foreach (var item in _list)
            {
                GridObject update_grid_object = gridsystem_object.grid_system.GetGridObject(item.Item1, item.Item2);
                if (update_grid_object.GetOccupied())
                {
                    Debug.Log("Cant Build Here");
                    can_build = false;
                    break;
                }
            }
            if (can_build)
            {
                building_ScriptableObject.SetBuildingData(pos, building_ScriptableObject.width, building_ScriptableObject.height);
                building_ScriptableObject.game_object = Instantiate(building_ScriptableObject.game_object, building_ScriptableObject.transform_position, Quaternion.identity);
                building_ScriptableObject.game_object.GetComponent<Buildable>().SetBuildingData(building_ScriptableObject);

                building_ScriptableObject.game_object.GetComponent<Buildable>().SetUpPanel(canvas, current_camera);

                foreach (var item in _list)
                {
                    GridObject update_grid_object = gridsystem_object.grid_system.GetGridObject(item.Item1, item.Item2);
                    update_grid_object.SetOccupied(true);
                    update_grid_object.SetColor(Color.grey);
                    update_grid_object.building_data = building_ScriptableObject;
                    gridsystem_object.grid_system.SetValue(item.Item1, item.Item2, update_grid_object);
                }
            }
        }



    }

    void ChangeObjectColor()
    {
        GetClickedObject()?.SetColor(Color.red);
    }
    void BuildableDestoryed(Buildable buildable_object)
    {
        List<(int, int)> _list = buildable_object.GetBuildingData().GetBuildingAreaList(buildable_object.GetBuildingData().position);
        foreach (var item in _list)
        {

            GridObject update_grid_object = gridsystem_object.grid_system.GetGridObject(item.Item1, item.Item2);
            update_grid_object.SetOccupied(false);
            update_grid_object.SetColor(update_grid_object.original_color);
            update_grid_object.building_data = null;
            gridsystem_object.grid_system.SetValue(item.Item1, item.Item2, update_grid_object);
        }
    }


    public IClickable GetClickedObject()
    {
        return PositionHelper.GetClickedObject();
    }

    public IDestoryable GetDestoryableObject()
    {
        return PositionHelper.GetDestoryableObject();
    }
    private void DebugList(List<(int, int)> _list)
    {
        foreach (var item in _list)
        {
            Debug.Log($"{item.Item1}, {item.Item2}");
        }
    }

    void right_click()
    {
        GetClickedObject()?.right_click();
    }

    void preview_building()
    {
        preview_object = Instantiate(selected_building_object.game_object, PositionHelper.ground_world_position, Quaternion.identity);
        preview_object.GetComponent<Buildable>().SetMaterial(preview_material);


    }

    public void button_click()
    {
        DestroyPreview();
        if (selected_building_index >= building_objects.Length)
        {
            selected_building_index = 0;
        }
        selected_building_object = building_objects[selected_building_index];
        preview_building();
        selected_building_index++;
    }

    void DestroyPreview()
    {
        if (preview_object)
        {
            preview_object.GetComponent<Buildable>().DestroyPanel();
            Destroy(preview_object);
            preview_object = null;
            /*if (preview_object.TryGetComponent<Buildable>(out Buildable _buildable))
            {
                _buildable.DestroyObject();
                //Destroy(preview_object);
                //preview_object = null;
            }*/

        }

    }

}