using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    [HideInInspector]
    public MiniRenderer R;

    [SerializeField]
    ButtonMap buttonMap;

    public bool isSelected = false;

    public class BtnInput
    {
        public bool a, b, up, down, left, right, start, select;
        public bool[] input = new bool[8];

        public BtnInput()
        {
            //Set(false, false, false, false, false, false, false, false);
            for (int i = 0; i < input.Length; i++) input[i] = false;
            SetBools();
        }

        BtnInput(BtnInput other)
        {
            for (int i = 0; i < input.Length; i++) input[i] = other.input[i];
            SetBools();
        }

        public void SetBools()
        {
            a = input[0];
            b = input[1];
            up = input[2];
            down = input[3];
            left = input[4];
            right = input[5];
            start = input[6];
            select = input[7];
        }

        public BtnInput copy()
        {
            return new BtnInput(this);
        }
    }

    public BtnInput btn;
    public BtnInput pbtn;

    int gameState = 0;

    [HideInInspector]
    public int frameCount = 0;

    void Init(MiniRenderer mr) // called from MiniRenderer
    {
        R = mr;

        btn = new BtnInput();
        pbtn = btn.copy();
    }

    void Update()
    {
        // -- inputs --
        // get key codes from button map
        if (!isSelected) return;

        KeyCode[][] buttonMapArray = buttonMap.ToArray();
        for (int i = 0; i < btn.input.Length; i++)
        {
            bool keyDown = false;
            for (int j = 0; j < buttonMapArray[i].Length; j++) keyDown |= Input.GetKey(buttonMapArray[i][j]);
            btn.input[i] = keyDown;
        }
        btn.SetBools();
    }

    void FrameUpdate() // called from MiniRenderer
    {

        if (gameState == 0)
        {
            frameCount++;
            Title();
            R.Display();

            if ((btn.start && !pbtn.start) || (btn.a && !pbtn.a) || (btn.b && !pbtn.b))
            { // if a, b, or start pressed reset game variables & start game
                gameState++;
                NewGame();
            }
        }
        else if (gameState == 1)
        {
            frameCount++;
            Draw();
            if (willPause || (btn.start && !pbtn.start))
            { // if start button pressed pause the game
                willPause = false;
                int oldL = R.lget();
                R.lset(9); // set to top layer
                R.put("pause", 32 - 4 * 5, 20);// draw pause button over the game
                R.lset(oldL);
                gameState = 2;
            }
            R.Display();
        }
        else
        {
            if ((btn.start && !pbtn.start) || (btn.a && !pbtn.a) || (btn.b && !pbtn.b)) gameState--;
        }

        pbtn = btn.copy(); // store btn into pbtn
    }

    bool willPause = false;

    public void Pause()
    {
        if (gameState != 1) return;

        willPause = true;
    }

    public void GameOver()
    {
        gameState = 0;
    }

    public abstract void NewGame();

    // to be overriden
    public abstract void Title();

    // to be overriden
    public abstract void Draw();

}
