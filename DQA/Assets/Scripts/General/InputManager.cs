using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class InputManager : MonoBehaviour {

	bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    
    private const float MinTimeBetweenMovement = .5f;
    private float timeMovement = 0f;
    private float analogSensitivty = .5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        CheckPlayerIndex();
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);

        if(state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released) {
            NotificationManager.PostNotification(Notifications.INPUT_A_PRESSED, null);
        }

        if(state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released) {
            NotificationManager.PostNotification(Notifications.INPUT_B_PRESSED, null);
        }

        if(state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released) {
            NotificationManager.PostNotification(Notifications.INPUT_X_PRESSED, null);
        }

        if(state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released) {
            NotificationManager.PostNotification(Notifications.INPUT_Y_PRESSED, null);
        }

        if(state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released) {
            NotificationManager.PostNotification(Notifications.INPUT_START_PRESSED, null);
        }

        if(state.Buttons.Back == ButtonState.Pressed && prevState.Buttons.Back == ButtonState.Released) {
            NotificationManager.PostNotification(Notifications.INPUT_BACK_PRESSED, null);
        }

        if(timeMovement > 0f) {
            timeMovement -= Time.deltaTime;
            if (timeMovement < 0)
                timeMovement = 0;
        }
        if(timeMovement == 0) {
            if(state.ThumbSticks.Left.X < -analogSensitivty || state.DPad.Left == ButtonState.Pressed) {
                NotificationManager.PostNotification(Notifications.INPUT_LEFT, null);
                timeMovement = MinTimeBetweenMovement;
            } else if(state.ThumbSticks.Left.X > analogSensitivty || state.DPad.Right == ButtonState.Pressed) {
                NotificationManager.PostNotification(Notifications.INPUT_RIGHT, null);
                timeMovement = MinTimeBetweenMovement;
            } else if(state.ThumbSticks.Left.Y < -analogSensitivty || state.DPad.Down == ButtonState.Pressed) {
                NotificationManager.PostNotification(Notifications.INPUT_DOWN, null);
                timeMovement = MinTimeBetweenMovement;
            } else if(state.ThumbSticks.Left.Y > analogSensitivty || state.DPad.Up == ButtonState.Pressed) {
                NotificationManager.PostNotification(Notifications.INPUT_UP, null);
                timeMovement = MinTimeBetweenMovement;
            }
        }
    }

    void CheckPlayerIndex() {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
    }

    /*
        string text = "Use left stick to turn the cube, hold A to change color\n";
        text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
        text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
        text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
        text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
        text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
        text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
        text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        
     * */
}
