using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.CutScene
{
    public class CutSceneContext
    {
        private readonly Dictionary<Type, object> _services = new();

        // Register a manager/system into the container
        public void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        // Resolve/Retrieve a manager inside an Action
        public T Resolve<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }

            Debug.LogWarning($"[CutsceneContext] Service of type {typeof(T).Name} not found!");
            return null;
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