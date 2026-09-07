using DG.Tweening;
using Scripts.CutScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDomainHandler : MonoBehaviour
{
    [SerializeField] private Slider horizontalLine;
    [SerializeField] private Slider verticalLine;
    [SerializeField] private CanvasGroup labelGroup;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        horizontalLine.value = 0;
        verticalLine.value = 0;
        labelGroup.alpha = 0;
        // TODO FINISH THIS
        // var hieght = verticalLine.transform.GetComponent<Rect>().height;height
        // TODO : NOT LIKE THIS
        var v = FindFirstObjectByType<CutsceneSequencer>();
        v.UICutSceneStartedEvent
            += StartSequence;
    }


    private void StartSequence(CutSceneStep step, UIContext ctx)
    {
        Debug.Log("Reached UI");
        var cinematicSequence = DOTween.Sequence();
        // cinematicSequence.Append(
        //     verticalLine.DOValue(1, .5f));
        // cinematicSequence.Append(
        //     horizontalLine.DOValue(1, .5f));

        label.text = ctx.text;
        cinematicSequence.AppendInterval(2f);
        cinematicSequence.Append(
            labelGroup.DOFade(1, 1f));
        cinematicSequence.AppendInterval(3f);

        cinematicSequence.onComplete += WrapUp;
    }

    public void SequenceUpdate()
    {
    }

    public void WrapUp()
    {
        label.text = "";
        labelGroup.DOFade(1, 1f);
    }
}