using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineParticles : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] ParticleSystem dustParticles;
    [SerializeField] ParticleSystem sparkParticles;
    [SerializeField] ParticleSystem activeParticles;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] LineRenderer line;
    Mesh lineMesh;

    void Start()
    {
        activeParticles.Stop();
        lineMesh = new Mesh();
        line.BakeMesh(lineMesh, true);
        meshFilter.mesh = lineMesh;
    }

    void Update()
    {
        if(debug)
        {
            if(Input.GetKeyDown(KeyCode.Alpha6))
            {
                Activate();
            }
        }
    }

    public void Activate()
    {
        sparkParticles.Stop();
        activeParticles.Play();
    }

}
