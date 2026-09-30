using System;
using SpaceShip;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField] private MainMenuUIHandler _mainMenuUIHandler;
    [SerializeField] private SpaceShipController _spaceShipController;
    [SerializeField] private BigBang _bigBang;

    private void Awake()
    {
        _mainMenuUIHandler.startGameplayRequested += StartGameplay;
    }

    private void Start()
    {
        _mainMenuUIHandler.ShowWithFadeInAsync();
    }

    private void StartGameplay()
    {
        _mainMenuUIHandler.HideScreen();
        _bigBang.Bang();
        _spaceShipController.StartShip();
    }
}