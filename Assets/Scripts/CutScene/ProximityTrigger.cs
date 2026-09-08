using System;
using Planets;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProximityTrigger : MonoBehaviour
{
    private Orbiter _orbiter;
    public static event Action<Orbiter> OnProximityEnter;

    private void Awake()
    {
        if (name == "Sun") enabled = false;
        var collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 1f;
    }

    public void SetOrbiter(Orbiter orbiter)
    {
        _orbiter = orbiter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnProximityEnter?.Invoke(_orbiter);
    }
}