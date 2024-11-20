using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    [SerializeField] RectTransform pauseTitlePanel;
    [SerializeField] float pauseTitleYPosBegin, pauseTitleYPosEnd;

    [SerializeField] RectTransform pauseContentPanel;
    [SerializeField] float pauseContentYPosBegin, pauseContentYPosEnd;

    [SerializeField] float tweenDuration;

    [SerializeField] CanvasGroup canvasGroup;

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        PauseIntro();
    }

    public async void ResumeGame() {
        await PauseOutro();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void PauseIntro() {
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        pauseTitlePanel.DOAnchorPosY(pauseTitleYPosEnd, tweenDuration).SetUpdate(true);
        pauseContentPanel.DOAnchorPosY(pauseContentYPosEnd, tweenDuration).SetUpdate(true);
    }

    async Task PauseOutro() {
        var canvasGroupTween = canvasGroup.DOFade(1, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        var pauseTitleTween = pauseTitlePanel.DOAnchorPosY(pauseTitleYPosBegin, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        var pauseContentTween = pauseContentPanel.DOAnchorPosY(pauseContentYPosBegin, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        await Task.WhenAll(canvasGroupTween, pauseTitleTween, pauseContentTween);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResumeGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
}
