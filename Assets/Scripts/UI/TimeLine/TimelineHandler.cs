﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TimelineHandler : MonoBehaviour
{   
    private bool m_isInit;

    public float timelineDuration;

    public GameObject timeLineDotContainer;
    public GameObject dotPrefab;

    public GameObject scrollView;

    public Vector2 offsetSpawn;

    public List<GameObject> dotPrefabList;

    private bool isMouseOnTimeLine;

    private bool canStart;

    private float currentTime;

    public TextMeshProUGUI timeText;

    private AlgoAction lastDirection;

    public bool IsMouseOnTimeLine{
        get{return isMouseOnTimeLine;}
        set{
            isMouseOnTimeLine = value;
        }
    }

    public bool stopScrollDown;
    private int amountOfDotScrolledDown = 0;
    public void Open()
    {
        timelineDuration = GameManager.Instance.levelChucks;
        if(!m_isInit) Init();
        canStart = true;
    }

    public void Init()
    {
        m_isInit = true;
        SetupTimelineDots();
        //se brancher sur l'event de leo
        SpaceWheel.Instance.eventSequenceEnds.AddListener(
            ()=>{
                MoveTimelineDown();
            }
        );
    }

    public void Close()
    {
        canStart = false;
    }

    public void Reset()
    {

    }

    private void Update() 
    {
        if(canStart)
        {
            currentTime += Time.deltaTime;
            timeText.text = Utility.TransformFloatToMMssmmTime(currentTime);
        }   
    }

    public void SetupTimelineDots()
    {
        for(int i = 0; i < timelineDuration; i++)
        {
            GameObject go = Instantiate(dotPrefab, transform.position, Quaternion.identity);
            go.GetComponent<RectTransform>().position = new Vector3(0f + offsetSpawn.x, i * go.GetComponent<RectTransform>().rect.height + offsetSpawn.y);
            go.transform.SetParent(timeLineDotContainer.transform);
            dotPrefabList.Add(go);
            go.GetComponent<TimelineDot>().number.text = (i).ToString();
        }

        scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(
            scrollView.GetComponent<RectTransform>().sizeDelta.x, 
            dotPrefabList.Count * dotPrefabList[0].GetComponent<RectTransform>().rect.height);
    }

    private void MoveTimelineDown()
    {
        timeLineDotContainer.transform.DOMoveY(-dotPrefab.GetComponent<RectTransform>().rect.height * amountOfDotScrolledDown , 1.5f).OnComplete(
            ()=>{
                if(!stopScrollDown)
                {
                    if(amountOfDotScrolledDown < timelineDuration)
                    {
                        amountOfDotScrolledDown++;
                        ReadAction();
                    }
                }
            }
        );
    }

    private void ReadAction()
    {
        Debug.Log("READ ACTION");
        dotPrefabList[amountOfDotScrolledDown].GetComponent<Image>().color = Color.red;

        List<AlgoAction> algoActionList = dotPrefabList[amountOfDotScrolledDown].GetComponent<TimelineDot>().algoActionList;
        AlgoAction direction = algoActionList.Find(x => x.m_actionData.actionType == Action.ActionType.Direction);
        if(direction != null)
            lastDirection = direction;

        Debug.Log("lastDirection is "+lastDirection.m_actionData.actionName);

        AlgoAction action = algoActionList.Find(x => x.m_actionData.actionType == Action.ActionType.Action);

        if(action != null)
        {
            Debug.Log("target red action is "+action.m_actionData.actionName);

            switch(action.m_actionData.actionName)
            {
                case InventaireHandler.AlgoActionEnum.Activate :
                break;

                case InventaireHandler.AlgoActionEnum.Shoot :
                    GameManager.Instance.character.GetComponent<ShootRaycast>().ShootKill(lastDirection.m_actionData.actionName);
                break;

                case InventaireHandler.AlgoActionEnum.Reload :
                    GameManager.Instance.character.GetComponent<ShootRaycast>().ReloadingGun();
                break;

                case InventaireHandler.AlgoActionEnum.Walk :
                    Debug.Log("Walk");
                    if(lastDirection.m_actionData.actionName == InventaireHandler.AlgoActionEnum.Up)
                        GameManager.Instance.character.GetComponent<PlayerMov>().Jump();
                    else
                        GameManager.Instance.character.GetComponent<PlayerMov>().MovePlayer(lastDirection.m_actionData.actionName);
                break;
            }
        }
    }


}