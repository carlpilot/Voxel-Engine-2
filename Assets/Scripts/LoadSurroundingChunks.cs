using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSurroundingChunks : MonoBehaviour {

    public int radius;

	void Update () {
        int currentChunkX = Mathf.FloorToInt (transform.position.x / (float) World.chunkWidth);
        int currentChunkY = Mathf.FloorToInt (transform.position.z / (float) World.chunkWidth);

        for(int i = currentChunkX - radius - 1; i <= currentChunkX + radius + 1; i++) {
            for(int j = currentChunkY - radius - 1; j <= currentChunkY + radius + 1; j++) {
                if (Vector2.Distance (new Vector2 (currentChunkX, currentChunkY), new Vector2 (i, j)) <= radius) {
                    World.instance.LoadChunk (i, j);
                } else {
                    World.instance.UnloadChunk (i, j);
                }
            }
        }
    }
}
