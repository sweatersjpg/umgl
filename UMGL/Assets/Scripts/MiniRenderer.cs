using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRenderer : MonoBehaviour
{
    [SerializeField]
    Texture spriteSheet;

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

    float SW;
    float SH;

    float t = 0;

    int currentLayer = 0;

    struct Spr
    {
        public float sx, sy, x, y, sw, sh, w, h;
        public bool flip;

        public Spr(float SX, float SY, float X, float Y, float SW, float SH, bool Flip, float W, float H) : this()
        {
            sx = SX;
            sy = SY;
            x = X;
            y = Y;
            sw = SW;
            sh = SH;
            flip = Flip;
            w = W;
            h = H;
        }
    }

    Stack<Spr>[] spr_buffer; // array of stacks of Sprs

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();

        rt = new RenderTexture(width, height, 32);
        m_Renderer.material.SetTexture("_MainTex", rt);

        // uncomment for emissive screen
        //mr.material.EnableKeyword("_EMISSIONMAP");
        //mr.material.SetTexture("_EmissionMap", rt);

        rt.filterMode = FilterMode.Point;

        SW = spriteSheet.width;
        SH = spriteSheet.height;

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

        // clears texture
        GL.Clear(true, true, new Color(0, 0, 0, 1));

        // used for keeping the texture pixel perfect? its required but idk why tbh
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, width, height, 0);

        // sprite drawing goes here
        gameObject.SendMessage("Draw");

        Display();

        GL.PopMatrix();

        // un-sets out texture to be drawable
        RenderTexture.active = null;
    }

    // draws everything from the sprite buffer to the screen
    void Display()
    {
        for(int i = 0; i < spr_buffer.Length; i++)
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

    // --- sprite functions ---

    public void spr(float sx, float sy, float x, float y)
    {
        spr(sx, sy, x, y, 16, 16, false, 16, 16);
    }

    public void spr(float sx, float sy, float x, float y, float sw, float sh)
    {
        spr(sx, sy, x, y, sw, sh, false, sw, sh);
    }

    public void spr(float sx, float sy, float x, float y, float sw, float sh, bool flip)
    {
        spr(sx, sy, x, y, sw, sh, flip, sw, sh);
    }

    // adds sprite info to current layer's buffer
    public void spr(float sx, float sy, float x, float y, float sw, float sh, bool flip, float w, float h)
    {
        Spr s = new Spr(sx, sy, x, y, sw, sh, flip, w, h);
        spr_buffer[lget()].Push(s);
    }

    // actually draws sprites to screen
    void drawSprite(Spr s)
    {
        Rect source = new Rect(s.sx / SW, (SH - s.sy - s.sh) / SH, s.sw / SW, s.sh / SH);
        Rect dest = new Rect(s.x, s.y, s.w, s.h);
        if (s.flip) dest = new Rect(s.x + s.sw, s.y, -s.sw, s.sh);
        Graphics.DrawTexture(dest, spriteSheet, source, 0, 0, 0, 0, null, -1);
    }
}