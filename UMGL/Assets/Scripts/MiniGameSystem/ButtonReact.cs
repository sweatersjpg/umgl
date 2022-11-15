using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReact : MonoBehaviour
{
    public MiniGame miniGame;

    public Transform a_button;
    public Transform b_button;
    public Transform start_button;
    public Transform select_button;
    public Transform d_pad;

    // Update is called once per frame
    void Update()
    {

        PushButton(a_button, miniGame.btn.a);
        PushButton(b_button, miniGame.btn.b);
        PushButton(start_button, miniGame.btn.start);
        PushButton(select_button, miniGame.btn.select);

        Vector3 angles = new Vector3();
        float tilt = 4;

        if (miniGame.btn.up) angles.x += tilt;
        if (miniGame.btn.down) angles.x -= tilt;
        if (miniGame.btn.left) angles.z += tilt;
        if (miniGame.btn.right) angles.z -= tilt;

        d_pad.localEulerAngles = angles;
    }

    void PushButton(Transform b, bool button)
    {
        //if (button) b.localPosition = origin + new Vector3(0, -0.005f, 0);
        //else b.localPosition = origin;
        if (button) b.localScale = new Vector3(1, 0.5f, 1);
        else b.localScale = new Vector3(1, 1, 1);
    }

}
