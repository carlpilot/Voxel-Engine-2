using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBuilder : MonoBehaviour {

    public static int n = 16; // Number of cells in one row of the texture atlas

    public static ChunkMeshObject BuildChunk (Chunk c) {
        List<Vector3> verts = new List<Vector3> ();
        List<int> tris = new List<int> ();
        List<Vector2> uvs = new List<Vector2> ();

        float oneNth = 1f / (float) n;

        for(int i = 0; i < c.voxels.GetLength(0); i++) {
            for (int j = 0; j < c.voxels.GetLength(1); j++) {
                for (int k = 0; k < c.voxels.GetLength(2); k++) {
                    List<int> openFaces = c.voxels[i, j, k].GetOpenFaces ();

                    int id = c.voxels[i, j, k].id;
                    VoxelTexture v = World.instance.textureScriptable.texturePositions[id];

                    for(int l = 0; l < openFaces.Count; l++) {

                        int si = verts.Count; // Start index for the triangles

                        switch (openFaces[l]) {
                            case 0:
                                // Bottom
                                verts.Add (new Vector3 (i, j, k));
                                verts.Add (new Vector3 (i + 1, j, k));
                                verts.Add (new Vector3 (i, j, k + 1));
                                verts.Add (new Vector3 (i + 1, j, k + 1));

                                uvs.Add (new Vector2 ((v.bottom.x + 1) / 16f, v.bottom.y / 16f));
                                uvs.Add (new Vector2 ((v.bottom.x + 1) / 16f, (v.bottom.y + 1) / 16f));
                                uvs.Add (new Vector2 (v.bottom.x / 16f, v.bottom.y / 16f));
                                uvs.Add (new Vector2 (v.bottom.x / 16f, (v.bottom.y + 1) / 16f));

                                tris.Add (si + 0);
                                tris.Add (si + 1);
                                tris.Add (si + 3);

                                tris.Add (si + 0);
                                tris.Add (si + 3);
                                tris.Add (si + 2);
                                break;
                            case 1:
                                // Top
                                verts.Add (new Vector3 (i, j + 1, k));
                                verts.Add (new Vector3 (i, j + 1, k + 1));
                                verts.Add (new Vector3 (i + 1, j + 1, k));
                                verts.Add (new Vector3 (i + 1, j + 1, k + 1));

                                uvs.Add (new Vector2 ((v.top.x) / 16f, (v.top.y) / 16f));
                                uvs.Add (new Vector2 ((v.top.x) / 16f, (v.top.y + 1) / 16f));
                                uvs.Add (new Vector2 ((v.top.x + 1) / 16f, (v.top.y) / 16f));
                                uvs.Add (new Vector2 ((v.top.x + 1) / 16f, (v.top.y + 1) / 16f));

                                tris.Add (si + 0);
                                tris.Add (si + 1);
                                tris.Add (si + 3);

                                tris.Add (si + 0);
                                tris.Add (si + 3);
                                tris.Add (si + 2);
                                break;
                            case 2:
                                // Left
                                verts.Add (new Vector3 (i, j, k));
                                verts.Add (new Vector3 (i, j, k + 1));
                                verts.Add (new Vector3 (i, j + 1, k));
                                verts.Add (new Vector3 (i, j + 1, k + 1));

                                uvs.Add (new Vector2 ((v.left.x + 1) / 16f, v.left.y / 16f));
                                uvs.Add (new Vector2 (v.left.x / 16f, v.left.y / 16f));
                                uvs.Add (new Vector2 ((v.left.x + 1) / 16f, (v.left.y + 1) / 16f));
                                uvs.Add (new Vector2 (v.left.x / 16f, (v.left.y + 1) / 16f));

                                tris.Add (si + 0);
                                tris.Add (si + 1);
                                tris.Add (si + 3);

                                tris.Add (si + 0);
                                tris.Add (si + 3);
                                tris.Add (si + 2);
                                break;
                            case 3:
                                // Right
                                verts.Add (new Vector3 (i + 1, j, k));
                                verts.Add (new Vector3 (i + 1, j + 1, k));
                                verts.Add (new Vector3 (i + 1, j, k + 1));
                                verts.Add (new Vector3 (i + 1, j + 1, k + 1));

                                uvs.Add (new Vector2 (v.right.x / 16f, v.right.y / 16f));
                                uvs.Add (new Vector2 (v.right.x / 16f, (v.right.y + 1) / 16f));
                                uvs.Add (new Vector2 ((v.right.x + 1) / 16f, v.right.y / 16f));
                                uvs.Add (new Vector2 ((v.right.x + 1) / 16f, (v.right.y + 1) / 16f));

                                tris.Add (si + 0);
                                tris.Add (si + 1);
                                tris.Add (si + 3);

                                tris.Add (si + 0);
                                tris.Add (si + 3);
                                tris.Add (si + 2);
                                break;
                            case 4:
                                // Back
                                verts.Add (new Vector3 (i, j, k));
                                verts.Add (new Vector3 (i, j + 1, k));
                                verts.Add (new Vector3 (i + 1, j, k));
                                verts.Add (new Vector3 (i + 1, j + 1, k));

                                uvs.Add (new Vector2 (v.back.x / 16f, v.back.y / 16f));
                                uvs.Add (new Vector2 (v.back.x / 16f, (v.back.y + 1) / 16f));
                                uvs.Add (new Vector2 ((v.back.x + 1) / 16f, v.back.y / 16f));
                                uvs.Add (new Vector2 ((v.back.x + 1) / 16f, (v.back.y + 1) / 16f));

                                tris.Add (si + 0);
                                tris.Add (si + 1);
                                tris.Add (si + 3);

                                tris.Add (si + 0);
                                tris.Add (si + 3);
                                tris.Add (si + 2);
                                break;
                            case 5:
                                // Front
                                verts.Add (new Vector3 (i, j, k + 1));
                                verts.Add (new Vector3 (i + 1, j, k + 1));
                                verts.Add (new Vector3 (i, j + 1, k + 1));
                                verts.Add (new Vector3 (i + 1, j + 1, k + 1));

                                uvs.Add (new Vector2 ((v.front.x + 1) / 16f, v.front.y / 16f));
                                uvs.Add (new Vector2 (v.front.x / 16f, v.front.y / 16f));
                                uvs.Add (new Vector2 ((v.front.x + 1) / 16f, (v.front.y + 1) / 16f));
                                uvs.Add (new Vector2 (v.front.x / 16f, (v.front.y + 1) / 16f));

                                tris.Add (si + 0);
                                tris.Add (si + 1);
                                tris.Add (si + 3);

                                tris.Add (si + 0);
                                tris.Add (si + 3);
                                tris.Add (si + 2);
                                break;
                        }
                    }
                }
            }
        }

        return new ChunkMeshObject (verts, tris, uvs);
    }
}
