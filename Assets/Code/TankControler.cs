using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class TankControler : MonoBehaviour
{
    [Header("Refrences")]
    public Tilemap tm;
    public Tile EmptyTile;
    public Tile TankTile;
    public TextMeshPro StatText;
    [Header("Stats")]
    public string Name;
    public Vector2Int Pos;
    public int Health;
    public int ActionPoints;
    // Start is called before the first frame update
    void Start()
    {
        SetCurrentTileToTank();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        
        SetPosToTile();
    }
    void UpdateUI()
    {
        StatText.text=ActionPoints.ToString()+":"+Health.ToString();
    }
    void SetCurrentTileToTank()
    {
        tm.SetTile(new Vector3Int(Pos.x,Pos.y,0),TankTile);
    }
    void SetPosToTile()
    {
        transform.position=tm.CellToWorld(new Vector3Int(Pos.x,Pos.y,0));
    }
    public void Move(Vector2Int NewPos,int ActionPointCost)
    {
        tm.SetTile(new Vector3Int(Pos.x,Pos.y,0),EmptyTile);
        Pos=NewPos;
        ActionPoints-=ActionPointCost;
        SetCurrentTileToTank();
    }
}
