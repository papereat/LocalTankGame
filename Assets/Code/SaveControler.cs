using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveControler : MonoBehaviour
{
    public static SaveControler current;
    public string file = "Test.text";

    void Awake()
    {
        current=this;
    }

    public void SaveGameData()
    {
        GameSaveData GSD = CreateGameSaveData();
        Save(GSD,file);
    }
    public void LoadGameData()
    {

    }
    GameSaveData CreateGameSaveData()
    {
        GameSaveData GSD = new GameSaveData();

        #region info Save Data
            //Info Save Data
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
        #endregion

        #region Tank Save Data
            
        #endregion

        return GSD;
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
