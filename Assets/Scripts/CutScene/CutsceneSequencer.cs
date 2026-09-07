using System;
using Scripts.CutScene;
using UnityEngine;

public class CutsceneSequencer : MonoBehaviour
{
    private CutSceneConfig _config;
    private float cinematicFOV = 75f;
    private int currentStepIndex = 0;
    private CutSceneStep[] _steps;

    public event Action<CutSceneStep, CameraCutSceneContext> CameraCutSceneStartedEvent;
    public event Action<CutSceneStep, UIContext> UICutSceneStartedEvent;

    private bool Initialized => _config != null;

    public void Init(CutSceneConfig config)
    {
        _config = config;
        _steps = _config.GetSteps().ToArray();
    }

    public void StartCutScene(CutSceneContext ctx)
    {
        if (!Initialized)
        {
            Debug.LogError("Not Initialized");
            return;
        }


        switch (_steps[currentStepIndex].type)
        {
            case StepType.Camera:
                CameraCutSceneStartedEvent?.Invoke(
                    _config.steps[currentStepIndex],
                    ctx.cameraContext);
                currentStepIndex++;
                StartCutScene(ctx);
                break;

            case StepType.UI:
                Debug.Log("Reached UI");
                UICutSceneStartedEvent?.Invoke(
                    _config.steps[currentStepIndex],
                    ctx.uiContext);
                break;
        }

        // Finish Cutscne 
        //     1. Cutscene should be from one side ✅
        //         2. check the scaling planets should be a little bigger than ship ✅
        //             3. add name of the planet fade animation ✅
        //                 4. reset camera after finished ✅
    }
}