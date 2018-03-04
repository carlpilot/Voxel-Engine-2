using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel {

    // CHUNK RELATIVE
    public int x, y, z;
    public Chunk parent;
    public int id;

    public Voxel (int _x, int _y, int _z, Chunk _parent, int _id) {
        x = _x;
        y = _y;
        z = _z;
        parent = _parent;
        id = _id;
    }

    /**
	 * 0 : Bottom (Y-)	1 : Top (Y+)
	 * 2 : Left (X-)	3 : Right (X+)
	 * 4 : Back (Z-)	5 : Front (Z+)
	 **/

    public List<int> GetOpenFaces () {
        List<int> faces = new List<int> ();

        if(id == 0) {
            // Don't render faces on air blocks!
            return faces;
        }

        if (y == 0 || parent.voxels[x, y - 1, z].id == 0) {
            // Bottom
            faces.Add (0);
        }
        if (y == World.chunkHeight - 1 || parent.voxels[x, y + 1, z].id == 0) {
            // Top
            faces.Add (1);
        }
        if (x == 0 || parent.voxels[x - 1, y, z].id == 0) {
            // Left
            faces.Add (2);
        }
        if (x == World.chunkWidth - 1 || parent.voxels[x + 1, y, z].id == 0) {
            // Right
            faces.Add (3);
        }
        if (z == 0 || parent.voxels[x, y, z - 1].id == 0) {
            // Back
            faces.Add (4);
        }
        if (z == World.chunkWidth - 1 || parent.voxels[x, y, z + 1].id == 0) {
            // Front
            faces.Add (5);
        }
        return faces;
    }
}
