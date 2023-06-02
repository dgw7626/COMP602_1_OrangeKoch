/*

 ************************************************
 *                                              *
 * Primary Dev: 	Corey Knigth	            *
 * Student ID: 		21130891		            *
 * Course Code: 	COMP602_2023_S1             *
 * Assessment Item: Orange Koch                 *
 * 						                        *
 ************************************************

*/
using UnityEngine;

/// <summary>
/// This class acts as a buffer between the keyboard/mouse and other classes.
/// It sets a state in realtime for an input, then rather than having the input call a 
/// method, a method can check the keystate and act accordingly.
/// </summary>
public class Player_InputManager : MonoBehaviour
{
    public float lookSensitivity = 1f;

    [Tooltip("Used to flip the vertical input axis")]
    public bool invertY = false;

    Player_PlayerController playerControllerscript;
    bool isShootHeld;

    /// <summary>
    /// Initialize variables
    /// </summary>
    void Start()
    {
        // Set look sensitivity speed from the Options Window
        lookSensitivity = Game_RuntimeData.playerSettings.lookSensitivity;

        // Set Mouse Invert Y Axis from the Options Window
        invertY = !Game_RuntimeData.playerSettings.invertMouseYAxis;

        playerControllerscript = GetComponent<Player_PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Mouse button input handled after mouse movement. 
    /// </summary>
    void LateUpdate()
    {
        isShootHeld = GetFireInputHeld();
    }

    /// <summary>
    /// Prevents any input action from the player.
    /// </summary>
    /// <returns></returns>
    public bool InputIsLocked()
    {
        return false;
    }

    /// <summary>
    /// Converts vertical and horizontal keyboard movement into a single vector
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Horizontal mouse
    /// </summary>
    /// <returns></returns>
    public float GetLookInputsHorizontal()
    {
        return GetMouseLookAxis("Mouse X");
    }

    /// <summary>
    /// Vertical mouse
    /// </summary>
    /// <returns></returns>
    public float GetLookInputsVertical()
    {
        return GetMouseLookAxis("Mouse Y");
    }

    /// <summary>
    /// Spacebar
    /// </summary>
    /// <returns></returns>
    public bool IsJumping()
    {
        if (InputIsLocked())
            return false;
        return Input.GetButton("Jump");
    }

    /// <summary>
    /// When the shoot button is released
    /// </summary>
    /// <returns></returns>
    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && isShootHeld;
    }

    /// <summary>
    /// Is the fire button currentley down
    /// </summary>
    /// <returns></returns>
    public bool GetFireInputHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetAxis("Shoot") > 0f;
    }

    /// <summary>
    /// Right click mouse
    /// </summary>
    /// <returns></returns>
    public bool GetAimInputHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetAxis("Aim") >= 0f;
    }

    /// <summary>
    /// Spirnt button(shift)
    /// </summary>
    /// <returns></returns>
    public bool GetSprintInputHeld()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButton("Sprint");
    }

    /// <summary>
    /// Crouch button (ctrl)
    /// </summary>
    /// <returns></returns>
    public bool GetCrouchInputDown()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonDown("Crouch");
    }

    /// <summary>
    /// Crouch button (ctrl)
    /// </summary>
    /// <returns></returns>
    public bool GetCrouchInputReleased()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonUp("Crouch");
    }

    /// <summary>
    /// Reload (r)
    /// </summary>
    /// <returns></returns>
    public bool GetReloadButtonDown()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonDown("Reload");
    }

    /// <summary>
    /// Shoot
    /// </summary>
    /// <returns></returns>
    public bool GetFireInputDown()
    {
        if (InputIsLocked())
            return false;

        return Input.GetButtonDown("Shoot");
    }

    /// <summary>
    /// Mouse scroll
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Tab
    /// </summary>
    /// <returns></returns>
    public bool GetScoreBoardInputDown()
    {
        if(InputIsLocked())
            return false;
        
        return Input.GetButtonDown("Scoreboard");

    }

    /// <summary>
    /// Number keys
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Mouse movement
    /// </summary>
    /// <param name="mouseInputName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns true or false if the Quit Game button is pressed
    /// </summary>
    public bool GetQuitGameButtonIsPressed()
    {
        // Check if Quit Game button is being pressed
        return Input.GetButton("QuitGame");
    }
}