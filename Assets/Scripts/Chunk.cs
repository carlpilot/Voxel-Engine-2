using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public int x, y;

    public volatile Voxel[,,] voxels = new Voxel[World.chunkWidth, World.chunkHeight, World.chunkWidth];
    public volatile bool voxelsInitialised = false;

    public void InitVoxels () {
        StartCoroutine (initVoxels ());
    }

    IEnumerator initVoxels () {

        //Debug.Log ("Initialising voxels on chunk " + x + "," + y);

        Thread t = new Thread (() => {
            for (int i = 0; i < voxels.GetLength (0); i++) {
                for(int j = 0; j < voxels.GetLength(1); j++) {
                    for(int k = 0; k < voxels.GetLength(2); k++) {
                        int blockType = WorldGen.Get (i + (this.x * World.chunkWidth), j, k + (this.y * World.chunkWidth));
                        voxels[i, j, k] = new Voxel (i, j, k, this, blockType);
                    }
                }
            }
            voxelsInitialised = true;
        });
        t.Start ();

        while (!voxelsInitialised) {
            yield return null;
        }

        //Debug.Log ("Chunk " + x + "," + y + " initialised");
    }

    public void Render () {
        StartCoroutine (RenderChunk ());
    }

    IEnumerator RenderChunk () {
        //Debug.Log ("Rendering chunk...");
        bool isDone = false;
        ChunkMeshObject cmo = null;

        Thread t = new Thread (() => {
            cmo = ChunkBuilder.BuildChunk (this);
            isDone = true;
        });
        t.Start ();

        while (!isDone) {
            yield return null;
        }

        Mesh m = new Mesh ();
        m.name = "ChunkMesh-" + x + "-" + y;
        m.vertices = cmo.verts.ToArray ();
        m.triangles = cmo.tris.ToArray ();
        m.uv = cmo.uvs.ToArray ();

        m.RecalculateBounds ();
        m.RecalculateNormals ();
        m.RecalculateTangents ();

        if (GetComponent<MeshFilter> () == null) {
            this.gameObject.AddComponent<MeshFilter> ().mesh = m;
            this.gameObject.AddComponent<MeshRenderer> ().material = World.instance.material;
        } else {
            GetComponent<MeshFilter> ().mesh = m;
        }

        //Debug.Log ("Chunk " + x + "," + y + " renderered");
    }
}
