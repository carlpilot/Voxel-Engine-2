﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMeshObject {

    public List<Vector3> verts;
    public List<int> tris;
    public List<Vector2> uvs;

    public ChunkMeshObject (List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
        this.verts = verts;
        this.tris = tris;
        this.uvs = uvs;
    }
}
