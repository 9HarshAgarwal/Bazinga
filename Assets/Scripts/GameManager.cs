using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Computer UI Elements")] 
    [SerializeField] private TMP_Text computerStatusText;

    [Header("Divider UI Elements")] 
    [SerializeField] private RectTransform timerBarImage;
    [SerializeField] private TMP_Text youWonTMP;
    [SerializeField] private TMP_Text scoreTMP;
    
    [Header("Player UI Elements")] 
    [SerializeField] private Button rockButton;
    [SerializeField] private Button paperButton;
    [SerializeField] private Button scissorsButton;
    [SerializeField] private Button lizardButton;
    [SerializeField] private Button spockButton;
    
    [Header("Popup UI Elements")] 
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text popupTitleText;
    [SerializeField] private TMP_Text popupScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text highScoreText;
    
    [Header("Settings")] 
    [SerializeField] private float timerBarDuration = 1f;
    [SerializeField] private string menuSceneName;
    
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
    }
    
    private void ResetUI()
    {
        computerStatusText.text = "Waiting for You";
        youWonTMP.gameObject.SetActive(false);
        youWonTMP.text = "You Win!";
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

        computerStatusText.text = computerChoice switch
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
            PlayerTies();
        }
        else if ((playerChoice == 0 && computerChoice == 2) ||
                 (playerChoice == 0 && computerChoice == 3) ||
                 (playerChoice == 1 && computerChoice == 0) ||
                 (playerChoice == 1 && computerChoice == 4) ||
                 (playerChoice == 2 && computerChoice == 1) ||
                 (playerChoice == 2 && computerChoice == 3) ||
                 (playerChoice == 3 && computerChoice == 1) ||
                 (playerChoice == 3 && computerChoice == 4) ||
                 (playerChoice == 4 && computerChoice == 0) ||
                 (playerChoice == 4 && computerChoice == 2))
        {
            PlayerWins();
        }
        else
        {
            PlayerLoses();
        }
    }
    
    private void PlayerTies()
    {
        youWonTMP.gameObject.SetActive(true);
        youWonTMP.text = "Tie!";
        scoreTMP.gameObject.SetActive(true);
        scoreTMP.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        
        StartCoroutine(WaitAndReset());
    }
    
    private void PlayerWins()
    {
        playerScore++;
        
        youWonTMP.gameObject.SetActive(true);
        youWonTMP.text = "You Win!";
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
        popupTitleText.text = "You Lost!";
        ShowLostPopup();
    }
    
    private void ExitGame()
    {
        SceneManager.LoadScene(menuSceneName);
    }
    
    private void OnLerpComplete()
    {
        popupTitleText.text = "Timer ran out!";
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
        popupScoreText.text = "Score: " + playerScore;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    private void OnDestroy()
    {
        LerpManager.OnComplete -= OnLerpComplete;
    }
}
