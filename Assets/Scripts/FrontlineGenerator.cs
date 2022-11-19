using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FrontlineGenerator:MonoBehaviour
{
    BattleController _battleController = BattleController.instance;
    private void Start()
    {
        var frontLine = CreateFrontline(_battleController.points);
    }

    public float maxVariation = 0.1f;
    public int numberOfPoints = 10;
    public List<FrontlinePointHolder> CreateFrontline(List<Vector2> basePoints)
    {
        List<FrontlinePointHolder> frontlinePoints = new List<FrontlinePointHolder>();
        List<Vector2> topPoints = null;
        for (int i=0; i<basePoints.Count-1; i++)
        {
            var thisPoint = basePoints[i];
            var nextPoint = basePoints[i + 1];
            FrontlinePointHolder holder = new FrontlinePointHolder();
            holder.point = basePoints[i];
            var bottomPoints =createBorderBetweenTwoPoints(thisPoint, nextPoint);
            holder.associatedPointsBottom = bottomPoints;
            holder.associatedPointsTop = topPoints;
            topPoints = bottomPoints;

        }

        return frontlinePoints;
    }

    public List<Vector2> createBorderBetweenTwoPoints(Vector2 p1, Vector2 p2)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float baseX = Mathf.Lerp(p1.x, p2.x, i/(float)numberOfPoints);
            float baseY = Mathf.Lerp(p1.y, p2.y, i/(float)numberOfPoints);
            float newX = baseX + Random.Range(-maxVariation, maxVariation);
            float newY = baseY + Random.Range(-maxVariation, maxVariation);
            points.Add(new Vector2(newX, newY));
        }

        return points;
    } 
}
