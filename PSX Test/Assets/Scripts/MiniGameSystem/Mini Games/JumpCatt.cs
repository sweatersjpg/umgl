using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

class JumpCatt : MiniGame
{
    int score;
    int scorelerp;

    Guy guy;
    List<Platform> platforms;

    [HideInInspector]
    public Vector2 cam;

    public override void Title()
    {
        int x = 31 - 37 / 2;
        int y = 8;

        R.spr(83, 0, x, y, 37, 12);
        R.spr(120, 0, x, y + 13, 33, 6);

        if (scorelerp < score) scorelerp++;
        String s = scorelerp + "";
        if (s.Length < 2) s = "0" + s;

        R.put(s, 33, 48 - 6 - 8);

        int[] frames = { 16, 32, 16 };

        R.spr(frames[(frameCount / 10) % 3], 0, 14, 27, 16, 16);
    }

    public override void NewGame()
    {
        // initialize/reset game variables here
        platforms = new List<Platform>();

        Platform p = new Platform(20, 40, "be a butterfly", "shark", this);
        platforms.Add(p);
        platforms.Add(new Platform(70, 30, "touch grass", "boobs", this));
        newPlatform();
        newPlatform();
        newPlatform();

        guy = new Guy(20, 48 - 8 - 13, this);
        guy.currentPlatform = p;

        score = 0;
        scorelerp = 0;

        cam = new Vector2();
    }

    public override void Draw()
    {
        String s = score + "";
        if (s.Length < 2) s = "0" + s;
        R.put(s, 32 - s.Length * 4, 10);

        float maxSpeed = 0.5f;
        if (!guy.dead) cam.x += Math.Min(maxSpeed, Map(score, 0, 80, 0.25f, maxSpeed));

        if (guy.getScreenPos().x > 40) cam.x += ((guy.pos.x - 40) - cam.x) / 8;

        // draw platforms

        guy.update();

        for (int i = platforms.Count - 1; i >= 0; i--) platforms[i].draw();
        guy.draw();
    }

    void newPlatform()
    {
        float x = platforms[platforms.Count - 1].pos.x + platforms[platforms.Count - 1].size.x;
        float[] distances = { 10, 20, 30 };

        x += distances[(int) Random.Range(0, distances.Length)];

        float[] widths = { 10, 20, 30 };
        float w = widths[(int) Random.Range(0, distances.Length)];

        platforms.Add(new Platform(x, w, "", "", this));

    }

    float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    // --- classes ---

    class Guy
    {

        public JumpCatt game;

        public Vector2 pos;
        public Vector2 vel;
        public Vector2 size;

        public bool grounded = false;
        public int squishTimer = 0;

        public float GRAV = 0.5f;
        public float jumpSpeed = -4;

        public bool dead = false;

        public Platform currentPlatform;

        public Guy(float x, float y, JumpCatt G)
        {
            game = G;
            pos = new Vector2(x, y);
            size = new Vector2(8, 13);
            vel = new Vector2(getVelocity(10), jumpSpeed);
        }

        public float jumpPower = 0;

        public void update()
        {

            vel += new Vector2(0, GRAV);
            pos += vel;

            float ground = 40;

            if (pos.y + size.y > ground && !dead)
            {

                vel.x = 0;

                Platform platform = isAbovePlatform();

                if (platform != null)
                {
                    vel.y = 0;

                    pos.y = ground - size.y;

                    if (!grounded)
                    {
                        squishTimer = 3;

                        pos.x = Mathf.Floor(pos.x);

                        if (platform != currentPlatform)
                        {
                            game.score++;
                            currentPlatform = platform;
                        }
                    }

                    grounded = true;
                }
            }

            if (pos.y > 48)
            {
                if (dead && vel.y > 0)
                {
                    //go to title screen
                    if (pos.y > 48 + 16) game.GameOver();
                }
                else
                {
                    dead = true;
                    vel = new Vector2(0, jumpSpeed);
                    GRAV = 0.25f;
                }
            }

            if (getScreenPos().x + 10 < 0)
            {
                dead = true;
                GRAV = 0.25f;
                jumpSpeed *= 0.8f;

                vel = new Vector2(getVelocity(32), jumpSpeed);
            }

            if (grounded && ((!game.btn.b && game.pbtn.b) || (!game.btn.a && game.pbtn.a)))
            {
                grounded = false;
                vel.y = jumpSpeed;

                float dist = game.Map(jumpPower, 0, 60, 10, 40);
                jumpPower = 0;

                vel.x = getVelocity(dist);
            }

            if (grounded && (game.btn.b || game.btn.a) && jumpPower < 60) jumpPower++;
        }

        public Platform isAbovePlatform()
        {

            for (int i = 0; i < game.platforms.Count; i++)
            {
                Platform platform = game.platforms[i];
                if (pos.x + size.x > platform.pos.x && platform.pos.x + platform.size.x > pos.x) return platform;
            }

            return null;
        }

        public Vector2 getScreenPos()
        {
            return pos - game.cam;
        }

        public float getVelocity(float d)
        {
            float t = ((-2 * jumpSpeed) / GRAV);
            float v = d / t;

            return v;
        }

        public void draw()
        {

            int frame = 16;

            if ((game.btn.b || game.btn.a) && grounded) frame = 32;

            if (!grounded)
            {
                if (vel.y > 0) frame = 64;
                else frame = 48;
            }

            if (squishTimer > 0)
            {
                squishTimer--;
                frame = 0;
            }

            if (dead) frame = 160;

            float offset = 0;
            if (jumpPower > 50)
            {
                if (game.frameCount % 4 >= 2) offset = 1;
            }
            game.R.spr(frame, 0, pos.x - 4 - game.cam.x + offset, pos.y - 3, 16, 16);
        }
    }

    // platform class
    public class Platform
    {
        public Vector2 pos;
        public Vector2 size;

        public String hopes;
        public String dreams;

        public List<Vector3> grass;

        public JumpCatt game;

        public Platform(float x, float w, String hope, String dream, JumpCatt G)
        {
            pos = new Vector2(x, 48 - 8);
            size = new Vector2(w, 8);

            game = G;

            hopes = hope;
            dreams = dream;

            grass = new List<Vector3>();

            int[] grassXs = { 121, 124, 127 };
            int ngrass = (int) Random.Range(0, 4);
            for (int i = 0; i < ngrass; i++)
            {
                grass.Add(new Vector3(Mathf.Floor(pos.x + 1 + Random.Range(0,size.x - 4)), pos.y - 3, grassXs[(int)Random.Range(0,grassXs.Length)]));
            }
        }

        public void draw()
        {
            game.R.spr(80, 0, pos.x - game.cam.x, pos.y + 1, 1, 8);
            game.R.spr(80, 0, pos.x + size.x - 1 - game.cam.x, pos.y + 1, 1, 8);
            game.R.spr(81, 0, pos.x + 1 - game.cam.x, pos.y + 1, 1, 1, false, size.x - 2, 1);

            if (getScreenPos().x + size.x < 0)
            {
                game.platforms.Remove(this);
                game.newPlatform();
            }

            foreach(Vector3 g in grass)
            {
                //println(g);
                game.R.spr((int)g.z, 13, g.x - game.cam.x, g.y - game.cam.y, 3, 3);
            }
        }

        Vector2 getScreenPos()
        {
            return pos - game.cam;
        }
    }

}
