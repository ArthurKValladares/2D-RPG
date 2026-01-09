using UnityEngine;
using System;

[Serializable]
public class Stat<T>
{
    [SerializeField] private T baseValue;

    public T GetValue() {
        return baseValue;
    }
}
