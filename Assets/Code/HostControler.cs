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
    public GameObject ConfirmationObject;
    public TextMeshProUGUI ConfirmationText;
    public TextMeshProUGUI ConfirmationCostText;
    public GameObject OponentMenu;
    public TextMeshProUGUI OpoenentMenuName;
    public TextMeshProUGUI OpoenentMenuHealthValue;
    public TextMeshProUGUI OpoenentMeneActionPointsValue;
    public TextMeshProUGUI OpoenentMeneRangeValue;
    public TextMeshProUGUI OpoenentMeneTradingAmountValue;
    public TextMeshProUGUI OpoenentMeneDamageAmountValue;
    public Slider TradingAmountSlider;
    public Slider DamageAmountSlider;
    public GameObject AlertPannel;
    public TextMeshProUGUI AlertPannelText;
    public TextMeshProUGUI AlertPannelButtonText;
    public GameObject YouPannelObject;
    public TextMeshProUGUI CurrentRangeText;
    public TextMeshProUGUI UpgradeCosts;

    [Header("Stats")]
    public bool TradingEnabled;
    public bool UpgradeEnabled;
    public int UpgradeRangeCost;
    public int MaxRange;
    public int MovementCost;
    public int AttackCost;
    [Header("Debug")]
    public Vector2 DebugPos;


    [SerializeField]
    bool YouPannelOpen;
    bool ConfirmationPanelOpen;
    int ConfirmationPannelType;
    
    bool AlertPannelOpen;
    bool OponentMenuOpen;
    
    int selectedOponent;
    int CurrentTank=0;
    bool tileIsSelected=false;
    [SerializeField]
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
    void OpenConfimationPanel(string MainText,string SecondaryText,int type)
    {
        ConfirmationObject.SetActive(true);
        ConfirmationPanelOpen=true;
        ConfirmationPannelType=type;
        
        ConfirmationText.text=MainText;
        ConfirmationCostText.text=SecondaryText;
    }
    void AlertMenuOpen(string MainText,string ButtonText)
    {
        AlertPannelOpen=true;
        AlertPannel.SetActive(true);
        AlertPannelText.text=MainText;
        AlertPannelButtonText.text=ButtonText;
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

        if(YouPannelOpen)
        {
            //Show YOu Pannel
            YouPannelObject.SetActive(true);
            //Put Range
            CurrentRangeText.text="Range: "+tankControlers[CurrentTank].TankRange.ToString();
            //Price
            UpgradeCosts.text="Upgrade Costs: "+UpgradeRangeCost.ToString();
        }
        else
        {
            YouPannelObject.SetActive(false);
        }
        if(OponentMenuOpen)
        {
            //Show Meny
            OponentMenu.SetActive(true);
            //Show oponent stats
            OpoenentMenuName.text=tankControlers[selectedOponent].Name;
            OpoenentMenuName.color=tankControlers[selectedOponent].TankColor;
            OpoenentMenuHealthValue.text=tankControlers[selectedOponent].Health.ToString();
            OpoenentMeneActionPointsValue.text=tankControlers[selectedOponent].ActionPoints.ToString();
            OpoenentMeneRangeValue.text=tankControlers[selectedOponent].TankRange.ToString();
            //Slider Stuff
            OpoenentMeneTradingAmountValue.text=TradingAmountSlider.value.ToString();
            OpoenentMeneDamageAmountValue.text=DamageAmountSlider.value.ToString();
            //Max Value for Sliders
            TradingAmountSlider.maxValue=tankControlers[CurrentTank].ActionPoints;
            DamageAmountSlider.maxValue=Mathf.Min(tankControlers[selectedOponent].Health, Mathf.FloorToInt(tankControlers[CurrentTank].ActionPoints/AttackCost));
        }
        else
        {
            OponentMenu.SetActive(false);
        }

    }
    public void ConfirmationButtonPressed(bool Accepted)
    {
        ConfirmationPanelOpen=false;
        
        if(ConfirmationPannelType==0)
        {
            //Movement
            if(Accepted)
            {
                int movCost=CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost);
                tankControlers[CurrentTank].Move(SelectedTile,movCost);
                Debug.Log(movCost);
                tileIsSelected=false;
            }
        }
        else if(ConfirmationPannelType==1)
        {
            if(Accepted)
            {
                tankControlers[CurrentTank].Attack((int)DamageAmountSlider.value,AttackCost);
                tankControlers[selectedOponent].BeAttack((int)DamageAmountSlider.value);
            }
        }
        else if(ConfirmationPannelType==2)
        {
            //trade
            if(Accepted)
            {
                tankControlers[CurrentTank].Donate((int)TradingAmountSlider.value);
                tankControlers[selectedOponent].ReciveActionPoints((int)TradingAmountSlider.value);
            }
        }
        else if(ConfirmationPannelType==3)
        {
            //Range Upgrade
            if(Accepted)
            {
                tankControlers[CurrentTank].UpgradeRange(UpgradeRangeCost);
            }
        }
        ConfirmationObject.SetActive(false);
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
    public void AttackButtonPressed()
    {
        if(DistanceBetweenPos(tankControlers[CurrentTank].Pos,tankControlers[selectedOponent].Pos)<=tankControlers[CurrentTank].TankRange)
        {
            //In range to attack
            //instant Attack
            OpenConfimationPanel("Are you sure you want to attack","Cost: "+((int)DamageAmountSlider.value*AttackCost).ToString(),1);
            
        }
        else
        {
            //Not in range to attack
            AlertMenuOpen("Player Out of Range","OK");
        }
    }
    public void UpgradeButtonPressed()
    {
        if(UpgradeEnabled)
        {
            if(tankControlers[CurrentTank].TankRange<MaxRange||MaxRange==-1)
            {
                if(tankControlers[CurrentTank].ActionPoints>=UpgradeRangeCost)
                {
                    OpenConfimationPanel("Are you sure you want to upgrade","Cost: "+UpgradeRangeCost.ToString(),3);
                }
                else
                {
                    AlertMenuOpen("Can't afford Upgrade","OK");
                }
            }
            else
            {
                AlertMenuOpen("You are at Max Range","Ok");
            }
        }
        else
        {
            AlertMenuOpen("Upgrading Range is Not Enabled","OK");
        }
        
    }
    public void DonateButtonPressed()
    {
        if(TradingEnabled)
        {
            OpenConfimationPanel("Are you sure you want to Trade","Cost: "+((int)TradingAmountSlider.value*AttackCost).ToString(),2);
        }
        else
        {
            AlertMenuOpen("Trading is disabled","OK");
        }
        
    }
    int DistanceBetweenPos(Vector2Int a,Vector2Int b)
    {
        int distance=Mathf.Max(Mathf.Abs(a.y-b.y),Mathf.Abs(a.x-b.x));

        return distance;
    }
    public void CloseAlertMenu()
    {
        AlertPannelOpen=false;
        AlertPannel.SetActive(false);
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

        }
        if(Input.GetKeyDown(KeyCode.E)&&!inTextField)
        {
            CurrentTank+=1;
            if(CurrentTank>tankControlers.Count-1)
            {
                tileIsSelected=false;
                CurrentTank=0;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(AlertPannelOpen)
            {
                CloseAlertMenu();
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(OponentMenuOpen)
            {
                OponentMenuOpen=false;
            }
            if(YouPannelOpen)
            {
                YouPannelOpen=false;
            }
            if(AlertPannelOpen)
            {
                CloseAlertMenu();
            }
        }
        //Click
        if(Input.GetKeyDown(KeyCode.Mouse0)&&!ConfirmationPanelOpen&&!OponentMenuOpen&&!AlertPannelOpen&&!YouPannelOpen)
        {
            if(GetMousePos()==SelectedTile&&tileIsSelected)
            {
                if(tm.GetTile(new Vector3Int(SelectedTile.x,SelectedTile.y,0))==EmptyTile)
                {
                    if(tankControlers[CurrentTank].ActionPoints>=CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost))
                    {
                        OpenConfimationPanel("Are you sure you want to move","Cost: "+CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost).ToString(),0);
                    }
                    else
                    {
                        AlertMenuOpen("You can't afford to move here\nCost: "+CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost).ToString()+" You Have: "+tankControlers[CurrentTank].ActionPoints.ToString(),"OK");
                    }
                }
                else 
                {
                    int x=0;
                    while (x<tankControlers.Count)
                    {
                        if(tankControlers[x].Pos==SelectedTile)
                        {
                            if(x!=CurrentTank)
                            {
                                selectedOponent=x;
                                OponentMenuOpen=true;
                            }
                            else
                            {
                                YouPannelOpen=true;   
                            }
                            
                            
                            
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
