using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Button Map", menuName = "ButtonMap")]
public class ButtonMap : ScriptableObject
{
    public KeyCode[] a;
    public KeyCode[] b;
    public KeyCode[] up;
    public KeyCode[] down;
    public KeyCode[] left;
    public KeyCode[] right;
    public KeyCode[] start;
    public KeyCode[] select;

    public KeyCode[][] ToArray()
    {
        KeyCode[][] map = new KeyCode[8][];
        map[0] = a;
        map[1] = b;
        map[2] = up;
        map[3] = down;
        map[4] = left;
        map[5] = right;
        map[6] = start;
        map[7] = select;
        return map;
    }
}