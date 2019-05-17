//=====================================================
// - FileName:      Main.cs
// - Created:       qutong
// - UserName:      2019/05/17 19:02:49
// - Email:         qt.cn@outlook.com
// - Description:   多场景烘焙lightmap混合
// - Copyright © 2019 qt. All rights reserved.
//======================================================
using UnityEngine;

namespace LightMapBlend
{
    public class Main : MonoBehaviour
    {

        void Start()
        {
            mat = target.GetComponent<Renderer>().material;
        }

        void Update()
        {
            mat.SetFloat("_Factor", Mathf.PingPong(Time.time, 1));
        }

        public Transform target;
        private Material mat;
    }
}