using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty {

    static float secondsToMaxDifficulty = 30;

    public static float GetDifficultyPercent() {
        // Returns a value between 0 & 1 to indicate current difficulty using Mathf.Clamp01. (0 = default, 1 = maximum)
        // Time.timeSinceLevelLoad instead of Time.time, because Time.time is not reset after scene is reloaded
        return Mathf.Clamp01(Time.timeSinceLevelLoad/secondsToMaxDifficulty);
    }

}
