//=====================================================
// - FileName:      CaptureDepth.cs
// - Created:       qutong
// - UserName:      2019/05/21 11:34:01
// - Email:         qt.cn@outlook.com
// - Description:   深度图
// - Copyright © 2019 qt. All rights reserved.
//======================================================
using UnityEngine;

public class CaptureDepth : MonoBehaviour {

    void Start()
    {
        Application.targetFrameRate = 30;

        _depthTexture = new RenderTexture(textureSize, textureSize, 16, RenderTextureFormat.ARGB32);
        _depthTexture.filterMode = FilterMode.Bilinear;
        _depthTexture.wrapMode = TextureWrapMode.Clamp;

        _depthCamera = gameObject.AddComponent<Camera>();
        _depthCamera.backgroundColor = Color.black;
        _depthCamera.clearFlags = CameraClearFlags.Color; ;
        _depthCamera.targetTexture = _depthTexture;
        _depthCamera.orthographic = true;
        _depthCamera.cullingMask = layerMask;
        _depthCamera.enabled = false;
        _depthCamera.farClipPlane = 20;
        _depthCamera.nearClipPlane = -20;

        _posToUV = new Matrix4x4();
        _posToUV.SetRow(0, new Vector4(0.5f, 0, 0, 0.5f));
        _posToUV.SetRow(1, new Vector4(0, 0.5f, 0, 0.5f));
        _posToUV.SetRow(2, new Vector4(0, 0, 1, 0));
        _posToUV.SetRow(3, new Vector4(0, 0, 0, 1));

        Shader.SetGlobalTexture("_LightSpaceDepthTexture", _depthTexture);
        Shader.SetGlobalFloat("_ShadowMapTexmapScale", 1f / textureSize);
    }

    void Update()
    {
        if (_depthSampleShader == null)
            _depthSampleShader = Shader.Find("ShadowMap/DepthTextureShader");

        if (_depthCamera != null)
            _depthCamera.RenderWithShader(_depthSampleShader, "RenderType");

        Matrix4x4 world2LightSpace = GL.GetGPUProjectionMatrix(_depthCamera.projectionMatrix, false) * _depthCamera.worldToCameraMatrix;
        Shader.SetGlobalMatrix("_World2LightSpace", world2LightSpace);
        Shader.SetGlobalMatrix("_World2LightSpace2UV", _posToUV * world2LightSpace);

        Shader.SetGlobalFloat("_bias", 0.0005f);
        Shader.SetGlobalFloat("_ShadowIntensity", 1-shadowIntensity);
    }

    void OnGUI()
    {
        if (_depthTexture != null)
            GUI.DrawTextureWithTexCoords(new Rect(0, 20, 150, 150), _depthTexture, new Rect(0, 0, 1, 1), false);

        if (GUI.Button(new Rect(0, 200, 100, 50), "HARD"))
        {
            ChangeShowType(ShadowType.HARD);
        }

        if (GUI.Button(new Rect(0, 250, 100, 50), "SOFT_PCF4x4"))
        {
            ChangeShowType(ShadowType.SOFT_PCF4x4);
        }
    }

    private void ChangeShowType(ShadowType type)
    {
        if (type == ShadowType.HARD)
        {
            Shader.EnableKeyword("HARD_SHADOW");
            Shader.DisableKeyword("SOFT_SHADOW_PCF4x4");
        }
        if (type == ShadowType.SOFT_PCF4x4)
        {
            Shader.DisableKeyword("HARD_SHADOW");
            Shader.EnableKeyword("SOFT_SHADOW_PCF4x4");
        }
    }

    public int textureSize;
    [Range(0.00001f, 1f)]
    public float shadowIntensity;
    public LayerMask layerMask;

    private Camera _depthCamera;
    private Shader _depthSampleShader;
    private RenderTexture _depthTexture;
    private Matrix4x4 _posToUV;
    private ShadowType _shadowType;

    private enum ShadowType
    {
        HARD,
        SOFT_PCF4x4,
    }
}
