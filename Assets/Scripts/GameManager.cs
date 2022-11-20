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
    [SerializeField] private Image buttonImg;
    [SerializeField] private Text buttonText;
    [SerializeField] private string victoryText;
    [SerializeField] private string defeatText;
    [SerializeField] private Image gameEndBg;
    [SerializeField] private Color lightColor;
    [SerializeField] private Color darkColor;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameEnd(true);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            GameEnd(false);
        }
    }

    public void GameEnd(bool victory)
    {
        Time.timeScale = 0;
        
        vcam.transform.DOMove(gameEndPosition.position, 3f).SetUpdate(true).SetEase(Ease.OutQuart);
        DOVirtual.Float(vcam.m_Lens.FieldOfView, targetFOV, 3f, (v) => vcam.m_Lens.FieldOfView = v)
            .SetUpdate(true).SetEase(Ease.OutQuart).SetLink(this.gameObject);
        
        gameEndTitle.text = victory ? victoryText : defeatText;
        gameEndUI.gameObject.SetActive(true);
        gameEndUI.transform.localScale = Vector3.one * 0.7f;

        var list = TrackManager.Current.AccuracyList;
        gameEndScore.text = $"Accuracy: {((list.Any() ? list.Average() : 0) * 100f):F}%";
        
        gameEndBg.color = buttonText.color = victory ? lightColor : darkColor;
        gameEndTitle.color = gameEndScore.color = buttonImg.color = victory ? darkColor : lightColor;

        gameEndUI.DOFade(0, 0);
        gameEndUI.DOFade(1f, 2f).SetUpdate(true).SetDelay(.8f).SetEase(Ease.OutQuart);
        gameEndUI.transform.DOScale(1f, 3f).SetUpdate(true).SetDelay(.8f).SetEase(Ease.OutQuart);
    }

    public void GameEndButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}