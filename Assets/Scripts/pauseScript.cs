using UnityEngine;

public class pauseScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private GameObject pauseText, QuitInPauseButton, ResumeGameButton;
    private void onAwake()
    {
        pauseText.SetActive(false);
        QuitInPauseButton.SetActive(false);
        ResumeGameButton.SetActive(false);
    }
    public void TogglePause()
    {
        if (LogicScript.Instance.state == LogicScript.GameState.GameRun)
        {
            LogicScript.Instance.TogglePause();
            pauseText.SetActive(LogicScript.Instance.isPaused);
            QuitInPauseButton.SetActive(LogicScript.Instance.isPaused);
            ResumeGameButton.SetActive(LogicScript.Instance.isPaused);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
