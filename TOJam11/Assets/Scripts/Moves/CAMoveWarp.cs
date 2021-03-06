﻿using UnityEngine;
using System.Collections;

public class CAMoveWarp : CarAction {

    public override void Start()
    {
        base.Start();
    }

    override public void Perform(ActionCallback callback)
    {
        base.Perform(callback);
        GetTile();
    }

    public override bool PerformAI(CarAction.ActionCallback callback)
    {
        Perform(callback);
        return true;
    }

    void GetTile()
    {
        GridTile g = car.tile.grid.GetRandom();
        if (g.car != null)
        {
            GetTile();
            return;
        }
        car.tile = g;
        Invoke("EndTurn", Settings.turnDelay);
    }

}
