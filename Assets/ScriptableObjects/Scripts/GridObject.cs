using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tile ScriptAbleObject", menuName = "Tile/Tile ScriptableObject", order = 0)]
public class GridObject : ScriptableObject
{
    public BuildingScriptableObject building_data;
    public GameObject game_object;
    private MeshRenderer _mesh_renderer;
    private Vector3 position;
    private Vector3 transform_position;
    private GridSys<GridObject> grid;
    private bool occupied;

    public Color original_color;

    public float width = 1;
    public float height = 1;
    public float cell_size = 5;


    public GridObject Init(Vector3 _position, GridSys<GridObject> _grid)
    {
        position = _position;
        grid = _grid;
        return this;
    }

    public override string ToString()
    {
        return $"{position.x}, {position.y}, {position.z}";
    }
    public void SetColor(Color _color)
    {
        _mesh_renderer.material.color = _color;
    }
    public Color GetColor()
    {
        return _mesh_renderer.material.color;
    }


    #region Getters and Setters
    public void SetPosition(Vector3 _position)
    {
        position = _position;
    }
    public Vector3 GetPosition()
    {
        return position;
    }
    public Vector3 GetTransformPosition()
    {
        return new Vector3(position.x * cell_size, position.y, position.z * cell_size);
    }
    public void SetOccupied(bool _occupied)
    {
        occupied = _occupied;
    }
    public bool GetOccupied()
    {
        return occupied;
    }
    public void SetGrid(GridSys<GridObject> _grid)
    {
        grid = _grid;
    }
    public GridSys<GridObject> GetGrid()
    {
        return grid;
    }

    public void SetGameObject(GameObject _gameobject)
    {
        game_object = _gameobject;
        _mesh_renderer = game_object.GetComponentInChildren<MeshRenderer>();
    }
    public GameObject GetGameObject()
    {
        return game_object;
    }

    #endregion


}
