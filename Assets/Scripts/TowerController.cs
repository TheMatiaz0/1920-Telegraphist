using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TowerController : MonoSingleton<TowerController>
{
    private float angle = 0;

    public float CurrentAngle
    {
        get
        {
            return angle+74;
        }
    }

    public Vector2 maxAngle = Vector2.zero;
    public float rotationSpeed = 0.4f;
    public GameObject towerToRotate;

    public Light2D light;
    public MeshRenderer circles;

    public Color lightOkColor;
    public Color lightWrongColor;

    public float okSpeed;
    public float wrongSpeed;

    /// IsCorrectly pressed
    public bool CurrentState
    {
        get
        {
            return _isCorrect;
        }
    }

    private bool _isCorrect = false;

    private void Start()
    {
        SetIsCorrect(false);
    }

    public void SetIsCorrect(bool isCorrect)
    {
        _isCorrect = isCorrect;

       // circles.material.SetFloat("_IsCorrect", isCorrect ? 1 : 0);
       if (light != null)
        {
            DOTween.To(() => light.color, (v) => light.color = v, isCorrect ? lightOkColor : lightWrongColor, 0.5f).SetLink(gameObject);
        }

        // DOVirtual.Float(circles.material.GetFloat("_Speed"), isCorrect ? okSpeed : wrongSpeed, 0.5f,
        //     (v) => circles.material.SetFloat("_Speed", v)).SetLink(gameObject);
    } 

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

