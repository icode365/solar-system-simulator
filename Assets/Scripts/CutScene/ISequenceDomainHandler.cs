using Scripts.CutScene;
using UnityEngine;

public interface ISequenceDomainHandler
{
    void StartSequence(CutSceneStep step, CutSceneContext ctx);
    void SequenceUpdate();
    void WrapUp();
}