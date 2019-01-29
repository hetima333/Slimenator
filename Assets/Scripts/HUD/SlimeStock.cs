using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStock : MonoBehaviour {

    [SerializeField]
    HUDManager _manager;

    public Dictionary<ElementType, int> SlimeStockList
    {
        get
        {
            return _manager.Player.SlimeStock;
        }
    }

    public int GetSlimeAmount()
    {
        int ammount = 0;

        var stock = _manager.Player.SlimeStock;
        foreach (var slime in stock)
        {
            ammount += slime.Value;
        }
        return ammount;
    }
}
