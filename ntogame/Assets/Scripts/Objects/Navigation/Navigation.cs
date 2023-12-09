using UnityEngine;

[System.Serializable]
public class Navigation
{
    public enum MoveStyle {Straight, Careful}
    private Vector3Int[] Path = new Vector3Int[0];

    public Vector3Int[] _Path => Path;

    public Navigation(bool[][] map, Vector3Int start, Vector3Int end)
    {
        Path = ToNicePath(new AStar(map, start, end)._Path);
    }

    private Vector3Int[] ToNicePath(Vector3Int[] path)
    {
        if (path.Length < 4)
        {
            return path;
        }

        Vector3Int[] newPath = new Vector3Int[path.Length];

        byte iStart = 0;
        byte iEnd = 0;

        newPath = StaticTools.ReduceMassive(newPath, 0);
        newPath[0] = path[0] + path[1];

        iStart = 1;

        newPath = StaticTools.ReduceMassive(newPath, 1);
        newPath[newPath.Length - 1] = path[path.Length - 1] + path[path.Length - 2];

        iEnd = 1;

        for (int i = iStart; i < newPath.Length - iEnd; i++)
        {
            newPath[i] = path[i + iStart];
        }

        return newPath;
    }
}