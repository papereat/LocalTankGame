using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldCreator : MonoBehaviour
{
    [Header("Refrences")]
    public Tilemap tm;
    public Tile EmptyTile;
    public Tile TankTile;
    [Header("Settings")]
    public GameObject TankPrefab;
    public int StartingHealth;
    public int StartingTankRange;

    public int StartingActionPoints;
    public Vector2Int worldSize;
    
    [SerializeField]
    List<TankControler> tankControlers;

    // Start is called before the first frame update
    void Start()
    {
        CreateTank();
        CreateTank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateTank()
    {
        GameObject tank=Instantiate(TankPrefab);
        TankControler tankControler=tank.GetComponent<TankControler>();

        tankControler.Health=StartingHealth;
        tankControler.ActionPoints=StartingActionPoints;
        tankControler.TankRange=StartingTankRange;

        tankControler.tm=tm;
        tankControler.TankColor=Color.HSVToRGB(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0.5f,1f));

        while (true)
        {
            Vector2Int TestPos=new Vector2Int(Random.Range(-worldSize.x,worldSize.x),Random.Range(-worldSize.y,worldSize.y));
            bool good=true;
            foreach (var item in tankControlers)
            {
                if (item.Pos==TestPos)
                {
                    good=false;
                }
            }
            if(good)
            {
                tankControler.Pos=TestPos;
                break;
            }
        }

        tankControlers.Add(tankControler);
        HostControler.current.tankControlers.Add(tankControler);

    }
    void CreateTilemap()
    {
        tm.BoxFill(new Vector3Int(1,5,0),EmptyTile,0,0,worldSize.x,worldSize.y);
        Debug.Log("test");
    }
}
