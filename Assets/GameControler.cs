using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class GameControler : MonoBehaviour
{
    public static GameControler Instance;
    int indexItem = 0;
    public int indexLevel = 0, countPeriod = 0;
    float _value = 0;
    public List<ItemControler> _LstItem;
    [SerializeField]ItemControler itemTarget;
    [SerializeField] Level levelCurrent;
    [SerializeField] ItemControler itemPrefab;
    [SerializeField] Transform conten;
    [SerializeField] Slider process;
    [SerializeField] Level[] level;
    [SerializeField] TMP_Text txtLevel;
    [SerializeField] GameObject _handle;
    [SerializeField]bool isComple = false;
    [SerializeField] GameObject EndGame;
    Action onAction;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
        UpdateProcess();
        onAction = SpawnItem;
    }
    public void GameStart()
    {
        indexLevel = 0;
        StartLevelCurr(true);
    }
    public void StartLevelCurr(bool isNextLevel)
    {
        isComple = false;
        if (isNextLevel)
            countPeriod = 0; // khi next level chu ki của thanh slide reset ve 0
        levelCurrent = level[indexLevel];
        SpawnItem();
        txtLevel.text = "Level " + (indexLevel + 1).ToString();
        ResetItem();
    }
    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (AllConditionDone())
                return;

            if (itemTarget.direction == Direction.Right)
            {
                itemTarget.OnSelectItem();

            }
            else
            {
                ResetItem();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (AllConditionDone())
                return;

            if (itemTarget.direction == Direction.Left)
                itemTarget.OnSelectItem();
            else
            {
                ResetItem();
                return;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (AllConditionDone())
                return;

            if ( itemTarget.direction == Direction.Up)
                itemTarget.OnSelectItem();
            else
            {
                ResetItem();
                return;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (AllConditionDone())
                return;

            if (itemTarget.direction == Direction.Down)
                itemTarget.OnSelectItem();
            else
            {
                ResetItem();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (AllConditionDone())
            {
                if (!isComple)
                {
                    Debug.Log("Good");
                    isComple = true;
                }
                
            }
            else
            {
                ResetItem();
                return;
            }
        }
    }
 
    public void SpawnItem()
    {
        foreach (Transform item in conten) { Destroy(item.gameObject);}
        _LstItem.Clear();
        int count = levelCurrent.countItem;
        for (int i = 0; i < count; i++)
        {
            ItemControler item= Instantiate(itemPrefab, conten);
            item.Init();
            _LstItem.Add(item);
        }
       
    }
    public void ChooseItemTarget()
    {
        indexItem++;
        if (indexItem <= _LstItem.Count-1)
            itemTarget = _LstItem[indexItem];
        else
        {
            return;
        }
    }
    public void ResetItem()
    {
        indexItem = 0;
        for (int i = 0; i < _LstItem.Count; i++)
            _LstItem[i].ResetItem();
        itemTarget = _LstItem[indexItem];
    }
    public void UpdateProcess()
    {
        _handle.SetActive(true);
        process.value = _value;
        DOTween.To(() => _value, x => _value = x, process.maxValue, 3)
            .OnUpdate(() =>
            {
                process.value = _value;
            })
            .OnComplete(() =>
            {
                if (!isComple)
                {
                    Debug.Log("miss");
                }
                _handle.SetActive(false);
                process.value = 0;
                _value = 0;
                countPeriod++;
                StartCoroutine(waitTime())
 ;               


            });
    }
    IEnumerator waitTime()
    {
        yield return new WaitForEndOfFrame();
        if (countPeriod == levelCurrent.conditionNextLevel)
        {
            indexLevel++;
            if (indexLevel <= level.Length - 1)
            {
                StartLevelCurr(true);
            }
            else
            {
                EndGame.SetActive(true );
                yield break;
            }
           
        }
        else
        {
            StartLevelCurr(false);

        }
        UpdateProcess();
    }
    public bool AllConditionDone()
    {
        for (int i = 0; i < _LstItem.Count; i++)
        {
            if (!_LstItem[i].isSelect)
            {
                return false;
            }
        }
        return true;
    }
}
public enum Direction
{
    Left,
    Right,
    Up ,
    Down
}
[System.Serializable]
public class Level
{
    public int countItem;
    public int conditionNextLevel;
}