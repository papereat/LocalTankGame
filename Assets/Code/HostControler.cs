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
    public Image ColorImage;
    public Transform SelectedTileUI;
    public Transform CurrentTileUI;
    public GameObject MovementAskingObject;
    public TextMeshProUGUI MovementCostText;
    public GameObject OponentMenu;
    public TextMeshProUGUI OpoenentMenuName;
    public TextMeshProUGUI OpoenentMenuHealthValue;
    public TextMeshProUGUI OpoenentMeneActionPointsValue;
    public TextMeshProUGUI OpoenentMeneRangeValue;
    public TextMeshProUGUI OpoenentMeneTradingAmountValue;
    public TextMeshProUGUI OpoenentMeneDamageAmountValue;
    public Slider TradingAmountSlider;
    public Slider DamageAmountSlider;

    [Header("Stats")]
    public int MovementCost;
    [Header("Debug")]
    public Vector2 DebugPos;


    [SerializeField]
    bool OponentMenuOpen;
    [SerializeField]
    int selectedOponent;
    bool AskingForMovement;
    int CurrentTank=0;
    bool tileIsSelected=false;
    Vector2Int SelectedTile;
    bool inTextField;
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
        ColorImage.color=tankControlers[CurrentTank].TankColor;
        if(tileIsSelected)
        {
            SelectedTileUI.gameObject.SetActive(true);
            SelectedTileUI.GetComponent<SpriteRenderer>().color=new Color(tankControlers[CurrentTank].TankColor.r,tankControlers[CurrentTank].TankColor.g,tankControlers[CurrentTank].TankColor.b,0.5f);
            Vector3 tilePos=tm.CellToWorld(new Vector3Int(SelectedTile.x,SelectedTile.y,0));
            SelectedTileUI.position=new Vector3(tilePos.x+0.5f,tilePos.y+0.5f,0);
        }
        else
        {
            SelectedTileUI.gameObject.SetActive(false);
        }

        CurrentTileUI.GetComponent<SpriteRenderer>().color=new Color(tankControlers[CurrentTank].TankColor.r,tankControlers[CurrentTank].TankColor.g,tankControlers[CurrentTank].TankColor.b,0.4f);
        Vector3 MouseTilPos=tm.CellToWorld(new Vector3Int(GetMousePos().x,GetMousePos().y,0));
        CurrentTileUI.position=new Vector3(MouseTilPos.x+0.5f,MouseTilPos.y+0.5f,0);

        if(AskingForMovement)
        {
            MovementAskingObject.SetActive(true);
            MovementCostText.text="Cost: "+CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost).ToString();
        }
        else
        {
            MovementAskingObject.SetActive(false);
        }

        if(OponentMenuOpen)
        {
            OponentMenu.SetActive(true);
            OpoenentMenuName.text=tankControlers[selectedOponent].Name;
            OpoenentMenuName.color=tankControlers[selectedOponent].TankColor;
            OpoenentMenuHealthValue.text=tankControlers[selectedOponent].Health.ToString();
            OpoenentMeneActionPointsValue.text=tankControlers[selectedOponent].ActionPoints.ToString();
            OpoenentMeneRangeValue.text=tankControlers[selectedOponent].TankRange.ToString();
            OpoenentMeneTradingAmountValue.text=TradingAmountSlider.value.ToString();
            OpoenentMeneDamageAmountValue.text=DamageAmountSlider.value.ToString();
            TradingAmountSlider.maxValue=tankControlers[CurrentTank].ActionPoints;
            DamageAmountSlider.maxValue=Mathf.Min(tankControlers[selectedOponent].Health,tankControlers[CurrentTank].ActionPoints);
        }
        else
        {
            OponentMenu.SetActive(false);
        }

    }
    public void MovementQuestionInput(bool Accepted)
    {
        AskingForMovement=false;
        if(Accepted)
        {
            int movCost=CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost);
            tankControlers[CurrentTank].Move(SelectedTile,movCost);
            Debug.Log(movCost);
            tileIsSelected=false;
        }
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
    public void TextSelected()
    {
        inTextField=true;
    }
    public void TextUnselected()
    {
        inTextField=false;
    }
    public void Inputs()
    {
        //Q and E
        if(Input.GetKeyDown(KeyCode.Q)&&!inTextField)
        {
            CurrentTank-=1;
            if(CurrentTank<0)
            {
                tileIsSelected=false;
                CurrentTank=tankControlers.Count-1;
            }
            AskingForMovement=false;
        }
        if(Input.GetKeyDown(KeyCode.E)&&!inTextField)
        {
            CurrentTank+=1;
            if(CurrentTank>tankControlers.Count-1)
            {
                tileIsSelected=false;
                CurrentTank=0;
            }
            AskingForMovement=false;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OponentMenuOpen)
            {
                OponentMenuOpen=false;
            }
        }
        //Click
        if(Input.GetKeyDown(KeyCode.Mouse0)&&!AskingForMovement&&!OponentMenuOpen)
        {
            if(GetMousePos()==SelectedTile&&tileIsSelected)
            {
                if(tm.GetTile(new Vector3Int(SelectedTile.x,SelectedTile.y,0))==EmptyTile)
                {
                    if(tankControlers[CurrentTank].ActionPoints<=CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost))
                    {
                        AskingForMovement=true;
                    }
                    else
                    {
                        //Cant move Too Little Action Points
                    }
                }
                else 
                {
                    int x=0;
                    while (x<tankControlers.Count)
                    {
                        if(tankControlers[x].Pos==SelectedTile&&x!=CurrentTank)
                        {
                            selectedOponent=x;
                            OponentMenuOpen=true;
                            
                            
                        }

                        x++;
                    }
                }
            }
            else
            {
                //Select TIle
                SelectedTile=GetMousePos();
                tileIsSelected=true;
            }
        }
    }
}
