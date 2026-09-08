using DG.Tweening;
using Scripts.CutScene;
using SpaceShip;
using UnityEngine;

public class ShipDomainHandler : MonoBehaviour
{
    private SpaceShipController _controller;

    private void Start()
    {
        _controller = GetComponent<SpaceShipController>();
        FindAnyObjectByType<CutsceneSequencer>().CameraCutSceneStartedEvent +=
            StartSequence;
    }

    public void StartSequence(
        CutSceneStep step, CameraCutSceneContext ctx,
        Sequence sequence)
    {
        _controller.enabled = false;
        sequence.onComplete += WrapUp;
    }

    public void SequenceUpdate()
    {
    }

    public void WrapUp()
    {
        _controller.enabled = true;
    }
}