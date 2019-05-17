//=====================================================
// - FileName:      Main.cs
// - Created:       qutong
// - UserName:      2019/05/17 14:58:53
// - Email:         qt.cn@outlook.com
// - Description:   合并蒙皮网格
// - Copyright © 2019 qt. All rights reserved.
//======================================================
using UnityEngine;

namespace CombineSkinnedMesh
{
    public class Main : MonoBehaviour
    {

        void Start()
        {
            MeshTest();
            Init();
        }

        void Update()
        {

        }

        private void Init()
        {
            GameObject bone = Instantiate(Resources.Load<GameObject>(boneName));
            GameObject[] meshs = new GameObject[meshNames.Length];
            for (int i = 0; i < meshs.Length; i++)
            {
                meshs[i] = Resources.Load<GameObject>(meshNames[i]);
            }
            SkinnedMeshCombine.CombineSkinnedMesh(bone, meshs, combineMaterial, CombineMaterial);

            animaton = bone.GetComponent<Animation>();
            animaton.wrapMode = WrapMode.Loop;
            animaton.Play("breath");

            Resources.UnloadUnusedAssets();
        }

        private Material CombineMaterial(SkinnedCombinedData data) {
            Material mat = new Material(Shader.Find("Role/CartoonCombine"));
            mat.SetTexture("_MainTex", data.textures[0]);
            mat.SetTexture("_Tex1", data.textures[1]);
            mat.SetTexture("_Tex2", data.textures[2]);
            mat.SetTexture("_Tex3", data.textures[3]);
            return mat;
        }

        void MeshTest()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4]{
                new Vector3(1, 1, 0),
                 new Vector3(-1, 1, 0),
                 new Vector3(1, -1, 0),
                 new Vector3(-1, -1, 0),
            };

            mesh.vertices = vertices;
            int[] triangles = new int[2 * 3]{
                0, 3, 1, 0, 2, 3
            };
            mesh.triangles = triangles;
           // SkinnedMeshCombiner.ReCalculateUV(mesh, 0);
        }


        public bool combineMaterial = false;

        private string boneName = "Role/04/Prefab/ch_pc_hou";
        private Animation animaton;

        private string[] meshNames = new string[] {
            "Role/04/Prefab/ch_pc_hou_004_tou",
            "Role/04/Prefab/ch_pc_hou_004_shen",
            "Role/04/Prefab/ch_pc_hou_004_shou",
            "Role/04/Prefab/ch_pc_hou_004_jiao",
        };
    }
}
