using UnityEngine;

public static class MeshUtils
{
    /// <summary>
    /// Generates a simple quad of any size
    /// </summary>
    /// <param name="size">The size of the quad</param>
    /// <param name="pivot">Where the mesh pivots</param>
    /// <returns>The quad mesh</returns>
    public static Mesh GenerateQuad(float size, Vector2 pivot)
    {
        Vector3[] _vertices =
        {
            new Vector3(size - pivot.x, 0, size - pivot.y),
            new Vector3(size - pivot.x, 0, 0 - pivot.y),
            new Vector3(0 - pivot.x, 0, 0 - pivot.y),
            new Vector3(0 - pivot.x, 0, size - pivot.y)
        };

        Vector2[] _uv =
        {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(0, 1)
        };

        int[] triangles =
        {
            0, 1, 2,
            2, 3, 0
        };

        return new Mesh
        {
            vertices = _vertices,
            uv = _uv,
            triangles = triangles
        };
    }
}