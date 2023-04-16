using System;
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
    [SerializeField] private TMP_Text popupScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;
    
    [Header("Settings")] 
    [SerializeField] private float timerBarDuration = 1f;
    [SerializeField] private string menuSceneName;

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
        timerBarImage.sizeDelta = new Vector2(Screen.width, timerBarImage.sizeDelta.y);
        youWonTMP.gameObject.SetActive(false);
        youWonTMP.text = "You Win!";
        scoreTMP.gameObject.SetActive(false);
        popupObject.SetActive(false);
        
        StartCoroutine(LerpManager.Lerp(timerBarImage, Screen.width, 0f, timerBarDuration, LerpManager.LerpType.Linear));
    }
    
    public void PlayGame(int playerChoice)
    {
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
        
        timerBarImage.sizeDelta = new Vector2(Screen.width, timerBarImage.sizeDelta.y);
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
        youWonTMP.gameObject.SetActive(true);
        youWonTMP.text = "You Win!";
        scoreTMP.gameObject.SetActive(true);
        
        var currentScore = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.SetInt("HighScore", currentScore + 1);
        scoreTMP.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        
        StartCoroutine(WaitAndReset());
    }
    
    private void PlayerLoses()
    {
        popupObject.SetActive(true);
        popupScoreText.text = $"Score: {PlayerPrefs.GetInt("HighScore", 0).ToString()}";
    }
    
    private void ExitGame()
    {
        SceneManager.LoadScene(menuSceneName);
    }
    
    private void OnLerpComplete()
    {
        popupObject.SetActive(true);
        popupScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    
    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(2f);
        ResetUI();
    }

    private void OnDestroy()
    {
        LerpManager.OnComplete -= OnLerpComplete;
    }
}
