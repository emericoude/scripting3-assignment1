using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [Header("Sections")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _inGame;
    [SerializeField] private GameObject _end;

    [Header("End")]
    [SerializeField] private TextMeshProUGUI _endPointsUGUI;
    [SerializeField] private TextMeshProUGUI _endDeathUGUI;
    [SerializeField] private TextMeshProUGUI _timeToFinishUGUI;

    [Header("Scene Loader Buttons")]
    [SerializeField] private Button[] _playButtons;
    [SerializeField] private Button[] _quitButtons;

    [Header("Points")]
    [SerializeField] private TextMeshProUGUI _pointsUGUI;

    [Header("Interaction UGUI")]
    [SerializeField] private TextMeshProUGUI _interactTMP;
    [SerializeField] private Image _interactionProgressImage;

    private void Start()
    {
        InitNavigationButtons();
        InitInteractionUGUI();
        InitPointsUGUI();
    }

    #region Scene Navigation

    private void InitNavigationButtons()
    {
        UnityAction loadLevelOne = () =>
        {
            SceneLoader.LoadScene(SceneLoader.LevelOneSceneName);
            GameManager.Instance.RunTimer = 0;
            GameManager.Instance.TimerEnabled = true;
            EnableInGameUGUI();
        };

        foreach (Button button in _playButtons)
            button.onClick.AddListener(loadLevelOne);

        UnityAction quitApplication = () => SceneLoader.QuitGame();
        foreach (Button button in _quitButtons)
            button.onClick.AddListener(quitApplication);
    }

    public void InitEndScene()
    {
        GameManager.Instance.TimerEnabled = false;

        _endPointsUGUI.text = GameManager.Instance.CurrentPoints.ToString();
        _timeToFinishUGUI.text = $"Time: {GameManager.Instance.RunTimer}s";
        _endDeathUGUI.text = "temp";
        EnableEndUGUI();
    }

    public void InitMenuScene()
    {
        EnableMenuUGUI();
    }

    private void EnableMenuUGUI()
    {
        _inGame.SetActive(false);
        _end.SetActive(false);

        _menu.SetActive(true);
    }

    private void EnableInGameUGUI()
    {
        _end.SetActive(false);
        _menu.SetActive(false);

        _inGame.SetActive(true);
    }

    private void EnableEndUGUI()
    {
        _inGame.SetActive(false);
        _menu.SetActive(false);

        _end.SetActive(true);
    }

    #endregion
    #region Points

    private void InitPointsUGUI()
    {
        _pointsUGUI.text = "0";
    }

    public void UpdatePointsUGUI(int pointAmount)
    {
        _pointsUGUI.text = pointAmount.ToString();
    }

    #endregion
    #region Interaction GUI

    private void InitInteractionUGUI()
    {
        UpdateInteractionProgressChange(0);
        SetInteractionText("");
    }

    public void SetInteractionText(string text)
    {
        _interactTMP.text = text;
    }

    public void UpdateInteractionProgressChange(float progress)
    {
        _interactionProgressImage.fillAmount = progress;
    }

    #endregion
}
