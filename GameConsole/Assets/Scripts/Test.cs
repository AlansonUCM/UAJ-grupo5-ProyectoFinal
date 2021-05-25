using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject cube;
    public char c='M';
    public bool ok = true;
    public float n = 2123;
    private void Start()
    {
        DebugCommand<float, float> SPAWN_CUBE = new DebugCommand<float, float>("spawn_cube", "Spawns a cube", "spawn_cube <float, float>", (x, y) =>
        {
            Instantiate(cube, new Vector3(x, y, 0), Quaternion.identity);
        });
        DebugConsole.Instance.AddCommand(SPAWN_CUBE);

    }

    public float getPositionX()
    {
        return cube.transform.position.x;
    }
    public float getPositionY()
    {
        return cube.transform.position.y;
    }
    public void mum(float x)
    {
        cube.transform.position += new Vector3(1, 0, 0);
    }
}
