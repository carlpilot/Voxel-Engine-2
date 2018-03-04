using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Break : MonoBehaviour {

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
            Voxel v = World.instance.RaycastVoxel (transform.position, transform.forward);
            if (v != null) {
                v.id = 0;
                v.parent.Render ();
            }
        }
	}
}
