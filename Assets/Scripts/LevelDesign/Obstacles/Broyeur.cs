﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broyeur : Obstacle
{
    // Start is called before the first frame update
    void Start()
    {
        SpaceWheel.Instance.eventSequenceEnds.AddListener(BroyerPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LoadScriptobstacle()
    {
        base.LoadScriptobstacle();
    }

    private void BroyerPlayer()
    {
        PlayerMov player;
        if(CheckForPlayer(out player))
        {
            GameManager.Instance.GameOver();
        }
    }
}
