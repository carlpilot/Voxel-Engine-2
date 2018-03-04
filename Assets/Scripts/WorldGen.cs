using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour {

    public static float scale = 0.01f;
    public static float height = 50f;
    public static float heightBuffer = 20;

	public static int Get (int x, int y, int z) {
        int heightAtPoint = Mathf.RoundToInt (SampleHeight (x, z));
        return y > heightAtPoint ? 0 : (y == heightAtPoint) ? 3 : (y > heightAtPoint - 3) ? 2 : 1;
    }

    public static float SampleHeight (int x, int z) {
        return Noise.Get ((x + 0.5f) * scale, (z + 0.5f) * scale) * height + heightBuffer;
    }
}
