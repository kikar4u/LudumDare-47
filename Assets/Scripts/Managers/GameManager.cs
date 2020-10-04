﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public GameObject currentGameObjectDragged;

    void Start()
    {
        Init();
    }

    void Init()
    {
        SpaceWheel.Instance.Init();
        //SpaceWheel.Instance.levelToLoad.listSections.Count;
        UIManager.Instance.Init();
    }

}