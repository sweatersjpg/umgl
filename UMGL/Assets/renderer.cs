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

    Renderer mr;

    float SW;
    float SH;

    float t = 0;

    Vector2 pos;
    Vector2 vel;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<Renderer>();

        //mr.material.EnableKeyword("_EMISSIONMAP");

        rt = new RenderTexture(width, height, 32);
        mr.material.SetTexture("_MainTex", rt);
        //mr.material.SetTexture("_EmissionMap", rt);

        rt.filterMode = FilterMode.Point;

        SW = spriteSheet.width;
        SH = spriteSheet.height;

        if (width == 0) width = 256;
        if (height == 0) height = 256;

        pos = new Vector2(Random.Range(0, width-16), Random.Range(0, height - 16));
        vel = new Vector2(1.5f, 1);

    }

    // Update is called once per frame
    void Update()
    {
        // run at a solid 60fps
        if (Time.time - t < 1.0 / frameRate) return;
        t = Time.time;

        // set our render texture to be drawable
        RenderTexture.active = rt;

        // clears texture
        GL.Clear(true, true, new Color(0, 0, 0, 1));

        // used for keeping the texture pixel perfect? its required but idk why tbh
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, width, height, 0);

        UpdateCat();
        // sprite drawing goes here
        //drawSprite(0, 0, 16, 32, 16, 16, false);
        drawSprite(16, 0, pos.x, pos.y, 16, 16);

        GL.PopMatrix();

        // un-sets out texture to be drawable
        RenderTexture.active = null;
    }

    private void UpdateCat()
    {
        pos += vel;

        if (pos.x > width - 16) vel.x = -Mathf.Abs(vel.x);
        if (pos.x < 0) vel.x = Mathf.Abs(vel.x);
        if (pos.y > height - 16) vel.y = -Mathf.Abs(vel.y);
        if (pos.y < 0) vel.y = Mathf.Abs(vel.y);

        //Debug.Log(vel);
    }

    void drawSprite(float sx, float sy, float x, float y, float sw, float sh)
    {
        // sourceRect uses normalized coodinates so (1, 1) is the size of the whole texture
        // so for pixel coordinates we use divide the desired coorinate by the width of the texture
        Rect source = new Rect(sx / SW, sy / SH, sw / SW, sh / SH);

        //source = new Rect(sx / SW, sy / SH, -sw / SW, sh / SH);

        Graphics.DrawTexture(new Rect(x, y, sw, sh), spriteSheet, source, (int) sw, (int) sw, (int) sh, (int) sh, null, -1);
    }
}