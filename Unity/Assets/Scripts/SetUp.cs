using System;
using System.Diagnostics;
using System.Reflection;
using GameOfLife;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetUp : MonoBehaviour
{
    private void Awake()
    {
        field = Game.GetInitialState();
        var width = field.GetLength(0);
        var height = field.GetLength(1);
        tilemap.size = new Vector3Int(width, height, 1);
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            tile.color = Color.green;
            tilemap.SetTile(new Vector3Int(x, y, 0), tile);
        }

        tilemap.RefreshAllTiles();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10);
        Camera.main.GetComponent<Camera>().orthographicSize = height / 2f;

        var resolution = Math.Min(Screen.height, Screen.width);
        Screen.SetResolution(resolution, resolution, false);
        lastUpdate = DateTime.Now;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tpos = tilemap.WorldToCell(worldPoint);
            field[tpos.x, tpos.y] = !field[tpos.x, tpos.y]; 
            var tileBse = tilemap.GetTile(tpos);
            if (tileBse != null && tileBse is Tile tile)
            {
                tile.color = field[tpos.x, tpos.y] ? Color.green : Color.black;
            }
            tilemap.RefreshTile(tpos);
        }

        var cameraMoveSpeed = 0.1f;
        if (Input.GetKey(KeyCode.LeftArrow))
            Camera.main.transform.position -= new Vector3(cameraMoveSpeed, 0, 0);
        if (Input.GetKey(KeyCode.RightArrow))
            Camera.main.transform.position += new Vector3(cameraMoveSpeed, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
            Camera.main.transform.position += new Vector3(0, cameraMoveSpeed, 0);
        if (Input.GetKey(KeyCode.DownArrow))
            Camera.main.transform.position -= new Vector3(0, cameraMoveSpeed, 0);
        if (Input.GetKeyDown(KeyCode.Space)) paused = !paused;

        if (Input.mouseScrollDelta.y != 0f)
        {
            grid.transform.localScale = new Vector3(Math.Min(Math.Max(grid.transform.localScale.x + Input.mouseScrollDelta.y / 25, 0.1f), 5),
                Math.Min(Math.Max(grid.transform.localScale.y + Input.mouseScrollDelta.y / 25, 0.1f), 5), 0);
        }

        if (Input.GetKeyDown(KeyCode.F1)) period = TimeSpan.FromSeconds(1);
        if (Input.GetKeyDown(KeyCode.F2)) period = TimeSpan.FromSeconds(0.9);
        if (Input.GetKeyDown(KeyCode.F3)) period = TimeSpan.FromSeconds(0.8);
        if (Input.GetKeyDown(KeyCode.F4)) period = TimeSpan.FromSeconds(0.7);
        if (Input.GetKeyDown(KeyCode.F5)) period = TimeSpan.FromSeconds(0.6);
        if (Input.GetKeyDown(KeyCode.F6)) period = TimeSpan.FromSeconds(0.5);
        if (Input.GetKeyDown(KeyCode.F7)) period = TimeSpan.FromSeconds(0.4);
        if (Input.GetKeyDown(KeyCode.F8)) period = TimeSpan.FromSeconds(0.3);
        if (Input.GetKeyDown(KeyCode.F9)) period = TimeSpan.FromSeconds(0.2);
        if (Input.GetKeyDown(KeyCode.F10)) period = TimeSpan.FromSeconds(0.1);
    }

    private void FixedUpdate()
    {
        var watch = new Stopwatch();
        watch.Start();
        TryUpdateState();
        UnityEngine.Debug.Log(watch.Elapsed);
    }

    private void TryUpdateState()
    {
        if (paused) return;
        if (DateTime.Now < lastUpdate + period) return;

        field = GameOfLife.Game.GetNextState(field);
        for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
            {
                var tileBse = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tileBse != null && tileBse is Tile tile)
                {
                    tile.color = field[x, y] ? Color.green : Color.black;
                }
            }

        tilemap.RefreshAllTiles();
        lastUpdate = DateTime.Now;
    }

    private TimeSpan period = TimeSpan.FromSeconds(0.5);
    private DateTime lastUpdate = DateTime.MinValue;
    private bool[,] field;
    private bool paused;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Grid grid;
}
