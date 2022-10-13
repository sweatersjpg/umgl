using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderer : MonoBehaviour
{
    [SerializeField]
    Texture spriteSheet;

    [SerializeField]
    int frameRate;

    [SerializeField]
    int width;
    [SerializeField]
    int height;

    RenderTexture rt;

    Renderer mr;

    float SW;
    float SH;

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

        if (SW == 0) SW = 256;
        if (SH == 0) SH = 256;
    }

    float t = 0;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        // set our render texture to be drawable
        RenderTexture.active = rt;

        // clears texture
        GL.Clear(true, true, new Color(0, 0, 0, 1));

        // used for keeping the texture pixel perfect? its required but idk why tbh
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, width, height, 0);


        // sprite drawing goes here
        //drawSprite(0, 0, 16, 32, 16, 16, false);
        drawSprite(16, 0, 24f + 16f * Mathf.Sin(t), 16, 16, 16);


        GL.PopMatrix();

        // un-sets out texture to be drawable
        RenderTexture.active = null;
    }

    void drawSprite(float sx, float sy, float x, float y, float sw, float sh)
    {
        // sourceRect uses normalized coodinates so (1, 1) is the size of the whole texture
        // so for pixel coordinates we use divide the desired coorinate by the width of the texture
        Rect source = new Rect(sx / SW, sy / SH, sw / SW, sh / SH);
        //if(flip) source = new Rect(sx / SW, sy / SH, -sw / SW, sh / SH);

        Graphics.DrawTexture(new Rect(x, y, sw, sh), spriteSheet, source, (int) sw, (int) sw, (int) sh, (int) sh, null, -1);
    }
}