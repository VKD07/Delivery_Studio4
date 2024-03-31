using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SineEffect : MonoBehaviour
{
    [SerializeField] bool x_Direction;
    [SerializeField] float amplitude = 1f;
    [SerializeField] float frequency = 1f;

    bool isEnabled;
    Vector3 initPos;


    private void Awake()
    {
        initPos = transform.position;
    }

    private void Update()
    {
        SineEffectLoop();
    }

    private void SineEffectLoop()
    {
        if (isEnabled)
        {
            float offset = Mathf.Sin(Time.time * frequency) * amplitude;
            float xPos = x_Direction ? offset : 0f;
            float yPos = !x_Direction ? offset : 0f;
            transform.position = initPos + new Vector3(xPos, yPos, 0f);
        }
    }

    public void EnableEffect(bool val)
    {
        isEnabled = val;

        if (!val)
        {
            transform.position = initPos;
        }
    }
}
