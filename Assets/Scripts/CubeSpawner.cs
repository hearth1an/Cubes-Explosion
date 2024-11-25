using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _minCubes = 2;
    [SerializeField] private int _maxCubes = 6;

    private Exploder _exploder;

    private void Awake()
    {
        _exploder = FindAnyObjectByType<Exploder>();
    }

    public void SpawnCubes(Cube sourceCube)
    {
        int childCount = Random.Range(_minCubes, _maxCubes + 1);
        List<Cube> newCubes = new List<Cube>();

        for (int i = 0; i < childCount; i++)
        {
            newCubes.Add(CreateCube(sourceCube));
        }

        _exploder.ExplodeCreatedCubes(newCubes, sourceCube.GetPosition());

        Destroy(sourceCube.gameObject);
    }

    private Cube CreateCube(Cube sourceCube)
    {
        int scaleReduceValue = 2;
        Vector3 newScale = sourceCube.GetScale() / scaleReduceValue;

        Cube newCube = Instantiate(_cubePrefab, sourceCube.GetPosition(), Random.rotation);         

        newCube.ChangeScale(newScale);

        newCube.ChangeColor();

        newCube.GetComponent<Rigidbody>().useGravity = true;

        return newCube;
    }
}
