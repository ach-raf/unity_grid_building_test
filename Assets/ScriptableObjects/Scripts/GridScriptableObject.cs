using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Grid ScriptableObject", menuName = "Grid/Grid ScriptableObject", order = 0)]
public class GridScriptableObject : ScriptableObject
{
    public new string name;
    public int width; //x
    public int y = 0;
    public int height; //z

    public float cell_size = 5;

    public Vector3 origin_position = new Vector3(0, 0, 0);
    public GridSys<GridObject> grid_system;
    //public BuildingScriptableObject[,] buildings_list;

    private void OnEnable()
    {

    }


}

