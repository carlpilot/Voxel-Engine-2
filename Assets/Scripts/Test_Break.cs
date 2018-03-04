using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Break : MonoBehaviour {

    public int rightClickRadius;

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
            Voxel v = World.instance.RaycastVoxel (transform.position, transform.forward);
            if (v != null) {
                v.id = 0;
                v.parent.Render ();
            }
        }

        if(Input.GetMouseButtonDown(1)) {
            Voxel v = World.instance.RaycastVoxel (transform.position, transform.forward);
            Vector3 pos = World.instance.VoxelToPosition (v);

            List<Chunk> chunksToReload = new List<Chunk> ();

            for(int i = -rightClickRadius; i <= rightClickRadius; i++) {
                for (int j = -rightClickRadius; j <= rightClickRadius; j++) {
                    for (int k = -rightClickRadius; k <= rightClickRadius; k++) {
                        if (Vector3.Distance (Vector3.zero, new Vector3 (i, j, k)) <= rightClickRadius) {

                            Voxel v2 = World.instance.PositionToVoxel (pos + new Vector3 (i, j, k));

                            if (v2 != null) {
                                v2.id = 0;
                                if (!chunksToReload.Contains (v2.parent)) {
                                    chunksToReload.Add (v2.parent);
                                }
                            }
                        }
                    }
                }
            }

            foreach (Chunk c in chunksToReload) {
                c.Render ();
            }
        }

        if (Input.GetKeyDown (KeyCode.B)) {

            List<Chunk> chunksToReload = new List<Chunk> ();

            for (int i = -rightClickRadius; i <= rightClickRadius; i++) {
                for (int j = -rightClickRadius; j <= rightClickRadius; j++) {
                    for (int k = -rightClickRadius; k <= rightClickRadius; k++) {

                        if (Vector3.Distance (Vector3.zero, new Vector3 (i, j, k)) <= rightClickRadius) {

                            Voxel v = World.instance.PositionToVoxel (transform.position + new Vector3 (i, j, k));

                            if (v != null) {
                                v.id = 4;
                                if (!chunksToReload.Contains (v.parent)) {
                                    chunksToReload.Add (v.parent);
                                }
                            }

                        }
                    }
                }
            }

            foreach (Chunk c in chunksToReload) {
                c.Render ();
            }
        }
	}
}
