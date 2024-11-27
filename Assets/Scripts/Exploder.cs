using System.Collections.Generic;
using UnityEngine;

public static class Exploder
{
    public static void TriggerExplosion(Vector3 center, float force, float radius, List<Cube> targetCubes)
    {
        if (targetCubes == null || targetCubes.Count == 0)
            return;

        foreach (Cube cube in targetCubes)
        {
            if (cube != null)
            {
                cube.AddExplosion(force, center, radius);
            }
        }
    }
}
