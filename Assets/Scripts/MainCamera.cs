using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    public static Camera cam;

    public static Camera Get()
    {
        if (cam == null)
        {
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            if(mainCamera != null)
            {
                GameObject g = GameObject.Instantiate(mainCamera) as GameObject;
                g.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
                cam = g.camera;
                cam.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, 0, Screen.height, 0, 100);
            }
        }
        return cam;
    }
}
