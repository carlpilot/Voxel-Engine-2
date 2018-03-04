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
}
