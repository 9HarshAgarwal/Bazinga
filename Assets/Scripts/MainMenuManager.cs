using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text highScoreText;
    
    [Header("Settings")]
    [SerializeField] private string gameSceneName;
    
    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        instructionsButton.onClick.AddListener(PlayInstructions);
        exitButton.onClick.AddListener(ExitGame);
        highScoreText.text = $"High Score: {PlayerPrefs.GetInt("HighScore", 0)}";
    }
    
    private void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void PlayInstructions()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=pIpmITBocfM");
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}
