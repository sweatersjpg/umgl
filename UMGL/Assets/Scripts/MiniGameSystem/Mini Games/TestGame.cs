using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TestGame : MiniGame
{
    int x = 0;
    int y = 0;

    public override void Title()
    {
        R.spr(0, 0, 16, 16);

        R.put("Title\nScreen!", 0, 0);
    }

    public override void NewGame()
    {
        x = 0;
        y = 0;
    }

    public override void Draw()
    {
        if (btn.up) x--;
        if (btn.down) x++;

        if (btn.left) y--;
        if (btn.right) y++;

        R.spr(16, 0, y, x);
    }
}
