using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour {

    public static float Get (float x, float y) {
        return Mathf.PerlinNoise (x, y);
    }
}
