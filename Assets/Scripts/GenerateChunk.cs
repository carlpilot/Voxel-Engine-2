using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChunk : MonoBehaviour {

    public int radius;

	void Start () {
        //world.GenerateChunk (0, 0);

        for(int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                if(Vector2.Distance(Vector2.zero, new Vector2(i, j)) <= radius) {
                    World.instance.GenerateChunk (i, j);
                }
            }
        }
	}
}
