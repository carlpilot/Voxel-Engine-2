using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour {

    public static World instance;

    public static int chunkWidth = 16;
    public static int chunkHeight = 64;
    public Material material;

    public TextureScriptable textureScriptable;

    public List<Vector2> chunksBeingGenerated = new List<Vector2> ();
    public Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk> ();
    public List<Vector2> loadQueue = new List<Vector2> ();

    void Awake () {
        if (instance == null) instance = this;
    }

    private void Update () {
        if(loadQueue.Count > 0 && !chunksBeingGenerated.Contains(loadQueue[0])) {
            LoadChunkFromQueue ((int)loadQueue[0].x, (int)loadQueue[0].y);
        }
    }

    public void ChangeVoxel (int x, int y, int z, int id) {
        ChangeVoxel (PositionToVoxel (new Vector3 (x, y, z)), id);
    }

    public void ChangeVoxel (Voxel v, int id) {
        ChangeVoxelWithoutReload (v, id);
        ReRenderVoxelChunks (v);
    }

    public void ChangeVoxelWithoutReload (int x, int y, int z, int id) {
        ChangeVoxelWithoutReload (PositionToVoxel (new Vector3 (x, y, z)), id);
    }

    public void ChangeVoxelWithoutReload (Voxel v, int id) {
        if (v != null)
            v.id = id;
    }

    public void ChangeVoxels (Voxel[] v, int id) {
        List<Chunk> chunksToReload = new List<Chunk> ();
        foreach (Voxel vox in v) {
            if (v != null) {
                ChangeVoxelWithoutReload (vox, id);
                List<Chunk> chunksToReloadForThisVoxel = GetChunksToReload (vox);
                if (chunksToReloadForThisVoxel != null) {
                    foreach (Chunk c in chunksToReloadForThisVoxel) {
                        chunksToReload.Add (c);
                    }
                }
            }
        }

        chunksToReload = chunksToReload.Distinct ().ToList ();

        foreach (Chunk c in chunksToReload) {
            c.Render ();
        }
    }

    public void ReRenderVoxelChunks (Voxel v) {

        if (v == null) return;

        List<Chunk> chunksToReload = GetChunksToReload (v);
        foreach(Chunk c in chunksToReload) {
            c.Render ();
        }
    }

    public List<Chunk> GetChunksToReload (Voxel v) {

        if (v == null) return null;

        List<Chunk> chunksToReload = new List<Chunk> ();
        chunksToReload.Add (v.parent);
        if (v.x == 0 && chunks.ContainsKey (new Vector2 (v.parent.x - 1, v.parent.y))) {
            chunksToReload.Add (chunks[new Vector2 (v.parent.x - 1, v.parent.y)]);
        }
        if (v.x == chunkWidth - 1 && chunks.ContainsKey (new Vector2 (v.parent.x + 1, v.parent.y))) {
            chunksToReload.Add (chunks[new Vector2 (v.parent.x + 1, v.parent.y)]);
        }
        if (v.y == 0 && chunks.ContainsKey (new Vector2 (v.parent.x, v.parent.y - 1))) {
            chunksToReload.Add (chunks[new Vector2 (v.parent.x, v.parent.y - 1)]);
        }
        if (v.y == chunkWidth - 1 && chunks.ContainsKey (new Vector2 (v.parent.x, v.parent.y + 1))) {
            chunksToReload.Add (chunks[new Vector2 (v.parent.x, v.parent.y + 1)]);
        }
        return chunksToReload;
    }

    public void GenerateChunk (int x, int y) {
        if (!chunksBeingGenerated.Contains (new Vector2 (x, y))) {
            StartCoroutine (generateChunk (x, y));
        }
    }

    public void LoadChunk(int x, int y) {
        if (!chunks.ContainsKey (new Vector2 (x, y)) && !loadQueue.Contains (new Vector2 (x, y))) { // Don't queue already loaded chunks or already queued chunks
            loadQueue.Add (new Vector2 (x, y));
        }
    }

    public void LoadChunkFromQueue (int x, int y) {
        if(chunks.ContainsKey(new Vector2(x, y))) {
            chunks[new Vector2 (x, y)].gameObject.SetActive (true);
        } else {
            GenerateChunk (x, y);
        }
    }

    public void UnloadChunk (int x, int y) {
        if (chunks.ContainsKey (new Vector2 (x, y))) {
            Destroy (chunks[new Vector2 (x, y)].gameObject);//.SetActive (false);
            chunks.Remove (new Vector2 (x, y));
        }
        if(loadQueue.Contains(new Vector2(x, y))) {
            loadQueue.Remove (new Vector2 (x, y));
        }
    }

    IEnumerator generateChunk (int x, int y) {
        chunksBeingGenerated.Add (new Vector2 (x, y));
        Chunk c = new GameObject ("Chunk " + x + "," + y).AddComponent<Chunk> ();
        c.x = x;
        c.y = y;
        c.InitVoxels ();
        c.gameObject.transform.position = new Vector3 (x * chunkWidth, 0f, y * chunkWidth);

        while(!c.voxelsInitialised) {
            yield return new WaitForEndOfFrame ();
        }

        chunks.Add (new Vector2 (x, y), c);
        c.Render ();
        chunksBeingGenerated.Remove (new Vector2 (x, y));
        loadQueue.Remove (new Vector2 (x, y));
    }

    public Voxel PositionToVoxel (Vector3 position) {
        if (position.y < 0 || position.y >= chunkHeight) return null;

        int chunkX = Mathf.FloorToInt (position.x / (float)chunkWidth);
        int chunkY = Mathf.FloorToInt (position.z / (float)chunkWidth);

        int voxelX = Mathf.FloorToInt (position.x - chunkX * chunkWidth);
        int voxelY = Mathf.FloorToInt (position.y);
        int voxelZ = Mathf.FloorToInt (position.z - chunkY * chunkWidth);

        //print (chunkX + "," + chunkY + " :: " + voxelX + "," + voxelY + "," + voxelZ);

        if (    chunks.ContainsKey (new Vector2 (chunkX, chunkY)) &&
                chunks[new Vector2 (chunkX, chunkY)].gameObject.activeInHierarchy &&
                chunks[new Vector2 (chunkX, chunkY)].voxels != null &&
                chunks[new Vector2(chunkX, chunkY)].voxels[voxelX, voxelY, voxelZ] != null
           ) {

            return chunks[new Vector2 (chunkX, chunkY)].voxels[voxelX, voxelY, voxelZ];
        } else {
            return null;
        }
    }

    public Vector3 VoxelToPosition (Voxel v) {
        if (v == null) return Vector3.zero;
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
