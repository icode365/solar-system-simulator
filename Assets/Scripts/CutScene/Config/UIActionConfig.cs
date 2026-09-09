using DG.Tweening;
using Scripts.CutScene;
using UnityEngine;

[CreateAssetMenu(fileName = "UIActionConfig", menuName = "CutScene/Create UI Action Config")]
public class UIActionConfig : ActionConfigBase
{
    public string text;
    public float preDelay = 1;
    public float stayDelay = 1;
    public float exitDelay = 1;

    public override Sequence GetSequence(CutSceneContext ctx)
    {
        var hudController = ctx.Resolve<HUDController>();
        var label = hudController.label;
        var labelGroup = hudController.labelGroup;

        label.text = ctx.Resolve<string>();

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(preDelay);
        sequence.Append(labelGroup
                .DOFade(1, stayDelay))
            .SetEase(_ease);
        sequence.AppendInterval(exitDelay);

        return sequence;
    }

    public override void WrapUp(CutSceneContext ctx)
    {
        var hudController = ctx.Resolve<HUDController>();
        var label = hudController.label;
        var labelGroup = hudController.labelGroup;
        label.text = "";
        labelGroup.DOFade(0, 1f);
    }
}