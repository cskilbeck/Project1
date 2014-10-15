﻿using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    public static Camera cam = CreateCamera();
    public static LayerMask layer = new LayerMask();

    private static Camera CreateCamera()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            GameObject g = GameObject.Instantiate(mainCamera) as GameObject;
            g.name = "2D Camera";
            //g.hideFlags = HideFlags.HideAndDontSave;
            cam = g.camera;
            cam.hdr = false;
            cam.renderingPath = RenderingPath.VertexLit;
            cam.clearFlags = CameraClearFlags.Nothing;
            cam.orthographic = true;
            cam.projectionMatrix = Matrix4x4.Ortho(0, Screen.width, 0, Screen.height, 0, 100);
            cam.DestroyComponent<AudioListener>();
            cam.DestroyComponent<GUILayer>();
            foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
            {
                c.cullingMask &= ~(1 << 31);
            }
            cam.cullingMask = 1 << 31;
            cam.enabled = true;
        }
        else
        {
            Debug.LogError("Need a 'MainCamera' tagged camera to clone from...");
        }
        return cam;
    }

    public static Camera Get()
    {
        return cam;
    }
}
