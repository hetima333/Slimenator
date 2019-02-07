using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    public static bool A_Button()
    {
        return Input.GetButtonDown("A_Button");
    }

    public static bool B_Button()
    {
        return Input.GetButtonDown("B_Button");
    }

    public static bool X_Button()
    {
        return Input.GetButtonDown("X_Button");
    }

    public static bool Y_Button()
    {
        return Input.GetButtonDown("Y_Button");
    }

    public static Vector3 LS_Joystick()
    {
        return new Vector3(LS_Horizontal(), 0, LS_Vertical());
    }

    public static Vector3 RS_Joystick()
    {
        return new Vector3(RS_Horizontal(), 0, RS_Vertical());
    }

    public static float LS_Horizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis("LS_Horizontal");
        r += Input.GetAxis("K_Horizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float LS_Vertical()
    {
        float r = 0.0f;
        r += Input.GetAxis("LS_Vertical");
        r += Input.GetAxis("K_Vertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float RS_Horizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis("RS_Horizontal");
        //r += Input.GetAxis("Mouse X");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float RS_Vertical()
    {
        float r = 0.0f;
        r += Input.GetAxis("RS_Vertical");
        //r += Input.GetAxis("Mouse Y");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static bool Left_Trigger()
    {
        if (Input.GetAxis("Left_Trigger") == 1)
            return true;

        return false;
    }

    public static bool Right_Trigger()
    {
        if (Input.GetAxis("Right_Trigger") == 1)
            return true;

        return false;
    }

    public static bool Left_Bumper()
    {
        return Input.GetButtonDown("Left_Bumper");
    }

    public static bool Right_Bumper()
    {
        return Input.GetButtonDown("Right_Bumper");
    }

    public static bool Suck_Input()
    {
        return Input.GetKey(KeyCode.Mouse0) || Left_Trigger();
    }

    public static bool CombineOrbs_Input()
    {
        return Input.GetKeyDown(KeyCode.LeftShift) || A_Button();
    }

    public static bool UseSkills_Input()
    {
        return Input.GetKeyDown(KeyCode.Mouse1) || Right_Trigger();
    }

    public static float SkillScroll_Input()
    {
        if (Left_Bumper())
            return 1;
        else if (Right_Bumper())
            return -1;

        return Input.GetAxis("Mouse ScrollWheel");
    }
}
