//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEditor;

//////////////////////////////////////////////////////////////////////

public static class Util
{
    //////////////////////////////////////////////////////////////////////

    public static T Create<T>() where T : MonoBehaviour
    {
        var go = new GameObject();
        T obj = go.AddComponent<T>();   // this is when Awake() is called...
        go.name = "New " + obj.GetType().Name;
        return obj;
    }

    //////////////////////////////////////////////////////////////////////

    public static bool DestroyComponent<T>(this Component obj)
    {
        var component = obj.GetComponent(typeof(T));
        if (component != null)
        {
            UnityEngine.Object.Destroy(component);
        }
        return component != null;
    }

    //////////////////////////////////////////////////////////////////////

    public static string CreateAssetFolder(string parent, string name)
    {
        string guid = AssetDatabase.AssetPathToGUID(parent);            // if parent exists
        if (guid.Length > 0)
        {
            string newPath = parent + "/" + name;
            guid = AssetDatabase.AssetPathToGUID(newPath);              // and new folder doesn't
            if (guid.Length == 0)
            {
                guid = AssetDatabase.CreateFolder(parent, name);        // create it
            }
            return AssetDatabase.GUIDToAssetPath(guid);                 // return the path
        }
        return "";                                                      // or "" if it couldn't (parent didn't exist)
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

    public static void SetParent(this Transform child, Transform parent)
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
