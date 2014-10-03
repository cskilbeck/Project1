//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

//////////////////////////////////////////////////////////////////////

public static class Util
{
    //////////////////////////////////////////////////////////////////////

    public static T Create<T>() where T : MonoBehaviour
    {
        var go = new GameObject();
        T obj = go.AddComponent<T>();   // this is when Awake() is called...
        go.name = obj.GetType().Name;
        return obj;
    }

    //////////////////////////////////////////////////////////////////////

    public static float Ease(float x)
    {
        float x2 = x * x, x3 = x2 * x;
        return 3 * x2 - 2 * x3;
    }

    //////////////////////////////////////////////////////////////////////

    public static Vector2 Lerp(Vector2 a, Vector2 b, float lerp)
    {
        Vector2 c = b - a;
        return a + c * lerp;
    }

    //////////////////////////////////////////////////////////////////////

    public static void SetParent(Transform child, Transform parent)
    {
        Vector3 pos = child.localPosition;
        Quaternion rot = child.localRotation;
        Vector3 scale = child.localScale;
        child.parent = parent;
        child.localScale = scale;
        child.localPosition = pos;
        child.localRotation = rot;
    }
}
