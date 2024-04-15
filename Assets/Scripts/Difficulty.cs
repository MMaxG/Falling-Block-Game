using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty {

    static float secondsToMaxDifficulty = 30;

    public static float GetDifficultyPercent(float currentTime) {
        // Returns a value between 0 & 1 to indicate current difficulty using Mathf.Clamp01. (0 = default, 1 = maximum)
        return Mathf.Clamp01(currentTime/secondsToMaxDifficulty);
    }

}
