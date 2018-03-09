using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Break : MonoBehaviour {

    public int rightClickRadius;

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
            Voxel v = World.instance.RaycastVoxel (transform.position, transform.forward);
            World.instance.ChangeVoxel (v, 0);
        }

        if(Input.GetMouseButtonDown(1)) {
            SetWithinClickRadius (0);
        }

        if (Input.GetKeyDown (KeyCode.B)) {
            SetWithinClickRadius (5);
        }
	}

    void SetWithinClickRadius (int id) {
        List<Voxel> voxels = new List<Voxel> ();
        for (int i = -rightClickRadius; i <= rightClickRadius; i++) {
            for (int j = -rightClickRadius; j <= rightClickRadius; j++) {
                for (int k = -rightClickRadius; k <= rightClickRadius; k++) {
                    if (Vector3.Distance (Vector3.zero, new Vector3 (i, j, k)) <= rightClickRadius) {
                        voxels.Add (World.instance.PositionToVoxel (transform.position + new Vector3 (i, j, k)));
                    }
                }
            }
        }
        World.instance.ChangeVoxels (voxels.ToArray (), id);
    }
}
