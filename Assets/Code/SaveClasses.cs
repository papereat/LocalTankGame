using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GameSaveData
{
    #region Info Save Data
        public string Name;
        public string Date;

    #endregion
    #region Host Save Data
        public Vector2Int worldSize;
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
        
    #endregion
    #region  Tank Save Data
        public List<TankSaveData> Tanks;
    #endregion
}
[System.Serializable]
public class TankSaveData
{
        public bool Dead;
        public string Name;
        public int TankRange;
        public Vector2Int Pos;
        public int Health;
        public int ActionPoints;
        public Color TankColor;
}
[System.Serializable]
public class CameraSaveData
{
        public float BaseCameraSpeed;
        public float CameraZoomSensitivity;
}