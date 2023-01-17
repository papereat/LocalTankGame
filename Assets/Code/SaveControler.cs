using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveControler : MonoBehaviour
{
    public string SavedName;
    public GameObject TankPrefab;
    public Tilemap tm;
    public static SaveControler current;
    public string file = "Test.text";

    void Awake()
    {
        current=this;
    }
    void Start()
    {
        
        if(SceneTransferScript.sceneTransferScript.LoadSaveGame)
        {
            LoadGameData(SceneTransferScript.sceneTransferScript.SaveName);
        }
    }

    public void SaveGameData(string SaveName)
    {
        GameSaveData GSD = CreateGameSaveData(SaveName);
        Save(GSD,"Game Save/"+SaveName+".json");
        SavedName=SaveName;
    }
    public void LoadGameData(string SaveName)
    {
        GameSaveData GSD=new GameSaveData();
        Debug.Log("Game Save/"+SaveName);
        Load(GSD,"Game Save/"+SaveName);

        StartGameData(GSD);

    }
    public void SaveSettingData(GameSaveData GSD)
    {
        Save(GSD,"Setting Save Data.json");
    }
    public GameSaveData LoadSettingData()
    {
        GameSaveData GSD=new GameSaveData();
        Load(GSD,"Setting Save Data.json");

        return GSD;
    }
    public void SaveCamData(CameraSaveData CSD)
    {
        Save(CSD,"Camera Save Data.json");
    }
    public CameraSaveData LoadCamData()
    {
        CameraSaveData CSD=new CameraSaveData();
        Load(CSD,"Camera Save Data.json");

        return CSD;
    }
    void StartGameData(GameSaveData GSD)
    {
       
        SavedName=GSD.Name;
        #region Host Save Data
            HostControler.current.TradingFine=GSD.TradingFine;
            HostControler.current.ActionPointsGainedPerRound=GSD.ActionPointsGainedPerRound;
            HostControler.current.TradingEnabled=GSD.TradingEnabled;
            HostControler.current.UpgradeEnabled=GSD.UpgradeEnabled;
            HostControler.current.UpgradeRangeCost=GSD.UpgradeRangeCost;
            HostControler.current.MaxRange=GSD.MaxRange;
            HostControler.current.MovementCost=GSD.MovementCost;
            HostControler.current.AttackCost=GSD.AttackCost;
            HostControler.current.HealthGainedOnKill=GSD.HealthGainedOnKill;
            HostControler.current.ActionPointSyphonPercent=GSD.ActionPointSyphonPercent;
            HostControler.current.GameStarted=true;
            WorldCreator.current.worldSize=GSD.worldSize;
            WorldCreator.current.CreateTilemap();
            WorldCreator.current.SwichUI();
        #endregion

        #region Tank Save Data
            foreach (var item in GSD.Tanks)
            {
                GameObject tankObj=CreateTank(item);
                HostControler.current.tankControlers.Add(tankObj.GetComponent<TankControler>());
            }
            
        #endregion

        HostControler.current.MoveCam();
    }
    GameObject CreateTank(TankSaveData TSD)
    {
        GameObject tankObj=Instantiate(TankPrefab);
        TankControler tc=tankObj.GetComponent<TankControler>();

        tc.Dead=TSD.Dead;
        tc.Name=TSD.Name;
        tc.tm=tm;
        tc.TankRange=TSD.TankRange;
        tc.Pos=TSD.Pos;
        tc.Health=TSD.Health;
        tc.ActionPoints=TSD.ActionPoints;
        tc.TankColor=TSD.TankColor;


        return tankObj; 
    }
    GameSaveData CreateGameSaveData(string SaveName)
    {
        GameSaveData GSD = new GameSaveData();

        #region info Save Data
            GSD.Name=SaveName;
            System.DateTime moment= System.DateTime.Now;
            GSD.Date=moment.Hour.ToString()+":"+moment.Minute.ToString()+":"+moment.Second.ToString()+" "+moment.Day+"/"+moment.Month+"/"+moment.Year;
        #endregion

        #region Host Save Data
            GSD.TradingFine=HostControler.current.TradingFine;
            GSD.ActionPointsGainedPerRound=HostControler.current.ActionPointsGainedPerRound;
            GSD.TradingEnabled=HostControler.current.TradingEnabled;
            GSD.UpgradeEnabled=HostControler.current.UpgradeEnabled;
            GSD.UpgradeRangeCost=HostControler.current.UpgradeRangeCost;
            GSD.MaxRange=HostControler.current.MaxRange;
            GSD.MovementCost=HostControler.current.MovementCost;
            GSD.AttackCost=HostControler.current.AttackCost;
            GSD.HealthGainedOnKill=HostControler.current.HealthGainedOnKill;
            GSD.ActionPointSyphonPercent=HostControler.current.ActionPointSyphonPercent;
            GSD.worldSize=WorldCreator.current.worldSize;
        #endregion

        #region Tank Save Data
            List<TankSaveData> tcs=new List<TankSaveData>();
            foreach (var item in HostControler.current.tankControlers)
            {
                tcs.Add(CreateTankData(item));
            }
            GSD.Tanks=tcs;
        #endregion

        return GSD;
    }
    TankSaveData CreateTankData(TankControler tc)
    {
        TankSaveData TSD=new TankSaveData();

        TSD.Dead=tc.Dead;
        TSD.Name=tc.Name;
        TSD.TankRange=tc.TankRange;
        TSD.Pos=tc.Pos;
        TSD.Health=tc.Health;
        TSD.ActionPoints=tc.ActionPoints;
        TSD.TankColor=tc.TankColor;


        return TSD;
    }
    public void Save(object objectToSave,string fileName)
    {
        string json=JsonUtility.ToJson(objectToSave);
        WriteToFile(fileName,json);
    }
    public void Load(object objectToLoad,string fileName)
    {
        string json = ReadFromFile(fileName);
        JsonUtility.FromJsonOverwrite(json,objectToLoad);
    }
    void WriteToFile(string fileName,string json)
    {
        string path= GetFilePath(fileName);
        FileStream fileStream=new FileStream(path,FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }
    string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if(File.Exists(path))
        {
            using (StreamReader reader= new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("file not found!");
        }
        return "";
    }
    string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
