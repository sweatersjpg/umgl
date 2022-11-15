using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TestGame : MiniGame
{
    int x = 0;

    public override void Title()
    {
        R.spr(0, 0, 16, 16);

        R.put("Title\nScreen!", 0, 0);
    }

    public override void NewGame()
    {
        x = 0;
    }

    public override void Draw()
    {
        if (btn.up) x--;
        if (btn.down) x++;

        R.spr(16, 0, 32 + 16 * Mathf.Sin(frameCount / 10f) - 8, 16 + x);
    }
}
