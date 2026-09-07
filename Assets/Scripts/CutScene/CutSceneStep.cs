using UnityEngine;

namespace Scripts.CutScene
{
    public enum StepType
    {
        Camera,
        UI,
        Ship
    }

    [System.Serializable]
    public class CutSceneStep
    {
        public StepType type;
        public float totalTime;
        public float rightOffset = 10f;
        public float backOffset = 10f;
    }

    public struct CutSceneContext
    {
        public UIContext uiContext;
        public CameraCutSceneContext cameraContext;
        
        public CutSceneContext(UIContext ui, CameraCutSceneContext camera)
        {
            uiContext = ui;
            cameraContext = camera;
        }
    }
    
    public struct CameraCutSceneContext
    {
        private Vector3 shipPosition;
        private Vector3 targetPosition;

        public CameraCutSceneContext(Vector3 ship, Vector3 target)
        {
            shipPosition = ship;
            targetPosition = target;
        }

        private Vector3 GetMidPoint()
        {
            return (shipPosition + targetPosition) / 2;
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

    public struct UIContext
    {
        public string text;
        
        public UIContext(string text)
        {
            this.text = text;
        }
    }
}