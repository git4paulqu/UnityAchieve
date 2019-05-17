//=====================================================
// - FileName:      SkinnedMeshCombiner.cs
// - Created:       qutong
// - UserName:      2019/05/17 14:58:53
// - Email:         qt.cn@outlook.com
// - Description:   合并蒙皮网格-数据结构
// - Copyright © 2019 qt. All rights reserved.
//======================================================
using UnityEngine;
using System.Collections.Generic;

public class SkinnedCombinedData
{
    public SkinnedCombinedData()
    {
        childs = new List<Transform>();
        materials = new List<Material>();
        combineInstances = new List<CombineInstance>();
        bones = new List<Transform>();
        textures = new List<Texture2D>();
    }

    public List<Transform> childs { private set; get; }
    public List<Material> materials { private set; get; }
    public List<CombineInstance> combineInstances { private set; get; }
    public List<Transform> bones { private set; get; }
    public List<Texture2D> textures { private set; get; }
}
