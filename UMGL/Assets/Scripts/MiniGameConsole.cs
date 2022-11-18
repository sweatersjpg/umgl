using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameConsole : MonoBehaviour
{
    public MiniGame miniGame;

    public Transform a_button;
    public Transform b_button;
    public Transform start_button;
    public Transform select_button;
    public Transform d_pad;

    public float buttonTilt = 4;
    public float tilt = -2;

    Vector3 targetAngles;
    Vector3 currentAngles;

    Outline outline;
    float outlineSize;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outlineSize = outline.OutlineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseIsOver && Input.GetMouseButtonDown(0)) OnMouseDownOff();

        PushButton(a_button, miniGame.btn.a);
        PushButton(b_button, miniGame.btn.b);
        PushButton(start_button, miniGame.btn.start);
        PushButton(select_button, miniGame.btn.select);

        Vector3 angles = new Vector3();

        if (miniGame.btn.up) angles.x += buttonTilt;
        if (miniGame.btn.down) angles.x -= buttonTilt;
        if (miniGame.btn.left) angles.z += buttonTilt;
        if (miniGame.btn.right) angles.z -= buttonTilt;

        d_pad.localEulerAngles = angles;

        Vector3 tiltAngles = new Vector3();

        if (miniGame.btn.up) tiltAngles.z += tilt;
        if (miniGame.btn.down) tiltAngles.z -= tilt;
        if (miniGame.btn.left) tiltAngles.x += tilt;
        if (miniGame.btn.right) tiltAngles.x -= tilt;
        if (miniGame.btn.a) tiltAngles.y -= tilt;
        if (miniGame.btn.b) tiltAngles.y += tilt;

        targetAngles = tiltAngles;
        currentAngles += (targetAngles - currentAngles) / 4 * Time.deltaTime * 50;
        transform.localEulerAngles = currentAngles;
    }

    bool mouseIsOver = false;

    private void OnMouseOver()
    {
        mouseIsOver = true;
        outline.OutlineWidth = outlineSize*2;
    }

    private void OnMouseExit()
    {
        mouseIsOver = false;
        outline.OutlineWidth = outlineSize;
    }

    private void OnMouseDown()
    {
        outline.OutlineColor = new Color(1, 1, 1);
        miniGame.isSelected = true;
    }

    private void OnMouseDownOff()
    {
        outline.OutlineColor = new Color(0,0,0);
        miniGame.isSelected = false;
        miniGame.Pause();
    }

    void PushButton(Transform b, bool button)
    {
        //if (button) b.localPosition = origin + new Vector3(0, -0.005f, 0);
        //else b.localPosition = origin;
        if (button) b.localScale = new Vector3(1, 0.5f, 1);
        else b.localScale = new Vector3(1, 1, 1);
    }

}
