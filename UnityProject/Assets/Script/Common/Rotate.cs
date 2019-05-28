//=====================================================
// - FileName:      Rotate.cs
// - Created:       qutong
// - UserName:      2019/05/28 16:11:53
// - Email:         qt.cn@outlook.com
// - Description:   
// - Copyright © 2019 qt. All rights reserved.
//======================================================
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float rotateSpeed = 30f;

	void Start () {
		
	}
	
	void Update () {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
