using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class FX_HeightFog : MonoBehaviour
{
    Dictionary<Camera, CommandBuffer> m_CameraCmdBuffers = new Dictionary<Camera, CommandBuffer>();
    public Color FogColor = Color.white;
    public float FogTransparency = 1;
    public Material HeightFogMaterial;

    void CleanUpCommandBuffers()
    {
        foreach (var cam in m_CameraCmdBuffers)
        {
            if (cam.Key != null)
            {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, cam.Value);
            }
        }
        m_CameraCmdBuffers.Clear();
    }

    void OnDisable()
    {
        CleanUpCommandBuffers();
    }

    void OnDestroy()
    {
        CleanUpCommandBuffers();
    }

    void Update()
    {
        foreach(var cam in Camera.allCameras)
        {
            if (!m_CameraCmdBuffers.ContainsKey(cam))
            {
                // create new command buffers
                var height_fog_cmd_buf = new CommandBuffer();
                height_fog_cmd_buf.name = "Height Fog Effect";
                m_CameraCmdBuffers[cam] = height_fog_cmd_buf;

                cam.AddCommandBuffer(CameraEvent.AfterForwardAlpha, height_fog_cmd_buf);
            }
        }

        foreach (var cam_buf in m_CameraCmdBuffers)
        {
            var cam = cam_buf.Key;
            var height_fog_cmd_buf = cam_buf.Value;
            height_fog_cmd_buf.Clear();

            var view_proj_id = Shader.PropertyToID("ViewProjectionInverse");
            var fog_id = Shader.PropertyToID("HeightFogParameters");
            var fog2_id = Shader.PropertyToID("HeightFogParameters2");

            var view_proj_inv = cam.cameraToWorldMatrix * cam.projectionMatrix.inverse;

            height_fog_cmd_buf.SetGlobalMatrix(view_proj_id, view_proj_inv);
            height_fog_cmd_buf.SetGlobalVector(fog_id, new Vector4(FogColor.r, FogColor.g, FogColor.b, transform.position.y));
            height_fog_cmd_buf.SetGlobalVector(fog2_id, new Vector4(FogTransparency, 0, 0, 0));

            height_fog_cmd_buf.DrawProcedural(Matrix4x4.identity, HeightFogMaterial, 0, MeshTopology.Triangles, 3);
        }
    }
}
