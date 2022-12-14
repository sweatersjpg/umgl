using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

class CarGame : MiniGame
{

    public override void Title()
    {
        DrawRoad(frameCount/2);
    }

    public override void NewGame()
    {
        
    }

    public override void Draw()
    {
        
    }

    public void DrawRoad(float X)
    {
        float x = X % 8;
        float x2 = X / 2 % 8;

        for (int i = 0; i < 9; i++)
        {
            R.spr(80, 0, -x + i * 8, 48 - 15, 8, 16);
            R.spr(80, 0, -x + i * 8, 48 - 31, 8, 16);

            R.spr(88, 0, -x2 + i * 8, 9, 8, 8);
        }

        R.spr(0, 0, 8, 22, 12, 10);
    }

    public class WaveP
    {
        //int lifespan

        WaveP()
        {

        }

        public void Draw()
        {

        }
    }

}
