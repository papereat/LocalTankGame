using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizerControler : MonoBehaviour
{
    public Transform ContentObject;
    public GameObject PlayerCustomizerPrefab;

    public List<PlayerCustomizer> playerCustomizers;

    
    public void Add()
    {
        GameObject pc=Instantiate(PlayerCustomizerPrefab,Vector3.zero,new Quaternion(0,0,0,0),ContentObject);

        playerCustomizers.Add(pc.GetComponent<PlayerCustomizer>());
    }

    public void Remove()
    {
        Destroy(playerCustomizers[playerCustomizers.Count-1].gameObject);
        playerCustomizers.RemoveAt(playerCustomizers.Count-1);
    }
}
