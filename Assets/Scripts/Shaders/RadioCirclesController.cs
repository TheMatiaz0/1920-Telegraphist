using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioCirclesController : MonoSingleton<RadioCirclesController>
{

    [SerializeField] private Material material;

    private bool IsInputDown
    {
        get { return _isInputDown; }
        set
        {
            material.SetInt( nameof(RadioShaderRef._IsCorrect),value ? 1 : 0);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isInputDown = true;
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isInputDown = false;
            }
        }


    }

    private void FixedUpdate()
    {
        IsInputDown = TowerController.Current.CurrentState; 
    }
}
