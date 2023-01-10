using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class TankControler : MonoBehaviour
{
    #region Other Objects
        [Header("Refrences")]
        public Tilemap tm;
        public Tile EmptyTile;
        public Tile TankTile;
        public TextMeshPro StatText;
        public Transform RangeShower;
        public TextMeshPro NameText;
    #endregion

    #region Tank Stats
        [Header("Stats")]
        public bool Dead;
        public string Name;
        public int TankRange;
        public Vector2Int Pos;
        public int Health;
        public int ActionPoints;
        public Color TankColor;
    #endregion
    
    #region Built in Fucrtions
        void Start()
        {
            SetCurrentTileToTank();
        }

        void Update()
        {
            UpdateUI();
            
            SetPosToTile();

            
        }
    #endregion
    
    #region Public Func
        public void Heal(int Amount)
        {
            Health+=Amount;
        }
        public void NextRound(int ActionPointsGained)
        {
            if(!Dead)
            {
                ActionPoints+=ActionPointsGained;
            }
        }
        public void Move(Vector2Int NewPos,int ActionPointCost)
        {
            tm.SetTile(new Vector3Int(Pos.x,Pos.y,0),EmptyTile);
            Pos=NewPos;
            ActionPoints-=ActionPointCost;
            SetCurrentTileToTank();
        }
        public void Attack(int AttacksDone,int AttackCose)
        {
            ActionPoints-=AttacksDone*AttackCose;
        }
        public void UpgradeRange(int UpgradeCost)
        {
            ActionPoints-=UpgradeCost;
            TankRange++;
        }
        public void Donate(int Cost)
        {
            ActionPoints-=Cost;
        }
        public void ReciveActionPoints(int ActionpointsRecieved)
        {
            ActionPoints+=ActionpointsRecieved;
        }
        public void BeAttack(int HealthLost)
        {
            Health-=HealthLost;
            DeathCheck();
        }
    #endregion
    
    #region Other Func
        void UpdateUI()
        {
            StatText.text=ActionPoints.ToString()+":"+Health.ToString();
            RangeShower.localScale=new Vector3(1+2*TankRange,1+2*TankRange,1);
            RangeShower.GetComponent<SpriteRenderer>().color=new Color(TankColor.r,TankColor.g,TankColor.b,0.5f);
            NameText.text=Name;
            NameText.color=TankColor;
        }
        void DeathCheck()
        {
            if(Health<=0)
            {
                Die();
            }
        }
        void Die()
        {
            Dead=true;            
        }
    
        void SetCurrentTileToTank()
        {
            tm.SetTile(new Vector3Int(Pos.x,Pos.y,0),TankTile);
        }
        void SetPosToTile()
        {
            transform.position=tm.CellToWorld(new Vector3Int(Pos.x,Pos.y,0));
        }
    #endregion
    
    
}
