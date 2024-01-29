using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
        SceneManager.LoadScene("Game Over");
        // mainMenuScreen.SetActive(false);
        // gameWinScreen.SetActive(false);
        // gameContainer.SetActive(false);
        // gameOverScreen.SetActive(true);
    }

    public void WinGame()
    {
        SceneManager.LoadScene("Game Win");
        // mainMenuScreen.SetActive(false);
        // gameContainer.SetActive(false);
        // gameOverScreen.SetActive(false);
        // gameWinScreen.SetActive(true);
    }

    public float introVoiceLineDelay = 3f;
    public void StartGame()
    {
        SceneManager.LoadScene("Game Start");
        // mainMenuScreen.SetActive(false);
        // gameWinScreen.SetActive(false);
        // gameOverScreen.SetActive(false);
        // gameContainer.SetActive(true);
    }
    
    public IEnumerator WaitSecondsUntilLoadWin()
    {
        yield return new WaitForSeconds(4);
        SceneM.instance.WinGame();
    }
    
    public IEnumerator WaitSecondsUntilLoadLose()
    {
        yield return new WaitForSeconds(4);
        SceneM.instance.GameOver();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Dismemberment 2");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BlindPlayer()
    {
        
    }
}