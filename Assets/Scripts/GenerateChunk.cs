using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChunk : MonoBehaviour {

    public World world;

	void Start () {
        world.GenerateChunk (0, 0);		
	}
}
