using System;
using SpaceShip;
using UnityEngine;

public class TargetDirectionResolver : MonoBehaviour
{
    [SerializeField] private SpaceShipController shipController;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private RectTransform targetMarker;
    [SerializeField] private RectTransform compassBarRect;

    private void LateUpdate()
    {
        if (!targetTransform) return;

        Vector3 playerFwd = Vector3.ProjectOnPlane(
            shipController.transform.forward, Vector3.up).normalized;
        Vector3 targetDir = Vector3.ProjectOnPlane(
            targetTransform.position - shipController.transform.position, Vector3.up).normalized;

        var relativeAngle = Vector3.SignedAngle(playerFwd, targetDir, Vector3.up);

        if (Mathf.Abs(relativeAngle) > 30f)
        {
            targetMarker.gameObject.SetActive(false);
        }
        else
        {
            targetMarker.gameObject.SetActive(true);
        }

        var normalizedX = relativeAngle / 60f;
        var xPos = normalizedX * compassBarRect.rect.width;

        targetMarker.anchoredPosition = new Vector2(targetMarker.rect.x, xPos);
    }
}