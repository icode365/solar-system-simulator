using DG.Tweening;
using Scripts.CutScene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDomainHandler : MonoBehaviour, ISequenceDomainHandler
{
    [SerializeField] private Image horizontalLine;
    [SerializeField] private Image verticalLine;
    [SerializeField] private TMP_Text label;

    void Start()
    {
    }

    public void StartSequence(CutSceneStep step, CutSceneContext ctx)
    {
        var cinematicSequence = DOTween.Sequence();
        cinematicSequence.Append(
           horizontalLine. )
    }

    public void SequenceUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void WrapUp()
    {
        throw new System.NotImplementedException();
    }
}