using UnityEngine;
using System.Collections;

public class Notifications {
    // FORMAT: List notification, then keys under it with no blank lines. After keys are listed (often times there will be no keys), then add a blank line
    public const string NOTIFICATION_LEVEL_LOADED = "levelLoadedNotification";

    public const string NOTIFICATION_PHASE_INCREMENTED = "phaseIncrementedNotification";
    public const string KEY_PHASE_INCREMENTED_PHASE = "phaseIncrementedPhase";

    public const string NOTIFICATION_ROUND_INCREMENTED = "roundIncrementedNotification";
    public const string KEY_ROUND_INCREMENTED_ROUND = "keyRoundIncrementedRound";

    public const string NOTIFICATION_PLAYER_MOVED_TO_ROOM = "playerMovedToRoom";
    public const string KEY_ROOM = "keyRoom";
    public const string KEY_PLAYER = "keyPlayer";

    public const string INPUT_A_PRESSED = "aPressed";
    public const string INPUT_B_PRESSED = "bPressed";
    public const string INPUT_X_PRESSED = "xPressed";
    public const string INPUT_Y_PRESSED = "yPressed";
    public const string INPUT_UP = "inputUp";
    public const string INPUT_DOWN = "inputDown";
    public const string INPUT_LEFT = "inputLeft";
    public const string INPUT_RIGHT = "inputRight";
    public const string INPUT_START_PRESSED = "startPressed";
    public const string INPUT_BACK_PRESSED = "backPressed";

}
