using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioCirclesController : MonoSingleton<RadioCirclesController>
{

    [SerializeField] private Material material;

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

    // Start is called before the first frame update
    void Awake()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
       // IsNeutral = !Input.GetKeyDown(KeyCode.Space);
        IsCorrect = TowerController.Current.CurrentState;

        Angle = TowerController.Current.CurrentAngle;

    }

    private void FixedUpdate()
    {
        // = TowerController.Current.CurrentState; 
    }
}
