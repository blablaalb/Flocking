using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class DrawCamFrustumAlways : MonoBehaviour
{
#if UNITY_EDITOR
    private Camera cam;

    internal void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnDrawGizmos()
    {
        if (cam)
        {
            Gizmos.matrix = cam.transform.localToWorldMatrix;
            Gizmos.DrawFrustum(cam.transform.position,
                cam.fieldOfView,
                cam.farClipPlane,
                cam.nearClipPlane,
                cam.aspect);
        }
    }
#endif
}
