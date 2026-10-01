using Planets;
using SpaceShip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetDirectionResolver : MonoBehaviour
{
    [SerializeField] private SpaceShipController shipController;
    [SerializeField] private Orbiter targetTransform;
    [SerializeField] private RectTransform horizontalTargetMarker;
    [SerializeField] private RectTransform horizontalCompassBarRect;

    [SerializeField] private RectTransform verticalTargetMarker;
    [SerializeField] private RectTransform verticalCompassBarRect;

    [SerializeField] private RectTransform onScreenMarker;
    [SerializeField] private TMP_Text onScreenLabel;
    [SerializeField] private CanvasGroup onScreenMarkerGroup;
    public float smoothTime = 0.05f;
    public AnimationCurve onScreenMarkerFade;
    public float onScreenMarkerMinVisiableThreshold = 20f;
    public float onScreenMarkerMaxVisiableThreshold = 100f;

    [SerializeField] private float horizontalFOV = 30f; // Field of view for horizontal tape
    [SerializeField] private float verticalFOV = 30f; // Field of view for vertical tape

    private Vector3 targetDir;
    private Vector3 targetScreenPoint;
    private Camera playerCam;
    private float distanceToTarget;

    private void Awake()
    {
        if (onScreenMarkerGroup == null)
            onScreenMarkerGroup = onScreenMarker?.GetComponent<CanvasGroup>();
    }

    public void SetDistanceToTarget(float distance) => distanceToTarget = distance;

    public void SetTarget(Orbiter target) => targetTransform = target;

    public void SetCamera(Camera cam) => playerCam = cam;

    private void LateUpdate()
    {
        if (targetTransform == null || playerCam == null) return;

        var offset = targetTransform.GetPosition() - shipController.transform.position;

        // Calculate direction to target
        targetDir = offset.normalized;

        targetScreenPoint = playerCam.WorldToScreenPoint(targetTransform.GetPosition());
        UpdateOnScreenMarker(shipController.transform, targetDir);

        // Update horizontal marker
        UpdateMarker(horizontalTargetMarker, horizontalCompassBarRect, targetScreenPoint.x, targetScreenPoint, true);

        // Update vertical marker
        UpdateMarker(verticalTargetMarker, verticalCompassBarRect, targetScreenPoint.y, targetScreenPoint, false);
    }

    private void UpdateOnScreenMarker(Transform ship, Vector3 offset)
    {
        if (targetTransform == null || targetTransform.Data == null) return;
 
        var dotProduct = Vector3.Dot(ship.forward, offset);

        if (dotProduct > 0.75f)
        {
            onScreenMarkerGroup.enabled = true;
        }
        else
        {
            onScreenMarkerGroup.enabled = false;
            return;
        }

        var normalizedDistance = (distanceToTarget - targetTransform.Data.radius + onScreenMarkerMinVisiableThreshold) /
                                 (onScreenMarkerMaxVisiableThreshold -
                                  onScreenMarkerMinVisiableThreshold);

        var alpha = onScreenMarkerFade.Evaluate(normalizedDistance * dotProduct);
        onScreenMarkerGroup.alpha = alpha;

        var lerpedPos = LerpPosition(onScreenMarker.position, targetScreenPoint);
        onScreenMarker.position = lerpedPos;
        UpdateDistanceLabel(targetTransform.Data.bodyName, distanceToTarget);
    }

    private void UpdateDistanceLabel(string orbiterName, float distance)
    {
        onScreenLabel.text = orbiterName + "\n " + distance.ToString("00000.00") + "km";
    }

    private Vector3 _currentVelocity;

    private Vector3 LerpPosition(Vector3 from, Vector3 to)
    {
        return Vector3.SmoothDamp(from, to, ref _currentVelocity, smoothTime);
    }

    private void UpdateMarker(RectTransform marker, RectTransform compassRect, float angle, Vector2 markerPos,
        bool isHorizontal)
    {
        if (marker == null || compassRect == null) return;

        // Check if target is within field of view
        if (Vector3.Dot(playerCam.transform.forward, targetTransform.GetPosition()) > 0.5)
        {
            marker.gameObject.SetActive(false);
            onScreenMarker.gameObject.SetActive(true);
        }
        else
        {
            marker.gameObject.SetActive(true);

            // Calculate position on the tape
            if (isHorizontal)
            {
                float xPos = angle;
                marker.position = new Vector2(xPos, marker.position.y);
            }
            else
            {
                float yPos = angle;
                marker.position = new Vector2(marker.position.x, yPos);
            }
        }
    }

    public Vector3 GetTargetDirection()
    {
        return targetDir;
    }
}