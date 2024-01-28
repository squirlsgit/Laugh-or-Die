using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneM : MonoBehaviour
{
    public static SceneM instance;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject mainMenuScreen;
    [SerializeField]
    private GameObject gameWinScreen;
    [SerializeField]
    private GameObject gameContainer;
    
    private void Awake()
    {
        instance = this;
    }


    public void GameOver()
    {
        
        mainMenuScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        gameContainer.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void WinGame()
    {
        mainMenuScreen.SetActive(false);
        gameContainer.SetActive(false);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(true);
    }

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameContainer.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}