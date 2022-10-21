using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderer : MonoBehaviour
{
    [SerializeField]
    Texture spriteSheet;

    [SerializeField]
    int frameRate = 60;

    [SerializeField]
    int width = 256;
    [SerializeField]
    int height = 256;

    RenderTexture rt;

    Renderer m_Renderer;

    float SW;
    float SH;

    float t = 0;

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
        spr(16, 0, 0, 0, 16, 16, false, 16, 32);
        spr(16, 0, 16, 16, 16, 16, true);

        GL.PopMatrix();

        // un-sets out texture to be drawable
        RenderTexture.active = null;
    }

    // sprite functions

    void spr(float sx, float sy, float x, float y)
    {
        spr(sx, sy, x, y, 16, 16, false, 16, 16);
    }

    void spr(float sx, float sy, float x, float y, float sw, float sh)
    {
        spr(sx, sy, x, y, sw, sh, false, sw, sh);
    }

    void spr(float sx, float sy, float x, float y, float sw, float sh, bool flip)
    {
        spr(sx, sy, x, y, sw, sh, flip, sw, sh);
    }

    void spr(float sx, float sy, float x, float y, float sw, float sh, bool flip, float w, float h)
    {
        Rect source = new Rect(sx / SW, (SH-sy-sh) / SH, sw / SW, sh / SH);
        Rect dest = new Rect(x, y, w, h);
        if (flip) dest = new Rect(x + sw, y, -sw, sh);
        Graphics.DrawTexture(dest, spriteSheet, source, 0,0,0,0, null, -1);
    }
}