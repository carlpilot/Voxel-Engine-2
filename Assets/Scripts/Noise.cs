using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour {

    public static int octaves = 8;
    public static float persistence = 0.5f;
    public static float octaveScaling = 3f;

    public static float Get (float x, float y) {
        float n = 0;
        for(int i = 0; i < octaves; i++) {
            n += Mathf.PerlinNoise (x * octaveScaling * (i + 1), y * octaveScaling * (i + 1)) * Mathf.Pow (persistence, (float) i);
        }
        return n / ((1f/persistence) / ((1f / persistence) - 1f)); // Map between 0 and 1
    }

    public static float GetBasic (float x, float y) {
        return Mathf.PerlinNoise (x, y);
    }
}
