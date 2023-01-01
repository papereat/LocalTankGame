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
    public Transform RangeShower;
    public TextMeshPro NameText;
    [Header("Stats")]
    public string Name;
    public int TankRange;
    public Vector2Int Pos;
    public int Health;
    public int ActionPoints;
    public Color TankColor;
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
        RangeShower.localScale=new Vector3(1+2*TankRange,1+2*TankRange,1);
        RangeShower.GetComponent<SpriteRenderer>().color=new Color(TankColor.r,TankColor.g,TankColor.b,0.5f);
        NameText.text=Name;
        NameText.color=TankColor;
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
    public void Attack(int ActionPointsUsed)
    {
        ActionPoints-=ActionPointsUsed;
    }
    public void BeAttack(int HealthLost)
    {
        Health-=HealthLost;
    }
}
