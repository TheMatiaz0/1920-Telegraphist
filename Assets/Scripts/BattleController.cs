using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BattlePoint
{
    public float posX = 0;
    public float power = 0;
    [HideInInspector]
    public GameObject obj;
    
    public BattlePoint(GameObject o)
    {
        posX = 0;
        power = 0;
        obj = o;
    }
    public void Move(float m)
    {
        posX += m*power;
        
        Vector3 pos = obj.transform.position;
        pos.x = posX;
        obj.transform.position = pos;
    }

    public void DecreasePower(float p)
    {
        power = Mathf.Lerp(power, 0, p);
        //if (power < 0) power += p;
        //else power -= p;
    }
}

public class BattleController : MonoBehaviour
{
    public List<BattlePoint> battlePoints;
    public Vector2 enemyAttackInterval = Vector2.zero;
    public float powerModifier = 1;
    public float powerDecreaseSpeed = 0.2f;
    public Vector2 enemyPower = Vector2.zero;
    public Vector2 myPower = Vector2.zero;

    public GameObject pointObj;
    private int pointAmount = 10;
    [HideInInspector]
    public List<Vector2> points;

    public static BattleController instance;
    private void Awake()
    {
        instance = this;
    }

    public void AddPowerAt(int i, float pow, int affectNeighbors = 3, float affectingModifier= 2)
    {
        battlePoints[i].power += pow;
        for (int j = i+1; j < i + affectNeighbors && j < pointAmount; j++)
        {
            if((battlePoints[j].posX > battlePoints[i].posX && pow<=0)
               || (battlePoints[j].posX < battlePoints[i].posX && pow>0))
                battlePoints[j].power += pow / ((j-i) * affectingModifier);
            
        }
        
        for (int j = i-1; j >= i - affectNeighbors && j >= 0; j--)
        {
            if((battlePoints[j].posX  > battlePoints[i].posX&& pow<=0)
               || (battlePoints[j].posX < battlePoints[i].posX && pow>0)) 
                battlePoints[j].power += pow / ((i-j) * affectingModifier);
        }
    }

    public void GoodClick()
    {
        AddPowerAt(selectedPoint,Random.Range(myPower.x,myPower.y));
    }

    public void BadClick()
    {
        AddPowerAt(selectedPoint,-Random.Range(myPower.x,myPower.y));
    }

    private void Start()
    {
        pointAmount = battlePoints.Count;
        points = new List<Vector2>();
        float space = 1;
        for (int i = 0; i < pointAmount; i++)
        {
            var obj = Instantiate(pointObj, new Vector3(0, space*pointAmount/2 - i*space, 0), Quaternion.identity, transform);
            battlePoints[i].obj = obj;
            points.Add(new Vector3(0, space*pointAmount/2 - i*space, 0));
        }
    }

    private float timeToAttack = 0;
    private int selectedPoint = 4;
    
    private float s = 0;
    public GameObject obbb;
    private bool bbb = false;
    void Update()
    {
        if (bbb)
        {
            s += Time.deltaTime*3;
            if (s >= 2) bbb = false;
        }
        else
        {
            s -= Time.deltaTime*3;
            if (s <=0) bbb = true;
        }

        obbb.transform.localScale = new Vector3(s, s, s);
        
        timeToAttack -= Time.deltaTime;
        int i = 0;
        foreach (var b in battlePoints)
        {
            b.Move(Time.deltaTime*powerModifier);
            b.DecreasePower(powerDecreaseSpeed);
            
            points[i] = b.obj.transform.position;
            i++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (s >= 1.7f)
            {
                GoodClick();
                Debug.Log("asdasdasd");
            }
            else
            {
                BadClick();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedPoint++;
            if (selectedPoint >= pointAmount) selectedPoint = pointAmount - 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedPoint--;
            if (selectedPoint < 0) selectedPoint = 0;
        }

        if (timeToAttack <= 0)
        {
            timeToAttack = Random.Range(enemyAttackInterval.x, enemyAttackInterval.y);
            // enemy attack
            AddPowerAt(Random.Range(0,pointAmount), Random.Range(enemyPower.x,enemyPower.y));
        }
    }
}
