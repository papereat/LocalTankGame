using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class HostControler : MonoBehaviour
{
    public static HostControler current;
    [Header("refrences")]
    public Tilemap tm;
    public Camera mainCamera;
    public Tile EmptyTile;
    public Tile TankTile;
    public List<TankControler> tankControlers;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ActionPointsText;
    public TMP_InputField NameInput;
    [Header("Stats")]
    public int MovementCost;
    [Header("Debug")]
    public Vector2 DebugPos;


    [SerializeField]
    int CurrentTank=0;
    [SerializeField]
    Vector2Int SelectedTile;
    void Awake()
    {
        current=this;
        mainCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        DebugPos=GetMousePos();
        Inputs();
        UpdateUI();
    }
    void UpdateUI()
    {
        HealthText.text=tankControlers[CurrentTank].Health.ToString();
        ActionPointsText.text=tankControlers[CurrentTank].ActionPoints.ToString();
        NameInput.text=tankControlers[CurrentTank].Name;
    }
    public void ChangeTankName()
    {
        tankControlers[CurrentTank].Name=NameInput.text;
    }

    Vector2Int GetMousePos()
    {
        Vector2 cameraPos=mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int(tm.WorldToCell(cameraPos).x,tm.WorldToCell(cameraPos).y);
    }

    int CheckMovementCost(Vector2Int a,Vector2Int b,int MovemetnCost)
    {
        int TilesMoved=Mathf.Max(Mathf.Abs(a.y-b.y),Mathf.Abs(a.x-b.x));

        return TilesMoved*MovemetnCost;
    }
    public void Inputs()
    {
        //Q and E
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CurrentTank-=1;
            if(CurrentTank<0)
            {
                CurrentTank=tankControlers.Count-1;
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            CurrentTank+=1;
            if(CurrentTank>tankControlers.Count-1)
            {
                CurrentTank=0;
            }
        }
        //Click
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(GetMousePos()==SelectedTile)
            {
                if(tm.GetTile(new Vector3Int(SelectedTile.x,SelectedTile.y,0))==EmptyTile)
                {
                    //Move
                    //Instantaly Move
                    int movCost=CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost);
                    if(tankControlers[CurrentTank].ActionPoints>=movCost)
                    {
                        tankControlers[CurrentTank].Move(SelectedTile,movCost);
                        Debug.Log(movCost);
                    }
                }
                else 
                {
                    //Open Oponent Menu
                }
            }
            else
            {
                //Select TIle
                SelectedTile=GetMousePos();
            }
        }
    }
}
