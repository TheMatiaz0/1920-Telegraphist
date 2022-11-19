using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private float angle = 0;
    public Vector2 maxAngle = Vector2.zero;
    public float rotationSpeed = 0.4f;
    public GameObject towerToRotate;

    void SetSelectedPoint()
    {
        BattleController.Current.SetClosestPointTo(transform.position, angle);
    }
    
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            angle -= rotationSpeed*Time.deltaTime;
            if (angle < maxAngle.x) angle = maxAngle.x;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            angle += rotationSpeed*Time.deltaTime;
            if (angle > maxAngle.y) angle = maxAngle.y;
        }

        towerToRotate.transform.localEulerAngles = new Vector3(0, 0, angle);
        SetSelectedPoint();

        //Debug.Log(Vector2.Angle(new Vector2(1,0), BattleController.instance.points[4]-(Vector2)transform.position));
    }
    
    public Vector2 GetIntersectionPointCoordinates(float angle, float x)
    {
        float b2 = Mathf.Sqrt(Mathf.Pow(x,2)/Mathf.Pow(Mathf.Cos(angle),2) - Mathf.Pow(x,2));
        return new Vector2(x,b2);
    }
}

