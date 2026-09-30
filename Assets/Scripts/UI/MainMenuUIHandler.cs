using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    public CanvasGroup CanvasGroup { get; private set; }
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    public event Action startGameplayRequested;
    public event Action exitGameplayRequested;

    private void Awake()
    {
        CanvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(() => startGameplayRequested?.Invoke());
        startButton.onClick.AddListener(() => exitGameplayRequested?.Invoke());
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();
    }

    public void ShowScreen()
    {
        this.ShowWithFadeInAsync();
    }
    
    public void HideScreen()
    {
        this.HideWithFadeOutAsync();
    }
}