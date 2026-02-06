using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemContainer : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems;

    public void PlayAll()
    {
        foreach (var ps in particleSystems)
        {
            if (ps)
                ps.Play();
        }
    }

    public void StopAll()
    {
        foreach (var ps in particleSystems)
        {
            if (ps)
                ps.Stop();
        }
    }


    private void OnValidate()
    {
        if (particleSystems.Count <= 0 || particleSystems == null)
        {
            var allParSys = GetComponentsInChildren<ParticleSystem>();
            particleSystems = new List<ParticleSystem>(allParSys);
        }
    }
}