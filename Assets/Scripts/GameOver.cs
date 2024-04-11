using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public GameObject gameOverScreen;
    public Text secondsSurvivedUI;
    public Text praiseText;
    public Image trophyImage;
    public Sprite goldTrophy;
    public Sprite silverTrophy;
    public Sprite bronzeTrophy;

    public int avg = 5;
    public int high = 10;
    public List<int> scores;

    bool gameOver;
    // Start is called before the first frame update
    void Start() {
        FindObjectOfType<PlayerController>().OnPlayerDeath += onGameOver;
    }

    // Update is called once per frame
    void Update() {
        if (gameOver) {
            if (Input.GetKeyDown (KeyCode.Space)) {
                SceneManager.LoadScene(0);
            }
        }
    }

    void onGameOver() {
        gameOverScreen.SetActive(true);
        int score = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        secondsSurvivedUI.text = score.ToString();
        praiseText.text = GeneratePraiseText(score);
        DisplayTrophy(EvaluateScore(score));
        gameOver = true;
        scores.Add(score);
        Debug.Log(scores.Count);
    }

    string GeneratePraiseText(int score) {
        Debug.Log(score);
        string[] lowStrings = {"You can do better", "Not great", "Try again", "Unlucky!", "Bad coding on that one", "Oof"};
        string[] avgStrings = {"Not bad", "Meh", "Aight", "I swear you had that", "Hey, thats pretty good!"};
        string[] highStrings = {"Hot damn!", "Thats gotta be a world record!", "You should go pro!", ""};

        string evalScore = EvaluateScore(score);

        if (evalScore == "low") {
            int index = Random.Range(0, lowStrings.Length);
            return lowStrings[index];
        } else if (evalScore == "avg") {
            int index = Random.Range(0, avgStrings.Length);
            return avgStrings[index];
        } else {
            int index = Random.Range(0, highStrings.Length);
            return highStrings[index];
        }
    }

    string EvaluateScore(int score) {
        if (score < avg) {
            return "low";
        } else if (score >= avg && score < high) {
            return "avg";
        } else {
            return "high";
        }
    }

    void DisplayTrophy(string score) {
        if (score == "low")
        {
            trophyImage.sprite = bronzeTrophy;
        }
        else if (score == "avg")
        {
            trophyImage.sprite = silverTrophy;
        }
        else if (score == "high")
        {
            trophyImage.sprite = goldTrophy;
        }
    }
}
