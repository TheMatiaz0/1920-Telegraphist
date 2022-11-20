using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FrontlineGenerator:MonoSingleton<FrontlineGenerator>
{
    BattleController _battleController = BattleController.Current;
    private LineRenderer _lineRenderer;
    

    Vector2 toV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    private void Start()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _battleController = FindObjectOfType<BattleController>();
       refreshFrontlineRenderer();
       
    }
    
    public void movePoint(int index, Vector3 oldPos, Vector3 newPos)
    {
        
        Vector3 diff = newPos - oldPos;
        Debug.Log(diff);
        Debug.Log(index);
        FrontlinePointHolder point = currentFrontline[index];
        if (point.associatedPointsTop!=null)
        for (int i=0; i<point.associatedPointsTop.Count; i++)
        {
            point.associatedPointsTop[i] = point.associatedPointsTop[i] + ((Vector2)diff)*(i/(float)numberOfPoints);
        }
        if (point.associatedPointsBottom!=null)
            for (int i=0; i<point.associatedPointsBottom.Count; i++)
            {
                point.associatedPointsBottom[i] = point.associatedPointsBottom[i] + ((Vector2)diff)*((numberOfPoints-i)/(float)numberOfPoints);
            }

        point.point = point.point + (Vector2)diff;
        refreshFrontLineRendererAfterMove();
    }

    private void refreshFrontLineRendererAfterMove()
    {
        _lineRenderer.SetPositions(flattenFrontlineList(currentFrontline).
            Select(d=>new Vector3(d.x,d.y, 0)).
            ToArray());
        
    }

    private void refreshFrontlineRenderer()
    {
        var x = _battleController.points;
        var frontLine = CreateFrontline(
            x
        );
        var readyFrontline = this.flattenFrontlineList(frontLine);
        _lineRenderer.positionCount = readyFrontline.Count;
        
        _lineRenderer.SetPositions(readyFrontline.
            Select(d=>new Vector3(d.x,d.y, 0)).
            ToArray());
        currentFrontline = frontLine;
    }
    private void Update()
    {
       
    }

    public float maxVariationX = 0.1f;
    public float maxVariationY = 0.1f;
    public int numberOfPoints = 10;

    private Vector2 getWorldPosition(Vector2 point, Vector2 offset)
    {
        return point + offset;
    }

    List<Vector2> merge(List<Vector2> pos, List<Vector2> offsets)
    {
        List<Vector2> result = new List<Vector2>();
        for (int i = 0; i < pos.Count; i++)
        {
            result.Add(getWorldPosition(pos[i], offsets[i]));
        }

        return result;
    }
    public List<Vector2> flattenFrontlineList(List<FrontlinePointHolder> frontline)
    {
        List<Vector2> flatList = new List<Vector2>();
        foreach (var frontlinePointHolder in frontline)
        {
            
            if (frontlinePointHolder.associatedPointsTop != null) 
            {
                flatList.AddRange(merge(frontlinePointHolder.associatedPointsTop, frontlinePointHolder.associatedPointsTopOffset));
              
            }
            flatList.Add(frontlinePointHolder.point);
          
            if (frontlinePointHolder.associatedPointsBottom != null) 
            {
                flatList.AddRange(merge(frontlinePointHolder.associatedPointsBottom, frontlinePointHolder.associatedPointsBottomOffset));

            }
            
        }

        return flatList;
    }

    private List<FrontlinePointHolder> currentFrontline;
    public List<FrontlinePointHolder> CreateFrontline(List<Vector2> basePoints)
    {
        List<FrontlinePointHolder> frontlinePoints = new List<FrontlinePointHolder>();
        Tuple<List<Vector2>,List<Vector2>> topPoints = null;
        for (int i=0; i<basePoints.Count; i++)
        {
            var thisPoint = basePoints[i];
            Vector2 nextPoint = Vector2.zero;
            if (i+1 < basePoints.Count)
                nextPoint = basePoints[i + 1];
            
            FrontlinePointHolder holder = new FrontlinePointHolder();
            holder.point = basePoints[i];
            Tuple<List<Vector2>,List<Vector2>> bottomPoints = new Tuple<List<Vector2>,List<Vector2>>(new List<Vector2>(), new List<Vector2>());
            if (nextPoint != Vector2.zero)
            {
                bottomPoints = createBorderBetweenTwoPoints(thisPoint, nextPoint);
                holder.associatedPointsBottom = bottomPoints.Item1;
                holder.associatedPointsBottomOffset = bottomPoints.Item2;
            }

            holder.associatedPointsTop = topPoints!=null?topPoints.Item1:null;
            holder.associatedPointsTopOffset =  topPoints!=null?topPoints.Item2:null;
            topPoints = bottomPoints;
            frontlinePoints.Add(holder);
        }
       
        return frontlinePoints;
    }

    public Tuple<List<Vector2>,List<Vector2>> createBorderBetweenTwoPoints(Vector2 p1, Vector2 p2)
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector2> pointsOffsets = new List<Vector2>();
        for (int i = 0; i < numberOfPoints; i++)
        {
            float baseX = Mathf.Lerp(p1.x, p2.x, i/(float)numberOfPoints);
            float baseY = Mathf.Lerp(p1.y, p2.y, i/(float)numberOfPoints);
            float newX = Random.Range(-maxVariationX, maxVariationX);
            float newY = Random.Range(-maxVariationY, maxVariationY);
            points.Add(new Vector2(baseX, baseY));
            pointsOffsets.Add(new Vector2(newX, newY));
        }

        return new Tuple<List<Vector2>, List<Vector2>>(points, pointsOffsets);
    } 
}
