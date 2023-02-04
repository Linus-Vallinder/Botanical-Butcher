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

    public void Start()
    {
        activeParticles.Stop();
        if(line.positionCount == 0)
        { 
            SetLine(new Vector3[] {transform.position, transform.position});
        }
    }

    public void SetLine(Vector3[] positions)
    {
        line.positionCount = positions.Length;
        line.SetPositions(positions);
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

    public void SetParticles((ParticleSystem, ParticleSystem, ParticleSystem) particles)
    {
        dustParticles = particles.Item1;
        sparkParticles = particles.Item2;
        activeParticles = particles.Item3;
    }

    public void Activate()
    {
        sparkParticles.Stop();
        activeParticles.Play();
    }
}
