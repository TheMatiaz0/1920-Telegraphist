using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BattlePoint
{
    [HideInInspector]
    public float posX = 100;
    [HideInInspector]
    public float power = 0;
    [HideInInspector]
    public GameObject obj;

    public bool capturable = false;
    public GameObject capturableObj;
    
    public BattlePoint(GameObject o)
    {
        posX = 100;
        power = 0;
        obj = o;
    }

    private bool captured = false;
    public void Move(float m, int i)
    {
        posX += m*power;
        
        Vector3 pos = obj.transform.position;
        pos.x = posX;
        obj.transform.position = pos;

        if (capturable && posX >= capturableObj.transform.position.x && !captured)
        {
            captured = true;
            BattleController.instance.AddCapturedPoints(1);
        } else if (capturable && posX > capturableObj.transform.position.x && captured)
        {
            captured = false;
            BattleController.instance.AddCapturedPoints(-1);
        }
        
        if(posX<=BattleController.instance.losingPosX) BattleController.instance.Lose();
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
    [Header("WIn & Lose")]
    public float losingPosX = 0;
    public List<GameObject> objectsToCapture;
    
    [Header("Points")]
    
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

    
    private int capturedPoints = 0, pointsToCapture=0;
    private bool lost = false, won=false;

    public static BattleController instance;
    private void Awake()
    {
        instance = this;
        pointAmount = battlePoints.Count;
        points = new List<Vector2>();
        float space = 1;
        for (int i = 0; i < pointAmount; i++)
        {
            var obj = Instantiate(pointObj, Vector2.zero, Quaternion.identity, transform);
            battlePoints[i].obj = obj;
            obj.transform.localPosition = new Vector2(0, space * pointAmount / 2 - i * space);
            points.Add(obj.transform.position);
        }

        pointsToCapture = battlePoints.Where(x => x.capturable).Count();
    }

    public void Lose()
    {
        if (lost || won) return;
        lost = true;
        Debug.Log("I LOST!");
    }
    
    public void Win()
    {
        if (lost || won) return;
        won = true;
        Debug.Log("I WON!");
    }
    
    public void AddCapturedPoints(int n)
    {
        capturedPoints += n;
        if (capturedPoints >= pointsToCapture)
        {
            Win();
        }
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

    public void SetClosestPointTo(Vector2 towerPos, float angle)
    {
        float minAng = Single.PositiveInfinity;
        int minIndex = 4;
        int i = 0;
        foreach (var p in points)
        {
            var ang = Vector2.Angle(new Vector2(1, 0),
                p - towerPos);
            if (p.y < towerPos.y) ang = -ang;
            if (Mathf.Abs(angle-ang)  < minAng)
            {
                minAng = Mathf.Abs(angle-ang);
                minIndex = i;
            }

            i++;
        }
        
        //Debug.Log(minIndex);
        selectedPoint = minIndex;
    }
    
    private float timeToAttack = 0;
    [HideInInspector]
    public int selectedPoint = 4;
    [HideInInspector]
    public float averagePointX = 0;
    
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
        float sum = 0;
        foreach (var b in battlePoints)
        {
            b.Move(Time.deltaTime*powerModifier,i);
            b.DecreasePower(powerDecreaseSpeed);
            sum += b.posX;
            points[i] = b.obj.transform.position;
            i++;
        }

        averagePointX = sum / pointAmount;

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

        if (timeToAttack <= 0)
        {
            timeToAttack = Random.Range(enemyAttackInterval.x, enemyAttackInterval.y);
            // enemy attack
            AddPowerAt(Random.Range(0,pointAmount), Random.Range(enemyPower.x,enemyPower.y));
        }
    }
}
