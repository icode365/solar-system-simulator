using System;
using DG.Tweening;
using Scripts.CutScene;
using UnityEngine;

public class CutsceneDirector : MonoBehaviour
{
    public event Action OnCutSceneCompleted;

    public void PlayTimeline(CinematicTimelineConfig timelineConfig, CutSceneContext ctx)
    {
        BuildTimelineSequence(timelineConfig, ctx);
    }

    public void BuildTimelineSequence(CinematicTimelineConfig timelineConfig, CutSceneContext ctx)
    {
        var masterSequence = DOTween.Sequence();
        masterSequence.SetAutoKill(false);

        var actions = timelineConfig.GetActions();

        foreach (var action in actions)
        {
            var domainSequence = action.GetSequence(ctx);

            if (action.playSimulatneously)
            {
                masterSequence.Join(domainSequence);
            }
            else
            {
                masterSequence.Append(domainSequence);
            }

            masterSequence.onComplete += () => action.WrapUp(ctx);
        }

        masterSequence.onComplete += OnSequenceCompleted;
        masterSequence.Play();
    }

    private void OnSequenceCompleted()
    {
        OnCutSceneCompleted?.Invoke();
    }
}