using System;
using DG.Tweening;
using Scripts.CutScene;
using SpaceShip;
using UnityEngine;

public class ShipDomainHandler : MonoBehaviour, ISequenceDomainHandler
{
    private SpaceShipController _controller;
    
    private void Start()
    {
        _controller = GetComponent<SpaceShipController>();    
    }

    public void StartSequence(CutSceneStep step, CutSceneContext ctx)
    {
        _controller.enabled = false;

        DOTween.Sequence();
        
    }

    public void SequenceUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void WrapUp()
    {
        throw new System.NotImplementedException();
    }
}
