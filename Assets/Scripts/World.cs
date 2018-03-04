using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public static World instance;

    public static int chunkWidth = 16;
    public static int chunkHeight = 256;
    public Material material;

    public TextureScriptable textureScriptable;

    public Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk> ();

    void Awake () {
        if (instance == null) instance = this;
    }

    public void GenerateChunk (int x, int y) {
        StartCoroutine (generateChunk (x, y));
    }

    IEnumerator generateChunk (int x, int y) {
        Chunk c = new GameObject ("Chunk " + x + "," + y).AddComponent<Chunk> ();
        c.x = x;
        c.y = y;
        c.InitVoxels ();

        while(!c.voxelsInitialised) {
            yield return new WaitForEndOfFrame ();
        }

        chunks.Add (new Vector2 (x, y), c);
        c.Render ();
    }

    public Voxel PositionToVoxel (Vector3 position) {
        if (position.y < 0) return null;

        int chunkX = Mathf.FloorToInt (position.x / (float)World.chunkWidth);
        int chunkY = Mathf.FloorToInt (position.z / (float)World.chunkWidth);

        int voxelX = Mathf.FloorToInt (position.x - chunkX);
        int voxelY = Mathf.FloorToInt (position.y);
        int voxelZ = Mathf.FloorToInt (position.z - chunkY);

        if (    chunks.ContainsKey (new Vector2 (chunkX, chunkY)) &&
                chunks[new Vector2 (chunkX, chunkY)].gameObject.activeInHierarchy &&
                chunks[new Vector2 (chunkX, chunkY)].voxels != null
           ) {

            return chunks[new Vector2 (chunkX, chunkY)].voxels[voxelX, voxelY, voxelZ];
        } else {
            return null;
        }
    }

    public Vector3 VoxelToPosition (Voxel v) {
        return new Vector3 (v.x + v.parent.x * World.chunkWidth, v.y, v.z + v.parent.y * chunkWidth);
    }

    public Voxel RaycastVoxel (Vector3 position, Vector3 direction) {
        bool hasFound = false;
        int i = 0;
        float step = 0.1f;
        while(!hasFound && (i * step) < 15) {
            Vector3 searchPosition = position + (i * direction.normalized * step);
            if(PositionToVoxel(searchPosition) != null && PositionToVoxel(searchPosition).id != 0) { // don't break air blocks
                hasFound = true;
                //print (PositionToVoxel (searchPosition).id + " at " + searchPosition);
                return PositionToVoxel (searchPosition);
            }
            i++;
        }
        return null;
    }
}
