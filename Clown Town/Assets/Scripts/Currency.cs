using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : ScriptableObject
{
    public Sprite icon;
    public float amount { get; private set; }

    public void Add(float x)
    {
        amount += x;
    }

    public void Subtract(float x)
    {
        amount -= x;
    }

    public void Multiply(float x)
    {
        amount *= x;
    }

    public void Divide(float x)
    {
        amount /= x;
    }

    public void Set(float x)
    {
        amount = x;
    }

    public void Setup()
    {
        amount = PlayerPrefs.GetFloat($"{name}_Currency");
    }

    public void Teardown()
    {
        PlayerPrefs.SetFloat($"{name}_Currency", amount);

    }
}
