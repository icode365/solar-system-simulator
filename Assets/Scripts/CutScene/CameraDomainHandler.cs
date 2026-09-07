using System;
using DG.Tweening;
using Scripts.CutScene;
using SpaceShip;
using UnityEngine;

public class CameraDomainHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private float cinematicFOV = 75f;

    private float initialFOV = 60f;
    private Vector3 initialCameraOffset;
    private Vector3 initialCameraRotation;

    public void OnEnable()
    {
        initialCameraOffset = _camera.transform.position;
        initialCameraRotation = _camera.transform.eulerAngles;
        initialFOV = _camera.fieldOfView;
        FindAnyObjectByType<CutsceneSequencer>().CameraCutSceneStartedEvent += StartSequence;
    }

    public void StartSequence(
        CutSceneStep stepConfig,
        CameraCutSceneContext ctx)
    {
        _camera = GetComponentInChildren<Camera>();
        GetComponent<CameraController>().enabled = false;

        // new DoTween.Sequence
        var cinematicSequence = DOTween.Sequence();
        cinematicSequence.Join(
            _camera.DOFieldOfView(
                cinematicFOV, 2f));

        // Sequence.spawn zoom-out
        cinematicSequence.Join(
            _camera.transform.DOMove(
                ctx.GetCamPositionWithOffset(
                    _camera.transform,
                    stepConfig.backOffset,
                    stepConfig.rightOffset),
                5f));

        // Sequence.spawn rotate correctly to include planet and spaceship
        cinematicSequence.Join(
            _camera.transform.DORotate(
                ctx.GetLookRotation(
                    _camera.transform,
                    stepConfig.backOffset,
                    stepConfig.rightOffset),
                5f));

        cinematicSequence.AppendInterval(5f);

        // Sequence.spawn rotate correctly to include planet and spaceship
        cinematicSequence.onComplete += WrapUp;
    }

    public void SequenceUpdate()
    {
    }

    public void WrapUp()
    {
        _camera.DOFieldOfView(
            initialFOV, 1f);
        _camera.transform.DOLocalMove(
            Vector3.zero, 1f);
        _camera.transform.DOLocalRotate(
            Quaternion.identity.eulerAngles, 1f);
        GetComponent<CameraController>().enabled = true;
    }
}