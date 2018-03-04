using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Break : MonoBehaviour {

    public int rightClickBreakRadius;

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

            for(int i = -rightClickBreakRadius; i <= rightClickBreakRadius; i++) {
                for (int j = -rightClickBreakRadius; j <= rightClickBreakRadius; j++) {
                    for (int k = -rightClickBreakRadius; k <= rightClickBreakRadius; k++) {
                        if (Vector3.Distance (Vector3.zero, new Vector3 (i, j, k)) <= rightClickBreakRadius) {
                            if (World.instance.PositionToVoxel (pos + new Vector3 (i, j, k)) != null) {
                                World.instance.PositionToVoxel (pos + new Vector3 (i, j, k)).id = 0;
                                if (!chunksToReload.Contains (World.instance.PositionToVoxel (pos + new Vector3 (i, j, k)).parent)) {
                                    chunksToReload.Add (World.instance.PositionToVoxel (pos + new Vector3 (i, j, k)).parent);
                                }
                            }
                        }
                    }
                }
            }

            foreach(Chunk c in chunksToReload) {
                c.Render ();
            }
        }
	}
}
