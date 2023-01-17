using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class HostControler : MonoBehaviour
{
    public static HostControler current;
    
    #region Other Objects
        [Header("refrences")]
        public Tilemap tm;
        Camera mainCamera;
        public Tile EmptyTile;
        public Tile TankTile;
        public List<TankControler> tankControlers;
    #endregion
    
    #region UI Refrences
        public TextMeshProUGUI GameSavedText;
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
        public TextMeshProUGUI SwichBackText;
        public TextMeshProUGUI SwichForwardText;
    #endregion
    
    #region Stats Variables
        [Header("Stats")]
        public int TradingFine;
        public int ActionPointsGainedPerRound;
        public bool TradingEnabled;
        public bool UpgradeEnabled;
        public int UpgradeRangeCost;
        public int MaxRange;
        public int MovementCost;
        public int AttackCost;
        public int HealthGainedOnKill;
        public float ActionPointSyphonPercent;
        public bool DeadPlayersDespawn;
    #endregion

    #region Other Public Stats
        [Header("Camera Stats")]
        public float BaseCameraSpeed;
        public Vector2 MinMaxCameraZoom;
        public float CameraZoomSensitivity;
        [Header("Misc")]
        public bool GameStarted;
    #endregion

    #region Private Var
        [Header("Host Stats")]
        [SerializeField]
        float CameraLerpTime;
        [SerializeField]
        float CameraLerpIncrements;
        [SerializeField]
        float ZoomAmount;
        [SerializeField]
        float StartingCameraZoom;
        Vector2 OldMousePos;
        bool RightMouseDown;
        bool YouPannelOpen;
        bool ConfirmationPanelOpen;
        int ConfirmationPannelType;
        bool AlertPannelOpen;
        bool OponentMenuOpen;
        
        int selectedOponent;
        int CurrentTank=0;
        bool tileIsSelected=false;
        Vector2Int SelectedTile;
        bool inTextField;
    #endregion
    
    #region Built in Func
        void Awake()
        {
            current=this;
            mainCamera=Camera.main;
        }
        void Start()
        {
            mainCamera.orthographicSize=StartingCameraZoom;
            
            CameraSaveData CSD=SaveControler.current.LoadCamData();

            BaseCameraSpeed=CSD.BaseCameraSpeed;
            CameraZoomSensitivity=CSD.CameraZoomSensitivity;
        }

        void Update()
        {
            Inputs();
            UpdateUI();
        }   
    #endregion

    #region UI
        void UpdateUI()
        {
            if(GameStarted)
            {
                #region HUD
                    HealthText.text=tankControlers[CurrentTank].Health.ToString();
                    ActionPointsText.text=tankControlers[CurrentTank].ActionPoints.ToString();
                    NameInput.text=tankControlers[CurrentTank].Name;
                    ColorImage.color=tankControlers[CurrentTank].TankColor;
                #endregion

                #region Selected TIle UI
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
                #endregion
                

                #region CurrentTileUI
                    CurrentTileUI.GetComponent<SpriteRenderer>().color=new Color(tankControlers[CurrentTank].TankColor.r,tankControlers[CurrentTank].TankColor.g,tankControlers[CurrentTank].TankColor.b,0.4f);
                    Vector3 MouseTilPos=tm.CellToWorld(new Vector3Int(GetMousePos().x,GetMousePos().y,0));
                    CurrentTileUI.position=new Vector3(MouseTilPos.x+0.5f,MouseTilPos.y+0.5f,0);
                #endregion
                

                #region You Pannel
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
                #endregion
                

                #region Oponent Menu
                    
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
                        TradingAmountSlider.maxValue=Mathf.Max(tankControlers[CurrentTank].ActionPoints-TradingFine,0);
                        DamageAmountSlider.maxValue=Mathf.Min(tankControlers[selectedOponent].Health, Mathf.FloorToInt(tankControlers[CurrentTank].ActionPoints/AttackCost));
                    }
                    else
                    {
                        OponentMenu.SetActive(false);
                    }
                #endregion
                

                #region Swich Tank button Text
                    int BackIndex=0;
                    int ForwardIndex=0;
                    if(CurrentTank==0)
                    {
                        BackIndex=tankControlers.Count-1;
                    }
                    else
                    {
                        BackIndex=CurrentTank-1;
                    }

                    if(CurrentTank==tankControlers.Count-1)
                    {
                        ForwardIndex=0;
                    }
                    else
                    {
                        ForwardIndex=CurrentTank+1;
                    }

                    while(tankControlers[BackIndex].Dead)
                    {
                        BackIndex--;
                        if(BackIndex==-1)
                        {
                            BackIndex=tankControlers.Count-1;
                        }
                    }
                    while(tankControlers[ForwardIndex].Dead)
                    {
                        ForwardIndex++;
                        if(ForwardIndex==tankControlers.Count)
                        {
                            ForwardIndex=0;
                        }
                    }
                    SwichBackText.text=tankControlers[BackIndex].Name;
                    SwichBackText.color=tankControlers[BackIndex].TankColor;
                    SwichForwardText.text=tankControlers[ForwardIndex].Name;
                    SwichForwardText.color=tankControlers[ForwardIndex].TankColor;
                #endregion
                
                #region Camera Zoom
                    mainCamera.orthographicSize=StartingCameraZoom*ZoomAmount;
                #endregion
            }
            
        }
        
        #region Confirmation Pannel
            void OpenConfimationPanel(string MainText,string SecondaryText,int type)
            {
                ConfirmationObject.SetActive(true);
                ConfirmationPanelOpen=true;
                ConfirmationPannelType=type;
                
                ConfirmationText.text=MainText;
                ConfirmationCostText.text=SecondaryText;
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
                    //Attack
                    if(Accepted)
                    {
                        tankControlers[CurrentTank].Attack((int)DamageAmountSlider.value,AttackCost);
                        tankControlers[selectedOponent].BeAttack((int)DamageAmountSlider.value);

                        //Player Killed
                        if(tankControlers[selectedOponent].Dead)
                        {
                            tankControlers[CurrentTank].Heal(HealthGainedOnKill);
                            tankControlers[CurrentTank].ReciveActionPoints(Mathf.RoundToInt(tankControlers[selectedOponent].ActionPoints*ActionPointSyphonPercent));
                        }
                    }
                }
                else if(ConfirmationPannelType==2)
                {
                    //trade
                    if(Accepted)
                    {
                        tankControlers[CurrentTank].Donate((int)TradingAmountSlider.value+TradingFine);
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
        #endregion
        
        #region Oponent Menu
            public void AttackButtonPressed()
            {
                if(!tankControlers[CurrentTank].Dead)
                {
                    if(DistanceBetweenPos(tankControlers[CurrentTank].Pos,tankControlers[selectedOponent].Pos)<=tankControlers[CurrentTank].TankRange)
                    {
                        OpenConfimationPanel("Are you sure you want to attack","Cost: "+((int)DamageAmountSlider.value*AttackCost).ToString(),1);
                    }
                    else
                    {
                        AlertMenuOpen("Player Out of Range","OK");
                    }
                }
                else
                {
                    AlertMenuOpen("You Dead","F");
                }
                
            }

            public void DonateButtonPressed()
            {
                if(!tankControlers[CurrentTank].Dead)
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
                else
                {
                    AlertMenuOpen("You Dead","F");
                }
            }
        #endregion

        public void UpgradeButtonPressed()
        {
            if(UpgradeEnabled)
            {
                if(!tankControlers[CurrentTank].Dead)
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
                    AlertMenuOpen("You Dead","F");
                }
            }
            else
            {
                AlertMenuOpen("Upgrading Range is Not Enabled","OK");
            }
            
        }
        
        void AlertMenuOpen(string MainText,string ButtonText)
        {
            AlertPannelOpen=true;
            AlertPannel.SetActive(true);
            AlertPannelText.text=MainText;
            AlertPannelButtonText.text=ButtonText;
        }
        public void CloseAlertMenu()
        {
            AlertPannelOpen=false;
            AlertPannel.SetActive(false);
        }
        public void TextSelected()
        {
            inTextField=true;
        }
        public void TextUnselected()
        {
            inTextField=false;
        }
    #endregion
    
    #region Game Mechainics
        public void MoveCam()
        {
            StartCoroutine(CameraLerpToTank(0,CameraLerpIncrements));
        }
        IEnumerator SaveGameText()
        {
            GameSavedText.color= new Color(GameSavedText.color.r,GameSavedText.color.g,GameSavedText.color.b,1);

            float x=0;
            while (true)
            {
                if(x<=1)
                {
                    x+=0.01f;
                }
                else
                {
                    if(GameSavedText.color.a<=0)
                    {
                        break;
                    }
                    else
                    {
                        GameSavedText.color= new Color(GameSavedText.color.r,GameSavedText.color.g,GameSavedText.color.b,GameSavedText.color.a-0.01f);
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        void SaveGame()
        {
            if(SaveControler.current.SavedName!=null)
            {
                SaveControler.current.SaveGameData(SaveControler.current.SavedName);
                Debug.Log(SaveControler.current.SavedName);
            }
            else
            {
                string SaveName=Random.Range(-5000,5000).ToString();
                SaveControler.current.SaveGameData(SaveName);
                Debug.Log(SaveName);
            }
            StartCoroutine(SaveGameText());
        }
        public void Inputs()
        {
            if(GameStarted)
            {
                //Save game on Control+S
                if(Input.GetKey(KeyCode.LeftControl)&&Input.GetKeyDown(KeyCode.S))
                {
                    SaveGame();
                }
                //Yes / Accept Input
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    if(AlertPannelOpen)
                    {
                        CloseAlertMenu();
                    }
                }

                // Exit Menu / no
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
                
                //No Pannels Open or text fields
                if(!ConfirmationPanelOpen&&!OponentMenuOpen&&!AlertPannelOpen&&!YouPannelOpen&&!inTextField)
                {
                    #region Swiching Inputs
                        if(Input.GetKeyDown(KeyCode.Q))
                        {
                            ChangeTank(false);
                        }
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            ChangeTank(true);
                        }
                    #endregion
                    
                    //next Round
                    if(Input.GetKeyDown(KeyCode.Tab))
                    {
                        NextRound();
                    }

                    //Teleport to Current Tank
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        StartCoroutine(CameraLerpToTank(CameraLerpTime,CameraLerpIncrements));
                    }

                    //Left Click
                    if(Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        //Clicked Slected TIle
                        if(GetMousePos()==SelectedTile&&tileIsSelected)
                        {
                            //Tile is Empty
                            if(tm.GetTile(new Vector3Int(SelectedTile.x,SelectedTile.y,0))==EmptyTile)
                            {
                                //Not dead
                                if(!tankControlers[CurrentTank].Dead)
                                {
                                   //Can Afford to Move
                                    if(tankControlers[CurrentTank].ActionPoints>=CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost))
                                    {
                                        OpenConfimationPanel("Are you sure you want to move","Cost: "+CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost).ToString(),0);
                                    }
                                    //Can't Afford to move
                                    else
                                    {
                                        AlertMenuOpen("You can't afford to move here\nCost: "+CheckMovementCost(tankControlers[CurrentTank].Pos,SelectedTile,MovementCost).ToString()+" You Have: "+tankControlers[CurrentTank].ActionPoints.ToString(),"OK");
                                    } 
                                }
                                else
                                {
                                    AlertMenuOpen("You Dead","F");
                                }
                                
                            }
                            //Tile has Tank
                            else 
                            {
                                //Check Who it is
                                int x=0;
                                while (x<tankControlers.Count)
                                {
                                    //selected tank is correct tank
                                    if(tankControlers[x].Pos==SelectedTile)
                                    {
                                        //Selectd tank is Oponent
                                        if(x!=CurrentTank)
                                        {
                                            selectedOponent=x;
                                            OponentMenuOpen=true;
                                            break;
                                        }
                                        //Selectd tasnk is self
                                        else
                                        {
                                            YouPannelOpen=true;  
                                            break; 
                                        }
                                    }

                                    x++;
                                }
                            }
                        }
                        //Clicked unSelcted tile
                        else
                        {
                            //Select TIle
                            SelectedTile=GetMousePos();
                            tileIsSelected=true;
                        }
                    }

                    //Right Mouse Down
                    if(Input.GetKey(KeyCode.Mouse1))
                    {
                        //Right mouse down Last frame
                        if(RightMouseDown)
                        {
                            Vector3 deltaMousePos=mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y))- mainCamera.ScreenToWorldPoint(OldMousePos);
                            OldMousePos= Input.mousePosition;


                            mainCamera.transform.position-=deltaMousePos;
                        }
                        //Right Mouse not down last farme
                        else
                        {
                            RightMouseDown=true;
                            OldMousePos=Input.mousePosition;
                        }
                    }
                    //Right Mouse not down
                    else
                    {
                        RightMouseDown=false;
                    }

                    #region Wasd And Arrow keys
                        Vector2 mov=new Vector2();

                        //Up
                        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
                        {
                            mov.y++;
                        }
                        //Down
                        if(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow))
                        {
                            mov.y--;
                        }
                        //Right
                        if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow))
                        {
                            mov.x++;
                        }
                        //LEft
                        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow))
                        {
                            mov.x--;
                        }

                        mainCamera.GetComponent<Rigidbody2D>().velocity=mov.normalized*BaseCameraSpeed*ZoomAmount;
                    #endregion

                    #region  Camera Zoom
                        //Zoom input 
                        if(Input.mouseScrollDelta!=new Vector2())
                        {
                            Vector2 scroll=Input.mouseScrollDelta;

                            ZoomAmount+=ZoomAmount*scroll.y*CameraZoomSensitivity;
                        }

                        //min Zoom
                        if(ZoomAmount<=MinMaxCameraZoom.x)
                        {
                            ZoomAmount=MinMaxCameraZoom.x;
                        }

                        //max Zoom
                        if(ZoomAmount>=MinMaxCameraZoom.y)
                        {
                            ZoomAmount=MinMaxCameraZoom.y;
                        }
                    #endregion
                }
            }
            
            
        }
        public void ChangeTank(bool ChangeForward)
        {
            if(ChangeForward)
            {
                CurrentTank+=1;
                if(CurrentTank>tankControlers.Count-1)
                {
                    tileIsSelected=false;
                    CurrentTank=0;
                }
                if(!tankControlers[CurrentTank].Dead)
                {
                    StartCoroutine(CameraLerpToTank(CameraLerpTime,CameraLerpIncrements));
                }
                else
                {
                    ChangeTank(true);
                }
                
            }
            else
            {
                CurrentTank-=1;
                if(CurrentTank<0)
                {
                    tileIsSelected=false;
                    CurrentTank=tankControlers.Count-1;
                }
                if(!tankControlers[CurrentTank].Dead)
                {
                    StartCoroutine(CameraLerpToTank(CameraLerpTime,CameraLerpIncrements));
                }
                else
                {
                    ChangeTank(false);
                }
            }
        }
        IEnumerator CameraLerpToTank(float Time,float timeBetweenRuns)
        {
            //Get position of tank aka  place I want camera to go to
            Vector2 TankPosition=tm.CellToWorld(new Vector3Int(tankControlers[CurrentTank].Pos.x,tankControlers[CurrentTank].Pos.y,0));
            Vector3 LerpTo=new Vector3(TankPosition.x,TankPosition.y,mainCamera.transform.position.z);

            //for while loop
            float x=timeBetweenRuns/Time;


            
            //if Time =0 than isntantaly telaprot
            if(Time==0)
            {
                x=1;
            }
            while (true)
            {
                //do the lerp
                mainCamera.transform.position=Vector3.Lerp(mainCamera.transform.position,LerpTo,x);

                //increment x
                x+=timeBetweenRuns/Time;

                //if x greater than 1 stop
                if(x>=1)
                {
                    break;
                }
                
                //wait exactaly timeBetweenRuns seconds than run again from while loop
                yield return new WaitForSeconds(timeBetweenRuns);
            }
        }
        void NextRound()
        {
            foreach (var item in tankControlers)
            {
                item.NextRound(ActionPointsGainedPerRound);
            }
        }

        public void ChangeTankName()
        {
            tankControlers[CurrentTank].Name=NameInput.text;
        }

        #region Calkulations
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
            
            
            int DistanceBetweenPos(Vector2Int a,Vector2Int b)
            {
                int distance=Mathf.Max(Mathf.Abs(a.y-b.y),Mathf.Abs(a.x-b.x));

                return distance;
            }
        #endregion
    #endregion
    

}
