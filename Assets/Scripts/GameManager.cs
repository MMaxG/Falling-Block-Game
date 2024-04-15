using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject player;
    public GameObject gameoverUI;

    public GameObject gameUI;
    public GameObject tutorialUI;
    public Text aliveScoreText;
    public Animator textAnimator;
    public Animator playerAnimator;
    int previousScore = 0;
    public Text pointsText;
    public Text praiseText;
    public Text recordText;

    public Spawner spawner;

    public Image trophyImage;
    public Sprite goldTrophy;
    public Sprite silverTrophy;
    public Sprite bronzeTrophy;

    // Text Animation stuff
    float minFontSize = 50f;
    float maxFontSize = 125f;
    float maxScore = 45; //

    public float backgroundTime;

    float timer = 0f;
    int score = 0;
    int avg = 15;
    int high = 45;
    public List<int> scores;

    bool gameOver = false;
    bool inTutorial = true;
    
    void Start() {
        FindObjectOfType<PlayerController>().OnPlayerDeath += onGameOver;
        player = FindAnyObjectByType<PlayerController>().gameObject;
        textAnimator = aliveScoreText.GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();
        LoadTutorial();
    }

    void LoadTutorial() {
        // Turn on tutorial UI
        tutorialUI.SetActive(true);

        // Hide unneccessary other UI elements
        gameUI.SetActive(false);
        gameoverUI.SetActive(false);
    }

    void EndTutorial() {
        tutorialUI.SetActive(false);
        gameUI.SetActive(true);
        inTutorial = false;
        StartNewGame(4);
    }

    void Update() {
        // If tutorial is active, wait until player presses one of the movement keys to end it and start game
        if (inTutorial) {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
                EndTutorial();
            }
        }
        
        // Update backgroundTimer to keep track of how many seconds the current game has been going for
        
        
        // In game mechanics
        if (!inTutorial && !gameOver) {
            backgroundTime += Time.deltaTime;
            aliveScoreText.text = score.ToString();
            timer += Time.deltaTime; 
            // Update score
            if (timer >= 1) {
                score += 1;
                timer = 0;
            }
        }
        
        // Play animation for score text when it changes
        float t = Mathf.Clamp01((float)score / maxScore);
        float lerpedFontSize = Mathf.Lerp(minFontSize, maxFontSize, t);
        aliveScoreText.fontSize = (int)lerpedFontSize;

        if (score != previousScore) {
            if (textAnimator != null) {
                textAnimator.SetTrigger("TriggerAnimation");
            }
            previousScore = score;
        }

        if (gameOver) {
            if (Input.GetKeyDown (KeyCode.Space)) {
                StartNewGame(3);
            }
        }
    }

    void StartNewGame(int spawnerDelay) {
        spawner.Deactivate();
        player.SetActive(true);
        player.transform.position = new Vector3(0f, -4f, 0f);

        gameUI.SetActive(true);
        gameoverUI.SetActive(false);

        score = 0;
        backgroundTime = 0;
        timer = 0;
        spawner.Activate(spawnerDelay);

        gameOver = false;
        inTutorial = false;
    }



    void onGameOver() {
        // Show relevant UI elements
        gameUI.SetActive(false);
        gameoverUI.SetActive(true);
        pointsText.text = score.ToString();
        praiseText.text = GeneratePraiseText(score);
        float randomHue = Random.Range(0f, 360f);
        praiseText.color = Color.HSVToRGB(randomHue / 360f, 0.79f, 0.70f);
        praiseText.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-3f, 3f));
        FitTextToContainer(praiseText);

        gameOver = true;

        DisplayTrophy(EvaluateScore(score));
        scores.Add(score);
        recordText.text = "Record:\n" + CalculateHighscore().ToString();
    }

    int CalculateHighscore() {
        int highscore = scores[0];

        for (int i = 1; i < scores.Count; i++) {
            if (scores[i] > highscore)
            {
                highscore = scores[i]; // Update the highestScore if a higher value is found
            }
        }

        return highscore;
    }

    string GeneratePraiseText(int score) {
        string[] lowStrings = {"You can do better", "Not great", "Try moving", "Uuh...", "Unlucky!", "Bad coding on that one", "Oof", "Bronze is alright... right?", "Thats impressively low", "Diagnosis: Skill Issue", "lag", "You're supposed to AVOID the boxes..."};
        string[] avgStrings = {"Not bad", "Meh", "Aight", "I swear you had that", "Hey, thats pretty good!", "I heard 45 is a GOLDEN trophy", "Nice. Not thrilling, but nice", "Bro, that was like less than 1 second from gold! Right?", "I saw it, the vision", "mid"};
        string[] highStrings = {"Hot damn!", "Thats gotta be a world record!", "You should go pro!", "You beat the game!", "GOOOooOooOoOOLD!!"};

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

    void FitTextToContainer(Text text)
    {
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = text.GetGenerationSettings(rectTransform.rect.size);

        // Calculate the preferred height based on the text's preferred settings
        float preferredHeight = textGen.GetPreferredHeight(text.text, generationSettings);

        // Get the current font size
        int fontSize = text.fontSize;

        // If the preferred height is greater than the current height, decrease font size
        while (preferredHeight > rectTransform.rect.height && fontSize > 1)
        {
            fontSize--;
            text.fontSize = fontSize;
            preferredHeight = textGen.GetPreferredHeight(text.text, generationSettings);
        }
    }
}
