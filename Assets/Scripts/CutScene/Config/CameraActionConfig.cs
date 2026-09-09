using DG.Tweening;
using Planets;
using Scripts.CutScene;
using SpaceShip;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraActionConfig", menuName = "CutScene/Create Camera Action Config")]
public class CameraActionConfig : ActionConfigBase
{
    public float cinematicFOV;
    public float backOffset = 20f;
    public float rightOffset = 10f;
    public float preDelay = 1;
    public float stayDelay = 1;
    public float exitDelay = 1;

    private Transform shipTransform;
    private Vector3 targetPosition;
    private float initialFOV;

    public override Sequence GetSequence(CutSceneContext ctx)
    {
        shipTransform = ctx.Resolve<Transform>();

        var _camera = shipTransform.GetComponentInChildren<Camera>();
        initialFOV = _camera.fieldOfView;

        targetPosition = ctx.Resolve<Orbiter>().GetPosition();
        shipTransform.GetComponentInChildren<CameraController>().enabled = false;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(preDelay);
        sequence.Join(
            _camera.DOFieldOfView(
                cinematicFOV, stayDelay));

        // Sequence.spawn zoom-out
        sequence.Join(
            _camera.transform.DOMove(
                GetCamPositionWithOffset(
                    _camera.transform,
                    backOffset,
                    rightOffset),
                stayDelay));

        // Sequence.spawn rotate correctly to include planet and spaceship
        sequence.Join(
            _camera.transform.DORotate(
                GetLookRotation(
                    _camera.transform,
                    backOffset,
                    rightOffset),
                stayDelay));

        sequence.AppendInterval(exitDelay);

        return sequence;
    }

    public override void WrapUp(CutSceneContext ctx)
    {
        shipTransform = ctx.Resolve<Transform>();
        var _camera = shipTransform.GetComponentInChildren<Camera>();
        targetPosition = ctx.Resolve<Orbiter>().GetPosition();

        _camera.DOFieldOfView(
            initialFOV, 1f);
        _camera.transform.DOLocalMove(
            Vector3.zero, 1f);
        _camera.transform.DOLocalRotate(
            Quaternion.identity.eulerAngles, 1f);
        shipTransform.GetComponentInChildren<CameraController>().enabled = true;
    }


    private Vector3 GetMidPoint()
    {
        return (shipTransform.position + targetPosition) / 2;
    }

    public Vector3 GetLookRotation(
        Transform camTransform,
        float backOffset,
        float rightOffset)
    {
        return Quaternion.LookRotation(
                GetMidPoint() - GetCamPositionWithOffset(
                    camTransform, backOffset, rightOffset))
            .eulerAngles;
    }

    public Vector3 GetCamPositionWithOffset(
        Transform camTransform,
        float backOffset,
        float rightOffset)
    {
        return GetMidPoint() +
               (camTransform.forward * -backOffset) +
               (camTransform.right * rightOffset);
    }
}