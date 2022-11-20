using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioCirclesController : MonoSingleton<RadioCirclesController>
{

    [SerializeField] private Material material;

    public bool IsInputEnabled { get; set; } = true;

    private bool IsNeutral
    {
        set
        {
            material.SetInt( nameof(RadioShaderRef._IsNeutral), value ? 1 : 0);
        }
    }

    private bool IsCorrect
    {
        
        set
        {
            material.SetInt(nameof(RadioShaderRef._IsCorrect), value ? 1 : 0);
        }
    }

    private float Angle
    {
        set
        {
            material.SetFloat( nameof(RadioShaderRef._Angle), value);
        }
    }

    public bool CurrentState
    {
        get
        {
            return false;
        }
    }


    private bool _isInputDown;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Update()
    {
        //if 
        IsNeutral = IsInputEnabled && !Input.GetKey(KeyCode.Space);
        IsCorrect = TowerController.Current.CurrentState;

        Angle = TowerController.Current.CurrentAngle;

    }
}
