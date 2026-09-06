using DG.Tweening;
using Planets;
using SpaceShip;
using UnityEngine;

public class CinematicCutsceneManager : MonoBehaviour
{
    [SerializeField] private CameraController camera;
    private SpaceShipController _ship;
    private Orbiter _planet;

    private float cinematicFOV = 75f;
    private bool initialized = false;

    public void Init(CameraController _camera, SpaceShipController ship, Orbiter planet)
    {
        camera = _camera;
        _ship = ship;
        _planet = planet;

        if (camera && _ship && _planet != null)
            initialized = true;
    }

    public void StartCutScene()
    {
        if (!initialized)
        {
            Debug.Log("Not Initialized");
            return;
        }

        var camComp = camera.GetComponentInChildren<Camera>();
        var initialFOV = camComp.fieldOfView;

        // calculate the camera's target position
        Vector3 midPoint = (_ship.transform.position + _planet.GetPosition()) / 2;
        // Add offset to camera position
        Vector3 target = midPoint + camera.transform.forward * -20f;
        var lookRotation = Quaternion.LookRotation(midPoint - target);

        // new DoTween.Sequence
        var cinematicSequence = DOTween.Sequence();
        cinematicSequence.Append(camComp.DOFieldOfView(cinematicFOV, 2f));
        // Sequence.spawn zoom-out
        cinematicSequence.Join(camera.transform.DOMove(target, 5f));
        // Sequence.spawn rotate correctly to include planet and spaceship
        cinematicSequence.Join(camera.transform.DORotate(lookRotation.eulerAngles, 5f));

        cinematicSequence.onComplete += () => Debug.Log("Cinematic Complete");
    }
}