using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FX_HeightFog : MonoBehaviour
{
    CommandBuffer HeightCmdBuf;
    public Color FogColor = Color.white;
    public float FogTransparency = 1;
    public Material HeightFogMaterial;

    // Start is called before the first frame update
    void OnEnable()
    {
        HeightCmdBuf = new CommandBuffer();
        HeightCmdBuf.name = "Height Fog Effect";
        HeightCmdBuf.DrawProcedural(Matrix4x4.identity, HeightFogMaterial, 0, MeshTopology.Triangles, 3);
        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardAlpha, HeightCmdBuf);
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnDisable()
    {
        Camera.main.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, HeightCmdBuf);
    }

    void Update()
    {
        HeightCmdBuf.Clear();

        var view_proj_id = Shader.PropertyToID("ViewProjectionInverse");
        var fog_id = Shader.PropertyToID("HeightFogParameters");
        var fog2_id = Shader.PropertyToID("HeightFogParameters2");

        var view_proj_inv = Camera.main.cameraToWorldMatrix * Camera.main.projectionMatrix.inverse; 


        HeightCmdBuf.SetGlobalMatrix(view_proj_id, view_proj_inv);
        HeightCmdBuf.SetGlobalVector(fog_id, new Vector4(FogColor.r, FogColor.g, FogColor.b, transform.position.y));
        HeightCmdBuf.SetGlobalVector(fog2_id, new Vector4(FogTransparency, 0, 0, 0));

        HeightCmdBuf.DrawProcedural(Matrix4x4.identity, HeightFogMaterial, 0, MeshTopology.Triangles, 3);
    }
}
