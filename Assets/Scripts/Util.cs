using System;
using UnityEngine;

public static class Util
{
    public static T Create<T>() where T : MonoBehaviour
    {
        var go = new GameObject();
        T obj = go.AddComponent<T>();
        go.name = obj.GetType().Name;
        return obj;
    }
}
