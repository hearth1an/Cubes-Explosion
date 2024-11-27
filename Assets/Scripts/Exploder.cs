using System.Collections.Generic;
using UnityEngine;

public class Exploder: MonoBehaviour
{
    public void TriggerExplosion(Vector3 center, float force, float radius, List<Cube> cubes)
    {
        if (cubes == null || cubes.Count == 0)
            return;

        foreach (Cube cube in cubes)
        {
            if (cube != null)
            {
                cube.AddExplosion(force, center, radius);
            }
        }
    }

    public List<Cube> FindCubesInRadius(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        List<Cube> cubes = new();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Cube>(out Cube cube))
            {
                cubes.Add(cube);
            }
        }

        return cubes;
    }
}
