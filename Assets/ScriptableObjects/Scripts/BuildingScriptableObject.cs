using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "Building ScriptableObject", menuName = "Building/Building ScriptableObject", order = 1)]
public class BuildingScriptableObject : ScriptableObject
{
    public GameObject game_object;
    private float click_error_tolerance = 1.5f;

    public float cell_size = 5;
    public Vector3 offset;
    public static int id_counter = 0;
    public int id_building;
    public int width;
    public int height;

    public Vector3 position;
    public Vector3 transform_position;
    public RessourceType ressource_type = RessourceType.None;

    public enum RessourceType
    {
        None,
        Standard,
        Premium
    }

    public void SetBuildingData(Vector3 _position, int _width, int _height, RessourceType _ressource_type = RessourceType.None)
    {

        this.id_building = id_counter;
        this.width = _width;
        this.height = _height;
        this.transform_position = CalculateOffsetPosition(_position);
        this.position = new Vector3(_position.x, _position.y + 1, _position.z);
        this.name = $"({position.x}, {position.y}, {position.z})";
        this.ressource_type = _ressource_type;
        id_counter++;
    }
    public override string ToString()
    {
        return $"Building: {name}";
    }

    public Vector3 CalculateOffsetPosition(Vector3 _position)
    {
        float x = _position.x + offset.x;
        float y = _position.y + offset.y;
        float z = _position.z - offset.z;
        return new Vector3(x, y, z);
    }
    public void GetXZ(Vector3 world_position, out int x, out int z)
    {

        int temp_x = Mathf.FloorToInt((world_position).x / cell_size);
        int temp_z = Mathf.FloorToInt((world_position).z / cell_size);

        if (world_position.x - click_error_tolerance <= temp_x || world_position.x - click_error_tolerance >= temp_x && world_position.z - click_error_tolerance <= temp_z || world_position.z - click_error_tolerance >= temp_z)
        {
            x = temp_x;
            z = temp_z;
        }
        else
        {
            x = -1;
            z = -1;

        }
    }

    public List<(int, int)> GetBuildingAreaList(Vector3 starting_position)
    {
        GetXZ(starting_position, out int x, out int z);
        List<(int, int)> result = new List<(int, int)>();

        for (int i = x; i < x + width; i += 1)
        {
            for (int j = z; j < z + height; j += 1)
            {
                result.Add((i, j));
            }
        }
        return result;
    }

    void InstantiateBuilding()
    {
        BuildingScriptableObject building_object = ScriptableObject.Instantiate(this);
        GameObject selected_object = PositionHelper.GetHitGameObject();
        IClickable clicked_object = selected_object.GetComponentInParent<IClickable>();
        Vector3 _pos = clicked_object.GetTransform().position;
        building_object.SetBuildingData(_pos, width, height);
        GameObject spawn_building = Instantiate(building_object.game_object, building_object.transform_position, Quaternion.identity);
    }





}
