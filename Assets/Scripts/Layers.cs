using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers {

    public static int player = 6, enemy = 7;

    internal static bool InMask(LayerMask layermask, int layer) {
        return layermask == (layermask | (1 << layer));
    }
}
