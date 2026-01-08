using System;
using UnityEngine;

[Serializable]
public class Stat<T>
{
    [SerializeField] private T baseValue;

    public T GetValue() {
        return baseValue;
    } 
}
