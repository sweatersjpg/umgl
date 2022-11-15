using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTest : MonoBehaviour
{
    MiniRenderer R;

    void Init(MiniRenderer mr)
    {
        // save reference to renderer
        R = mr;

        
    }

    void Draw()
    {
        R.lset(1);
        R.spr(0, 0, 16, 16);

        R.lset(0);
        R.spr(0, 0, 20, 12);
    }
}
