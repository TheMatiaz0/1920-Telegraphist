using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;
using DG.Tweening;

public class LightAnim : MonoBehaviour
{
    public Vector2 interval;
    private float timer=0;

    private Light2D light;
    private Animator anim;

    private void Start()
    {
        light = GetComponent<Light2D>();
        anim = GetComponent<Animator>();
        timer = Random.Range(interval.x, interval.y);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = Random.Range(interval.x, interval.y);
            anim.Play("Fade");
        }
    }
}
