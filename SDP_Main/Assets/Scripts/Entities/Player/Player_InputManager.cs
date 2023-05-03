using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InputManager : MonoBehaviour
{
    public float lookSensitivity = 1f;

    [Tooltip("Used to flip the vertical input axis")]
    public bool invertY = false;

    Player_PlayerController playerControllerscript;
    bool isShootHeld;

    void Start()
    {
        playerControllerscript = GetComponent<Player_PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        isShootHeld = GetFireInputHeld();
    }

    //TODO: Lock input for Gameover, other
    public bool InputIsLocked()
    {
        return false;
    }

    //TODO: Put input names into a static class.
    public Vector3 GetMoveInput()
    {
        if (InputIsLocked())
            return Vector3.zero;

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f,
            Input.GetAxisRaw("Vertical"));

        // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
        move = Vector3.ClampMagnitude(move, 1);

        return move;
    }

    public bool OnTest()
    {
        return Input.GetButtonDown("Test");
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseLookAxis("Mouse X");
    }

    public float GetLookInputsVertical()
    {
        return GetMouseLookAxis("Mouse Y");
    }

    public bool IsJumping()
    {
        if (InputIsLocked())
            return false;
        return Input.GetButton("Jump");
    }

    public bool IsJumpHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButton("Jump");
    }

    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && isShootHeld;
    }

    public bool GetFireInputHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetAxis("Shoot") > 0f;
    }

    public bool GetAimInputHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetAxis("Aim") >= 0f;
    }

    public bool GetSprintInputHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButton("Sprint");
    }

    public bool GetCrouchInputDown()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonDown("Crouch");
    }

    public bool GetCrouchInputReleased()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonUp("Crouch");
    }

    public bool GetReloadButtonDown()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonDown("Reload");
    }
    public int GetSwitchWeaponInput()
    {
        if (InputIsLocked())
            return 0;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            return -1;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            return 1;
        else
            return 0;
    }

    public int GetSelectWeaponInput()
    {
        if (InputIsLocked())
            return 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            return 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            return 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            return 3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            return 4;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            return 5;
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            return 6;
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            return 7;
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            return 8;
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            return 9;
        else
            return 0;
    }

    float GetMouseLookAxis(string mouseInputName)
    {
        if (InputIsLocked())
            return 0;

        float i = Input.GetAxisRaw(mouseInputName);

        // handle inverting vertical input
        if (invertY && mouseInputName == "Mouse Y")
            i *= -1f;

        // apply sensitivity multiplier
        i *= lookSensitivity;

        // reduce mouse input amount
        i *= 0.01f;
        return i;
    }
    /// <summary>
    /// Returns true or false if the Proximity Voice button is pressed
    /// </summary>
    public bool GetVoiceMuteButtonIsPressed()
    {
        // Check if Mute button is being pressed
        return Input.GetButtonDown("ProximityVoiceMute");
    }
    /// <summary>
    /// Returns true or false if the Push to Talk button is pressed
    /// </summary>
    public bool GetPTTButtonIsPressed()
    {
        // Check if Push To Talk button is being pressed
        return Input.GetButton("PTTVoice");
    }
}
