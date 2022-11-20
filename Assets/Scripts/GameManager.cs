using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using Tracks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Transform gameEndPosition;
    [SerializeField] private float targetFOV;
    [SerializeField] private CanvasGroup gameEndUI;
    [SerializeField] private Text gameEndTitle;
    [SerializeField] private Text gameEndScore;
    [SerializeField] private Text gameEndReason;
    [SerializeField] private Image buttonImg;
    [SerializeField] private Image button2Img;
    [SerializeField] private Text buttonText;
    [SerializeField] private Text button2Text;
    [SerializeField] private string victoryText;
    [SerializeField] private string defeatText;
    [SerializeField] private Image gameEndBg;
    [SerializeField] private Color lightColor;
    [SerializeField] private Color darkColor;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioClip defeatMusic;
    [SerializeField] private AudioSource musicSource;

    private void Start()
    {
        gameEndUI.alpha = 0;
    }

    /*
    private void Update()
    {
        if (!Input.GetKey(KeyCode.LeftControl)) return;
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameEnd(true, "debug");
        } else if (Input.GetKeyDown(KeyCode.F10))
        {
            GameEnd(false, "debug");
        }
    }
    */

    public void GameEnd(bool victory, string reason = "")
    {
        Time.timeScale = 0;
        
        vcam.transform.DOMove(gameEndPosition.position, 3f).SetUpdate(true).SetEase(Ease.OutQuart).SetLink(this.gameObject);
        DOVirtual.Float(vcam.m_Lens.FieldOfView, targetFOV, 3f, (v) => vcam.m_Lens.FieldOfView = v)
            .SetUpdate(true).SetEase(Ease.OutQuart).SetLink(this.gameObject);
        
        gameEndTitle.text = victory ? victoryText : defeatText;
        gameEndReason.text = reason;
        
        gameEndUI.gameObject.SetActive(true);
        gameEndUI.transform.localScale = Vector3.one * 0.7f;

        var list = TrackManager.Current.AccuracyList;
        gameEndScore.text = $"Accuracy: {((list.Any() ? list.Average() : 0) * 100f):F}%\nMaximum streak: {TrackManager.Current.MaxCombo}";
        
        gameEndBg.color = buttonText.color = button2Text.color = victory ? lightColor : darkColor;
        gameEndTitle.color = gameEndReason.color = gameEndScore.color = buttonImg.color = button2Img.color = victory ? darkColor : lightColor;

        foreach (var item in TrackManager.Current.Sources)
        {
            item.Stop();
        }

        musicSource.PlayOneShot(victory ? victoryMusic : defeatMusic);

        gameEndUI.DOFade(0, 0).SetLink(this.gameObject);
        gameEndUI.DOFade(1f, 2f).SetUpdate(true).SetDelay(.8f).SetEase(Ease.OutQuart).SetLink(this.gameObject);
        gameEndUI.transform.DOScale(1f, 3f).SetUpdate(true).SetDelay(.8f).SetEase(Ease.OutQuart).SetLink(this.gameObject);
    }

    public void Restart()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainMenu");
    }
}