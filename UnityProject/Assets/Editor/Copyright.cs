//=====================================================
// - FileName:      #SCRIPTNAME#.cs
// - Created:       #AuthorName#
// - UserName:      #CreateTime#
// - Email:         #AuthorEmail#
// - Description:   
// - Copyright © 2018 Qu Tong. All rights reserved.
//======================================================
using UnityEngine;
using System.Collections;
using System.IO;

public class Copyright : UnityEditor.AssetModificationProcessor
{
    private const string AuthorName = "qutong";
    private const string AuthorEmail = "qt.cn@outlook.com";

    private const string DateFormat = "yyyy/MM/dd HH:mm:ss";
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = File.ReadAllText(path);
            allText = allText.Replace("Copyright © 2018 Qu Tong. All rights reserved.", "Copyright © 2019 qt. All rights reserved.");
            allText = allText.Replace("#AuthorName#", AuthorName);
            allText = allText.Replace("#AuthorEmail#", AuthorEmail);
            allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString(DateFormat));
            File.WriteAllText(path, allText);
            UnityEditor.AssetDatabase.Refresh();
        }

    }
}