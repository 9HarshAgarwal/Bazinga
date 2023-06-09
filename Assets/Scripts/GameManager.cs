using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Computer UI Elements")] 
    [SerializeField] private TMP_Text computerStatusTMP;

    [Header("Divider UI Elements")] 
    [SerializeField] private RectTransform timerBarImage;
    [SerializeField] private TMP_Text gameResultTMP;
    [SerializeField] private TMP_Text scoreTMP;

    [Header("Player UI Elements")] 
    [SerializeField] private Button rockButton;
    [SerializeField] private Button paperButton;
    [SerializeField] private Button scissorsButton;
    [SerializeField] private Button lizardButton;
    [SerializeField] private Button spockButton;

    [Header("Popup UI Elements")] 
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text popupTitleTMP;
    [SerializeField] private TMP_Text popupScoreTMP;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text highScoreTMP;

    [Header("Settings")] 
    [SerializeField] private float timerBarDuration = 1f;
    [SerializeField] private string menuSceneName;
    [SerializeField] private Sprite rock2Sprite;
    [SerializeField] private Image rockImage;

    private int playerScore;

    private void Start()
    {
        ResetUI();

        rockButton.onClick.AddListener(() => PlayGame(0));
        paperButton.onClick.AddListener(() => PlayGame(1));
        scissorsButton.onClick.AddListener(() => PlayGame(2));
        lizardButton.onClick.AddListener(() => PlayGame(3));
        spockButton.onClick.AddListener(() => PlayGame(4));

        restartButton.onClick.AddListener(ResetUI);
        exitButton.onClick.AddListener(ExitGame);

        LerpManager.OnComplete += OnLerpComplete;
        
        // Just for fun
        if (PlayerPrefs.GetInt("HighScore", 0) > 0)
        {
            rockImage.sprite = rock2Sprite;
        }
    }

    private void ResetUI()
    {
        computerStatusTMP.text = "Waiting for You";
        gameResultTMP.gameObject.SetActive(false);
        gameResultTMP.text = "You Win!";
        scoreTMP.gameObject.SetActive(false);
        popupObject.SetActive(false);

        timerBarImage.localScale = new Vector3(1, 1, 1);

        StartCoroutine(LerpManager.Lerp(timerBarImage, 1, 0f, timerBarDuration, LerpManager.LerpType.Linear));
    }

    public void PlayGame(int playerChoice)
    {
        StopAllCoroutines();
        timerBarImage.localScale = new Vector3(0, 1, 1);

        var computerChoice = Random.Range(0, 5);

        computerStatusTMP.text = computerChoice switch
        {
            0 => "Rock",
            1 => "Paper",
            2 => "Scissors",
            3 => "Lizard",
            4 => "Spock",
            _ => "Error"
        };

        if (playerChoice == computerChoice)
        {
            popupTitleTMP.text = "It's a tie!";
            PlayerTies();
        }
        else
        {
            switch (playerChoice, computerChoice)
            {
                case (0, 2):
                    gameResultTMP.text = "Rock crushes Scissors";
                    PlayerWins();
                    break;
                case (2, 0):
                    popupTitleTMP.text = "Rock crushes Scissors";
                    PlayerLoses();
                    break;
                case (0, 3):
                    gameResultTMP.text = "Rock crushes Lizard";
                    PlayerWins();
                    break;
                case (3, 0):
                    popupTitleTMP.text = "Rock crushes Lizard";
                    PlayerLoses();
                    break;
                case (1, 0):
                    gameResultTMP.text = "Paper covers Rock";
                    PlayerWins();
                    break;
                case (0, 1):
                    popupTitleTMP.text = "Paper covers Rock";
                    PlayerLoses();
                    break;
                case (1, 4):
                    gameResultTMP.text = "Paper disproves Spock";
                    PlayerWins();
                    break;
                case (4, 1):
                    popupTitleTMP.text = "Paper disproves Spock";
                    PlayerLoses();
                    break;
                case (2, 1):
                    gameResultTMP.text = "Scissors cuts Paper";
                    PlayerWins();
                    break;
                case (1, 2):
                    popupTitleTMP.text = "Scissors cuts Paper";
                    PlayerLoses();
                    break;
                case (2, 3):
                    gameResultTMP.text = "Scissors decapitates Lizard";
                    PlayerWins();
                    break;
                case (3, 2):
                    popupTitleTMP.text = "Scissors decapitates Lizard";
                    PlayerLoses();
                    break;
                case (3, 1):
                    gameResultTMP.text = "Lizard eats Paper";
                    PlayerWins();
                    break;
                case (1, 3):
                    popupTitleTMP.text = "Lizard eats Paper";
                    PlayerLoses();
                    break;
                case (3, 4):
                    gameResultTMP.text = "Lizard poisons Spock";
                    PlayerWins();
                    break;
                case (4, 3):
                    popupTitleTMP.text = "Lizard poisons Spock";
                    PlayerLoses();
                    break;
                case (4, 0):
                    gameResultTMP.text = "Spock vaporizes Rock";
                    PlayerWins();
                    break;
                case (0, 4):
                    popupTitleTMP.text = "Spock vaporizes Rock";
                    PlayerLoses();
                    break;
                case (4, 2):
                    gameResultTMP.text = "Spock smashes Scissors";
                    PlayerWins();
                    break;
                case (2, 4):
                    popupTitleTMP.text = "Spock smashes Scissors";
                    PlayerLoses();
                    break;
                default:
                    Debug.LogError("Bazinga! You shouldn't be here.");
                    ResetUI();
                    break;
            }
        }
    }


    private void PlayerTies()
    {
        gameResultTMP.gameObject.SetActive(true);
        gameResultTMP.text = "Tie!";
        scoreTMP.gameObject.SetActive(true);
        scoreTMP.text = playerScore.ToString();

        StartCoroutine(WaitAndReset());
    }

    private void PlayerWins()
    {
        playerScore++;

        gameResultTMP.gameObject.SetActive(true);
        scoreTMP.gameObject.SetActive(true);
        scoreTMP.text = playerScore.ToString();

        var currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentHighScore < playerScore)
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
        }

        StartCoroutine(WaitAndReset());
    }

    private void PlayerLoses()
    {
        ShowLostPopup();
    }

    private void ExitGame()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void OnLerpComplete()
    {
        popupTitleTMP.text = "Timer ran out!";
        ShowLostPopup();
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(2f);
        ResetUI();
    }

    private void ShowLostPopup()
    {
        popupObject.SetActive(true);
        popupScoreTMP.text = "Score: " + playerScore;
        highScoreTMP.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    private void OnDestroy()
    {
        LerpManager.OnComplete -= OnLerpComplete;
    }
}