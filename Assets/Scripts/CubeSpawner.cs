using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _minCubes = 2;
    [SerializeField] private int _maxCubes = 6;

    private void Awake()
    {
        foreach (Cube cube in FindObjectsOfType<Cube>())
        {
            SubscribeToCube(cube);
        }        
    }

    private void SubscribeToCube(Cube cube)
    {
        cube.SplitAllowed += SpawnCubes;
    }

    public void SpawnCubes(Cube sourceCube)
    {
        int childCount = UnityEngine.Random.Range(_minCubes, _maxCubes + 1);
        List<Cube> newCubes = new List<Cube>();

        for (int i = 0; i < childCount; i++)
        {
            newCubes.Add(CreateCube(sourceCube));
        }

        foreach (Cube cube in newCubes)
        {
            cube.AddExplosion(sourceCube.baseExplosionForce, sourceCube.GetPosition(), sourceCube.baseExplosionRadius);
        }
    }

    private Cube CreateCube(Cube sourceCube)
    {
        int scaleReduceValue = 2;
        Vector3 newScale = sourceCube.GetScale() / scaleReduceValue;

        Cube newCube = Instantiate(_cubePrefab, sourceCube.GetPosition(), UnityEngine.Random.rotation);       

        newCube.ChangeScale(newScale);
        newCube.ChangeColor();
        newCube.UpdateSplitChance(sourceCube.GetCurrentSplitChance());
        newCube.GetComponent<Rigidbody>().useGravity = true;

        return newCube;
    }
}
