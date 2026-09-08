using System;
using DG.Tweening;
using Scripts.CutScene;
using UnityEngine;

public class CutsceneSequencer : MonoBehaviour
{
    private CutSceneConfig _config;
    private float cinematicFOV = 75f;
    private int currentStepIndex = 0;
    private CutSceneStep[] _steps;

    public event Action<CutSceneStep, CameraCutSceneContext, Sequence> CameraCutSceneStartedEvent;
    public event Action<CutSceneStep, UIContext, Sequence> UICutSceneStartedEvent;

    public bool Initialized => _config != null;
    private Sequence masterSequence;

    public void Init(CutSceneConfig config)
    {
        _config = config;
        _steps = _config.GetSteps().ToArray();

        masterSequence = DOTween.Sequence();
        masterSequence.SetAutoKill(false);
        masterSequence.onComplete += OnSequenceCompleted;
    }

    private void OnSequenceCompleted()
    {
        Debug.Log("Finished Sequence.");
    }

    public void StartCutScene(CutSceneContext ctx)
    {
        if (!Initialized)
        {
            Debug.LogError("Not Initialized");
            return;
        }

        if (currentStepIndex >= _steps.Length)
        {
            masterSequence.Play();
            return;
        }

        switch (_steps[currentStepIndex].type)
        {
            case StepType.Camera:
                CameraCutSceneStartedEvent?.Invoke(
                    _config.steps[currentStepIndex],
                    ctx.cameraContext,
                    masterSequence);
                break;

            case StepType.UI:
                // Debug.Log("Reached UI");
                UICutSceneStartedEvent?.Invoke(
                    _config.steps[currentStepIndex],
                    ctx.uiContext,
                    masterSequence);
                break;
        }

        currentStepIndex++;
        StartCutScene(ctx);
    }
}