using System;
using Planets;
using SpaceShip;
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
    [SerializeField] private Image onScreenMarkerImage;
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
        onScreenMarkerImage = onScreenMarker?.GetComponent<Image>();
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
        var dotProduct = Vector3.Dot(ship.forward, offset);
        
        if (dotProduct > 0.75f)
        {
            onScreenMarkerImage.enabled = true;
        }
        else
        {
            onScreenMarkerImage.enabled = false;
            return;
        }

        var normalizedDistance = (distanceToTarget - onScreenMarkerMinVisiableThreshold) /
                                 (onScreenMarkerMaxVisiableThreshold -
                                  onScreenMarkerMinVisiableThreshold);

        var alpha = onScreenMarkerFade.Evaluate(normalizedDistance * dotProduct );
        var color = onScreenMarkerImage.color;
        onScreenMarkerImage.color = new Color(color.r, color.g, color.b, alpha);

        var lerpedPos = LerpPosition(onScreenMarker.position, targetScreenPoint);
        onScreenMarker.position = lerpedPos;
    }

    private Vector3 _currentVelocity;

    private Vector3 LerpPosition(Vector3 from, Vector3 to)
    {
        return Vector3.SmoothDamp(from, to, ref _currentVelocity, 0.2f);
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
            // onScreenMarker.position = markerPos;
        }
        else
        {
            marker.gameObject.SetActive(true);
            // onScreenMarker.gameObject.SetActive(false);

            // Calculate position on the tape
            if (isHorizontal)
            {
                float xPos = angle; // * compassRect.rect.width;
                marker.position = new Vector2(xPos, marker.position.y);
            }
            else
            {
                float yPos = angle; // * compassRect.rect.height;
                marker.position = new Vector2(marker.position.x, yPos);
            }
        }
    }

    public Vector3 GetTargetDirection()
    {
        return targetDir;
    }
}