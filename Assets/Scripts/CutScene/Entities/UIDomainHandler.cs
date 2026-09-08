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


    private void StartSequence(
        CutSceneStep step, UIContext ctx,
        Sequence sequence)
    {
        Debug.Log("Reached UI");
        // cinematicSequence.Append(
        //     verticalLine.DOValue(1, .5f));
        // cinematicSequence.Append(
        //     horizontalLine.DOValue(1, .5f));

        label.text = ctx.text;
        sequence.AppendInterval(2f);
        sequence.Append(
            labelGroup.DOFade(1, 1f));
        sequence.AppendInterval(3f);

        sequence.onComplete += WrapUp;
    }

    public void SequenceUpdate()
    {
    }

    public void WrapUp()
    {
        label.text = "";
        labelGroup.DOFade(0, 1f);
    }
}