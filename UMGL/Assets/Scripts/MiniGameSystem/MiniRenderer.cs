using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRenderer : MonoBehaviour
{
    [SerializeField]
    Texture spriteSheet;

    [SerializeField]
    Texture fontSheet;

    [SerializeField]
    int frameRate = 60;

    [SerializeField]
    int width = 256;
    [SerializeField]
    int height = 256;

    [SerializeField]
    int layers = 10;

    RenderTexture rt;

    Renderer m_Renderer;

    float t = 0;

    int currentLayer = 0;

    // stores arguments for drawing a sprite to the screen later
    struct Spr
    {
        public float sx, sy, x, y, sw, sh, w, h;
        public bool flip;
        public Texture texture;

        // I don't know what ": this()" is for
        public Spr(float sx, float sy, float x, float y, float sw, float sh, bool flip, float w, float h, Texture texture) : this()
        {
            this.sx = sx;
            this.sy = sy;
            this.x = x;
            this.y = y;
            this.sw = sw;
            this.sh = sh;
            this.flip = flip;
            this.w = w;
            this.h = h;
            this.texture = texture;
        }
    }

    Stack<Spr>[] spr_buffer; // array of stacks of Sprs

    // Start is called before the first frame update
    void Awake()
    {
        m_Renderer = GetComponent<Renderer>();

        rt = new RenderTexture(width, height, 32);
        m_Renderer.material.SetTexture("_MainTex", rt);

        // uncomment for emissive screen
        //mr.material.EnableKeyword("_EMISSIONMAP");
        //mr.material.SetTexture("_EmissionMap", rt);

        rt.filterMode = FilterMode.Point;

        if (width == 0) width = 256;
        if (height == 0) height = 256;

        // init spr buffer
        spr_buffer = new Stack<Spr>[layers];
        for(int i = 0; i < spr_buffer.Length; i++)
        {
            spr_buffer[i] = new Stack<Spr>();
        }

        gameObject.SendMessage("Init", this);

    }

    // Update is called once per frame
    void Update()
    {

        // run at a solid 30fps
        if (Time.time - t < 1.0 / frameRate) return;
        t = Time.time;

        // set our render texture to be drawable
        RenderTexture.active = rt;

        // used for keeping the texture pixel perfect? its required but idk why tbh
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, width, height, 0);

        // sprite drawing goes here
        gameObject.SendMessage("FrameUpdate");

        //Display();

        GL.PopMatrix();

        // un-sets out texture to be drawable
        RenderTexture.active = null;
    }

    // draws everything from the sprite buffer to the screen
    public void Display()
    {
        // clears texture
        GL.Clear(true, true, new Color(0, 0, 0, 1));

        for (int i = 0; i < spr_buffer.Length; i++)
            while (spr_buffer[i].Count > 0) drawSprite(spr_buffer[i].Pop());
    }

    // --- layer functions ---

    public void lset(int layer)
    {
        currentLayer = layer;
    }

    public int lget()
    {
        return currentLayer;
    }

    // --- text functions ---

    public void put(float n, float x, float y) =>put(n.ToString(), x, y);
    public void put(int n, float x, float y) => put(n.ToString(), x, y);

    public void put(string str, float X, float Y)
    {
        float x = X-8, y = Y;
        for(int i = 0; i < str.Length; i++)
        {
            if(str[i] == '\n')
            {
                x = X-8;
                y += 8;
                continue;
            }
            int index = (int)str[i] - 32;
            int sx = index % 32 * 8;
            int sy = index / 32 * 8;
            spr(fontSheet, sx, sy, x += 8, y, 8, 8, false, 8, 8);
        }
    }

    // --- sprite functions ---

    public void spr(float sx, float sy, float x, float y)
        => spr(spriteSheet, sx, sy, x, y, 16, 16, false, 16, 16);

    public void spr(float sx, float sy, float x, float y, float sw, float sh)
        => spr(spriteSheet, sx, sy, x, y, sw, sh, false, sw, sh);

    public void spr(float sx, float sy, float x, float y, float sw, float sh, bool flip)
        =>spr(spriteSheet, sx, sy, x, y, sw, sh, flip, sw, sh);

    // adds sprite info to current layer's buffer
    public void spr(float sx, float sy, float x, float y, float sw, float sh, bool flip, float w, float h)
        =>spr(spriteSheet, sx, sy, x, y, sw, sh, flip, w, h);

    public void spr(Texture texture, float sx, float sy, float x, float y, float sw, float sh, bool flip, float w, float h)
    {
        spr_buffer[lget()].Push(new Spr(sx, sy, x, y, sw, sh, flip, w, h, texture));
    }

    // actually draws sprites to screen
    void drawSprite(Spr s)
    {
        int SW = s.texture.width;
        int SH = s.texture.height;

        Rect source = new Rect(s.sx / SW, (SH - s.sy - s.sh) / SH, s.sw / SW, s.sh / SH);
        Rect dest = new Rect(s.x, s.y, s.w, s.h);
        if (s.flip) dest = new Rect(s.x + s.sw, s.y, -s.sw, s.sh);
        Graphics.DrawTexture(dest, s.texture, source, 0, 0, 0, 0, null, -1);
    }
}