using UnityEngine;
using System.Collections.Generic;

public class ParticleManager : MonoSingleton<ParticleManager>
{
    

    [SerializeField]
    private List<ParticleData> particles = new List<ParticleData>();

   

    private void Start()
    {
        CacheParticles();
    }

    #region CACHE

    private void CacheParticles()
    {
        foreach (var data in particles)
        {
            if (data.cacheParticle && data.particlePrefab != null)
            {
                ParticleSystem instance = Instantiate(data.particlePrefab, transform);

                instance.gameObject.SetActive(false); // prevent auto play
                data.cachedInstance = instance;
            }
        }
    }

    #endregion

    #region PLAY

    public void PlayParticle(string particleName)
    {
        PlayParticle(particleName, Vector3.zero, false);
    }

    public void PlayParticle(string particleName, Vector3 position)
    {
        PlayParticle(particleName, position, true);
    }

    private void PlayParticle(string particleName, Vector3 position, bool usePosition)
    {
        Debug.Log("playeing particle: "+particleName);
        ParticleData data = particles.Find(p => p.particleName == particleName);

        if (data == null)
        {
            Debug.LogWarning("Particle not found: " + particleName);
            return;
        }

        ParticleSystem ps = null;

        // If cached → use cached
        if (data.cacheParticle && data.cachedInstance != null)
        {
            ps = data.cachedInstance;
        }
        else
        {
            // Instantiate new one
            ps = Instantiate(data.particlePrefab);
        }

        if (usePosition)
        {
            ps.transform.position = position;
        }

        ps.gameObject.SetActive(true);
        ps.Play();

        // If NOT cached → auto destroy
        if (!data.cacheParticle)
        {
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }

    #endregion
}



[System.Serializable]
public class ParticleData
{
    public string particleName;
    public ParticleSystem particlePrefab;
    public bool cacheParticle = true;

    [HideInInspector]
    public ParticleSystem cachedInstance;
}
