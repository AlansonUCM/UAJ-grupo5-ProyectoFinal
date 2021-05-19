using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject cube;

    private void Start()
    {
        DebugCommand<float, float> SPAWN_CUBE = new DebugCommand<float, float>("spawn_cube", "Spawns a cube", "spawn_cube <float, float>", (x, y) =>
        {
            Instantiate(cube, new Vector3(x, y, 0), Quaternion.identity);
        });
        DebugConsole.Instance.AddCommand(SPAWN_CUBE);

        DebugInfo<float, float> CUBE_POSITION = new DebugInfo<float, float>("Cube Position", "Shows the cube's position", () =>
        {
            return cube.transform.position.x;
        }, () =>
        {
            return cube.transform.position.y;
        });
        DebugPanel.Instance.AddInfo(CUBE_POSITION);
    }
}
