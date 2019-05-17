//=====================================================
// - FileName:      SkinnedMeshCombine.cs
// - Created:       qutong
// - UserName:      2019/05/17 14:58:53
// - Email:         qt.cn@outlook.com
// - Description:   合并蒙皮网格
// - Copyright © 2019 qt. All rights reserved.
//======================================================

using UnityEngine;

public class SkinnedMeshCombine {


    public static void CombineSkinnedMesh(GameObject skeleton, GameObject[] skinnedMeshObjects, bool combine = false, OnCombineCustomizeMaterial customizeMaterialFunc = null) {
        SkinnedMeshRenderer[] waitCombines = new SkinnedMeshRenderer[skinnedMeshObjects.Length];
        for (int i = 0; i < skinnedMeshObjects.Length; i++)
        {
            waitCombines[i] = skinnedMeshObjects[i].GetComponentInChildren<SkinnedMeshRenderer>();
        }
        CombineSkinnedMesh(skeleton, waitCombines, combine, customizeMaterialFunc);
    }

    public static void CombineSkinnedMesh(GameObject skeleton, SkinnedMeshRenderer[] skinnedMeshRenderers, bool combine = false, OnCombineCustomizeMaterial customizeMaterialFunc = null)
    {
        SkinnedCombinedData data = CollectInfomationFromSkinnedMeshs(skeleton, skinnedMeshRenderers, combine);
        if (null == data)
            return;

        SkinnedMeshRenderer smr = skeleton.AddComponent<SkinnedMeshRenderer>();
        smr.sharedMesh = new Mesh();
        smr.sharedMesh.CombineMeshes(data.combineInstances.ToArray(), combine, false);
        smr.bones = data.bones.ToArray();

        if (combine && null != customizeMaterialFunc)
        {
            smr.material = customizeMaterialFunc(data);
        }
        else {
            smr.materials = data.materials.ToArray();
        }
    }

    private static SkinnedCombinedData CollectInfomationFromSkinnedMeshs(GameObject skeleton, SkinnedMeshRenderer[] skinnedMeshRenderers, bool combine = false)
    {
        if (null == skeleton)
            return null;

        SkinnedCombinedData data = new SkinnedCombinedData();
        data.childs.AddRange(skeleton.GetComponentsInChildren<Transform>(true));

        int index = 0;
        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            if (combine)
            {
                data.textures.Add(smr.sharedMaterials[0].GetTexture("_MainTex") as Texture2D);
            }
            else
                data.materials.AddRange(smr.sharedMaterials);

            int meshCount = smr.sharedMesh.subMeshCount;
            for (int i = 0; i < meshCount; i++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = i;
                if (combine)
                    ReCalculateUV(ci.mesh, index);
                data.combineInstances.Add(ci);
            }

            int bonesCount = smr.bones.Length;
            for (int i = 0; i < bonesCount; i++)
            {
                int childCount = data.childs.Count;
                for (int j = 0; j < childCount; j++)
                {
                    if (smr.bones[i].name.Equals(data.childs[j].name))
                    {
                        data.bones.Add(data.childs[j]);
                        break;
                    }
                }
            }
            index++;
        }

        return data;
    }

    public static void ReCalculateUV(Mesh mesh, int uvOffsetX)
    {
        if (uvOffsetX >= 0)
        {
            Vector2[] uv = mesh.uv;
            int count = mesh.uv.Length;
            for (int i = 0; i < count; ++i)
            {
                Vector2 tmp = uv[i];
                tmp.x = tmp.x - Mathf.Floor(tmp.x);
                tmp.x += uvOffsetX;
                tmp.y = tmp.y - Mathf.Floor(tmp.y);
                uv[i] = tmp;
            }
            mesh.uv = uv;
        }
    }
}

public delegate Material OnCombineCustomizeMaterial(SkinnedCombinedData data);
