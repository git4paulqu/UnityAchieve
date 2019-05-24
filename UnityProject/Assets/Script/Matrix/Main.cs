//=====================================================
// - FileName:      Main.cs
// - Created:       qutong
// - UserName:      2019/05/22 19:02:48
// - Email:         qt.cn@outlook.com
// - Description:   
// - Copyright © 2019 qt. All rights reserved.
//======================================================
using UnityEngine;

namespace Matrix
{
    public class Main : MonoBehaviour
    {

        void Start()
        {
        }

        void Update()
        {
            object2WorldMatrix = target.transform.localToWorldMatrix;
            world2CameraMatrix = mainCamera.worldToCameraMatrix;
            projectionMatrix = GL.GetGPUProjectionMatrix(mainCamera.projectionMatrix, false);
        }

        void OnGUI() {
            GUIStyle style = new GUIStyle();
            style.normal.background = null;
            style.normal.textColor = Color.black;
            style.fontSize = 20;
            GUILayout.Label("Object2WorldMatrix", style);
            GUILayout.Label(object2WorldMatrix.ToString(), style);
            GUILayout.Space(5);

            GUILayout.Label("World2CameraMatrix", style);
            GUILayout.Label(world2CameraMatrix.ToString(), style);
            GUILayout.Space(5);

            GUILayout.Label("ProjectionMatrix", style);
            GUILayout.Label(projectionMatrix.ToString(), style);

            GUILayout.Space(5);
            Matrix4x4 matrix = projectionMatrix * world2CameraMatrix * object2WorldMatrix;
            GUILayout.Label("Matrix4x4", style);
            GUILayout.Label(matrix.ToString(), style);
            Vector3 clipSpacePos = matrix * target.transform.position;
            GUILayout.Label(clipSpacePos.ToString(), style);
        }


        public GameObject target;
        public Camera mainCamera;

        public Matrix4x4 object2WorldMatrix;
        public Matrix4x4 world2CameraMatrix;
        public Matrix4x4 projectionMatrix;

        private Vector2 pos;
        private Vector2 size = new Vector2(20f, 20f);
    }
}

