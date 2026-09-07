using System;
using Scripts.CutScene;
using UnityEngine;

public class CutsceneSequencer : MonoBehaviour
{
    private CutSceneConfig _config;
    private float cinematicFOV = 75f;
    private int currentStepIndex = 0;
    private CutSceneStep[] _steps;

    public event Action<CutSceneStep, CutSceneContext> CutSceneStartedEvent;

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
            Debug.Log("Not Initialized");
            return;
        }

        switch (_steps[currentStepIndex].type)
        {
            case StepType.Camera:
                CutSceneStartedEvent?.Invoke(_config.steps[currentStepIndex], ctx);
                break;
        }

        // Finish Cutscne 
        //     1. Cutscene should be from one side
        //         2. check the scaling planets should be a little bigger than ship
        //             3. add name of the planet fade animation
        //                 4. reset camera after finished
    }
}