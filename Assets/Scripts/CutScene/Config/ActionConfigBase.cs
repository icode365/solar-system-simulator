using DG.Tweening;
using Scripts.CutScene;
using UnityEngine;

public abstract class ActionConfigBase : ScriptableObject
{
    public bool playSimulatneously = false;
    protected Ease _ease = Ease.InOutCubic;

    public abstract Sequence GetSequence(CutSceneContext ctx);
    public abstract void WrapUp(CutSceneContext ctx);
}