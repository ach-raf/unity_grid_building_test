using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GridSys<TGenericType>
{

    private int width;
    private int height;
    private int y = 0;
    private float cell_size;
    private Vector3 origin_position;
    private TGenericType[,] grid_array;

    private float click_error_tolerance = 1.5f;



    public GridSys(int _width, int _height, float _cell_size, Vector3 _origin_position, Func<GridSys<TGenericType>, int, int, int, TGenericType> CreateGridObject)
    {
        this.width = _width;
        this.height = _height;
        this.cell_size = _cell_size;
        this.origin_position = _origin_position;
        grid_array = new TGenericType[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                SetValue(x, z, CreateGridObject(this, x, y, z));
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, y, z) * cell_size + origin_position;
    }

    public void GetXZ(Vector3 world_position, out int x, out int z)
    {

        int temp_x = Mathf.FloorToInt((world_position - origin_position).x / cell_size);
        int temp_z = Mathf.FloorToInt((world_position - origin_position).z / cell_size);

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

    public TGenericType GetGridObject(Vector3 world_position)
    {
        int x, z;
        GetXZ(world_position, out x, out z);
        if (x >= 0 && z >= 0 && x < width && z < height)
            return grid_array[x, z];
        else return default(TGenericType);
    }
    public TGenericType GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
            return grid_array[x, z];
        else return default(TGenericType);
    }

    public void SetValue(int x, int z, TGenericType CreateGridObject)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            grid_array[x, z] = CreateGridObject;
        }
    }
    public void SetValue(Vector3 world_position, TGenericType CreateGridObject)
    {
        int x, z;
        GetXZ(world_position, out x, out z);
        SetValue(x, z, CreateGridObject);

    }

    public TGenericType GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return grid_array[x, z];
        }
        else return default(TGenericType);
    }
    public TGenericType GetValue(Vector3 world_position)
    {
        int x, z;
        GetXZ(world_position, out x, out z);
        return GetValue(x, z);
    }

    public int GetArrayLength()
    {
        return grid_array.Length;
    }




}

