using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class WorldCreator : MonoBehaviour
{
    public static WorldCreator current;
    #region Other Objects
        [Header("Refrences")]
        public Tilemap tm;
        public Tile EmptyTile;
        public Tile TankTile;
        public GameObject TankPrefab;
        public PlayerCustomizerControler PCC;
    #endregion
    
    #region UI Refrences
        public GameObject InGameUI;
        public GameObject SetupUI;
        public TMP_InputField WorldSizeX;
        public TMP_InputField WorldSizeY;
        public TMP_InputField MDFOT;
        public TMP_InputField Health;
        public TMP_InputField AP;
        public TMP_InputField Range;
        public TMP_InputField MovemntCost;
        public TMP_InputField AttackCost;
        public TMP_InputField RangeUpgradeCost;
        public TMP_InputField MaxRange;
        public TMP_InputField APGPR;
        public TMP_InputField HealthGaindPerKill;
        public TMP_InputField DonationFine;
        public Slider ActionPointSyphonPercent;
        public Toggle EnableDonating;
        public Toggle EnableRangeUpgrade;
        public Toggle EnableDeadPlayersDespawning;








    #endregion

    #region Gernation Settings
        [Header("Settings")]
        public int StartingTanks;
        public int StartingHealth;
        public int StartingTankRange;
        public int StartingActionPoints;
        public Vector2Int worldSize;
        public int MinDistFromOtherTanks;
    #endregion
    
    #region Private Var
        List<TankControler> tankControlers=new List<TankControler>();
        int PlayerGenerationRunAttempts=10;
    #endregion


    #region  Built in Func
    void Awake()
    {
        current=this;
    }
    #endregion

    public void SwichUI()
    {
        SetupUI.SetActive(false);
        InGameUI.SetActive(true);
    }
    #region  Object Generator
        public void CreateWorld()
        {
            //Get Variables
            GetVariables();

            //Generate tile map
            CreateTilemap();

            //Create Random tanks
            CreateTanks();

            //Change UI
            SwichUI();

            //Alert Host Controler
            HostControler.current.GameStarted=true;

            //Move Cam
            HostControler.current.MoveCam();
        }
        void CreateTanks()
        {
            foreach (var item in PCC.playerCustomizers)
            {
                CreateTank(item.Name,item.color);
            }
        }
        void GetVariables()
        {
            worldSize=new Vector2Int(int.Parse(WorldSizeX.text),int.Parse(WorldSizeY.text));
            MinDistFromOtherTanks=int.Parse(MDFOT.text);
            StartingHealth=int.Parse(Health.text);
            StartingActionPoints=int.Parse(AP.text);
            StartingTankRange=int.Parse(Range.text);

            HostControler.current.MovementCost=int.Parse(MovemntCost.text);
            HostControler.current.AttackCost=int.Parse(AttackCost.text);
            HostControler.current.UpgradeRangeCost=int.Parse(RangeUpgradeCost.text);
            HostControler.current.MaxRange=int.Parse(MaxRange.text);
            HostControler.current.ActionPointsGainedPerRound=int.Parse(APGPR.text);
            HostControler.current.TradingFine=int.Parse(DonationFine.text);
            HostControler.current.TradingEnabled=EnableDonating.Value;
            HostControler.current.UpgradeEnabled=EnableRangeUpgrade.Value;
            HostControler.current.HealthGainedOnKill=int.Parse(HealthGaindPerKill.text);
            HostControler.current.ActionPointSyphonPercent=ActionPointSyphonPercent.value;
        }
        GameObject BaseTank()
        {
            //Create Object
            GameObject tank=Instantiate(TankPrefab);

            //Get Tank Controler Componenet
            TankControler tankControler=tank.GetComponent<TankControler>();
            
            //Set Basic Variables
            tankControler.Health=StartingHealth;
            tankControler.ActionPoints=StartingActionPoints;
            tankControler.TankRange=StartingTankRange;
            tankControler.tm=tm;

            //Set Random Color
            tankControler.TankColor=Color.HSVToRGB(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,0.75f));

            bool Succes=false;
            //Find Position
            int x=0;
            while (x<PlayerGenerationRunAttempts)
            {
                //Chos Radnom spot
                Vector2Int TestPos=new Vector2Int(Random.Range(-worldSize.x,worldSize.x),Random.Range(-worldSize.y,worldSize.y));
                
                bool good=true;
                foreach (var item in tankControlers)
                {
                    //Tanks Nearby
                    if(Mathf.Abs(item.Pos.x-TestPos.x)<MinDistFromOtherTanks||Mathf.Abs(item.Pos.y-TestPos.y)<MinDistFromOtherTanks)
                    {
                        good=false;
                    }
                }
                //Random Postion not taken
                if(good)
                {
                    tankControler.Pos=TestPos;
                    Succes=true;
                    break;
                }

                x++;
            }
            //Tank Was Unable to Generate
            if(!Succes)
            {
                Destroy(tank);
                return null;
            }

            return tank;
        }
        void CreateTank()
        {
            //Create base tank
            GameObject tank=BaseTank();
            
            //Tank Unable to Generate
            if(tank==null)
            {
                FailedToGeneratePlayer();
                return;
               
            }
            
            //Add to List of Tank Controlers
            tankControlers.Add(tank.GetComponent<TankControler>());
            HostControler.current.tankControlers.Add(tank.GetComponent<TankControler>());
        }

        void CreateTank(string Name,Color color)
        {   
            //Crate Base Tank
            GameObject tank=BaseTank();
            
            //Get Tank controler Component
            

            //Tank Unable to Generate
            if(tank==null)
            {
                FailedToGeneratePlayer();
                return;
            }
            TankControler tankControler=tank.GetComponent<TankControler>();
            
            //Set Name and Color
            tankControler.Name=Name;
            tankControler.TankColor=color;

            //Add to List of Tank Controlers
            tankControlers.Add(tankControler);
            HostControler.current.tankControlers.Add(tankControler);
        }

        public void CreateTilemap()
        {
            Debug.Log("tes");
            for (int x = -worldSize.x; x < worldSize.x; x++)
            {
                for (int y = -worldSize.y; y < worldSize.y; y++)
                {
                    tm.SetTile(new Vector3Int(x,y,0),EmptyTile);
                }
            }
        }
    #endregion

    #region Other func
        void FailedToGeneratePlayer(){}
    #endregion
    
}
