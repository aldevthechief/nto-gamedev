using UnityEngine;

public class AStar 
{
    private Vector3Int[] Path = new Vector3Int[0];
    private Cell[] OpenCells = new Cell[0];
    private Cell[] CloseCells = new Cell[0];
    private bool[][] Map = new bool[0][];
    private Vector3Int End;
    private Vector3Int Start;
    private int Steps = 0;

    public Vector3Int[] _Path => Path;
    public Vector3Int _Start => Start;
    public int _Steps => Steps;

    public AStar(bool[][] map, Vector3Int start, Vector3Int end)
    {
        Map = map;
        Solve(start, end, 1000);
    }

    public AStar(bool[][] map, Vector3Int start, Vector3Int end, int safeLimit)
    {
        Map = map;
        Solve(start, end, safeLimit);
    }

    private void Solve(Vector3Int start, Vector3Int end, int safeLimit)
    {
        Vector3Int[] path = new Vector3Int[0];

        Start = start;
        End = end;
        if (Start != start)
        {
            path = StaticTools.ExpandMassive(path, new Vector3Int(Start.x - start.x, Start.y - start.y));
        }

        OpenCells = new Cell[0];
        CloseCells = new Cell[0];
        Open(null, Start);
        if (OpenCells.Length < 1)
        {
            Path = null;
            return;
        }

        Cell current = OpenCells[0];

        int safe = 0;

        while (safe < safeLimit)
        {
            safe++;

            current = LowestF(OpenCells);
            if(current == null)
            {
                Path = null;
                return;
            }
            else if (current._Position == End)
            {
                break;
            }

            CloseCells = StaticTools.ExpandMassive(CloseCells, current);
            OpenCells = StaticTools.RemoveFromMassive(OpenCells, current);

            Expand(current);
        }

        if (safe < safeLimit)
        {
            path = StaticTools.ExpandMassive(path, current.Path(End));

            if (end != End)
            {
                path = StaticTools.ExpandMassive(path, end - End);
            }

            Path = path;
        }

        OpenCells = null;
        CloseCells = null;
    }

    private void Expand(Cell current)
    {
        Open(current, current._Position + new Vector3Int(-1, 1));
        Open(current, current._Position + new Vector3Int(0, 1, 0));
        Open(current, current._Position + new Vector3Int(1, 1, 0));
        Open(current, current._Position + new Vector3Int(1, 0, 0));
        Open(current, current._Position + new Vector3Int(1, -1, 0));
        Open(current, current._Position + new Vector3Int(0, -1, 0));
        Open(current, current._Position + new Vector3Int(-1, -1, 0));
        Open(current, current._Position + new Vector3Int(-1, 0, 0));
    }

    private void Open(Cell parent, Vector3Int position)
    {
        if(position.x < 0 || position.y < 0)
        {
            return;
        }

        foreach (Cell cell in CloseCells)
        {
            if (cell._Position == position)
            {
                return;
            }
        }

        foreach (Cell cell in OpenCells)
        {
            if (cell._Position == position)
            {
                if (cell._G > Distance(parent._Position, position) + parent._G)
                {
                    cell.ChangeParent(parent);
                }

                return;
            }
        }

        if(position.y >= Map.Length)
        {
            return;
        }
        else if(position.x >= Map[position.y].Length)
        {
            return;
        }
        else if (Map[position.y][position.x])
        {
            return;
        }

        Cell newCell = new Cell(this, parent, position);
        OpenCells = StaticTools.ExpandMassive(OpenCells, newCell, 0);
    }

    private Cell LowestF(Cell[] OpenCells)
    {
        if(OpenCells.Length < 1)
        {
            return null;
        }

        Cell lowest = OpenCells[0];

        foreach (Cell cell in OpenCells)
        {
            if (lowest._F > cell._F)
            {
                lowest = cell;
            }
            else if (lowest._F == cell._F)
            {
                if (lowest._H > cell._H)
                {
                    lowest = cell;
                }
            }
        }

        return lowest;
    }

    [System.Serializable]
    private class Cell
    {
        private AStar AStar;
        private Cell Parent;
        private Vector3Int Position;
        private int G; //distance to start
        private int H; //distance to end

        public Vector3Int _Position => Position;
        public int _G => G;
        public int _H => H;
        public int _F => H + G;

        public Cell(AStar astar, Cell parent, Vector3Int position)
        {
            AStar = astar;
            Position = position;

            H = Distance(Position, AStar.End);

            if(position.z != AStar.End.z)
            {
                H /= 4;
            }

            if (parent != null)
            {
                ChangeParent(parent);
            }
            else
            {
                G = 0;
            }
        }

        public void ChangeParent(Cell parent)
        {
            Parent = parent;

            G = Distance(parent.Position, Position) + parent.G;
        }

        public Vector3Int[] Path(Vector3Int childPosition)
        {
            Vector3Int position = childPosition - Position;

            if (Parent != null)
            {
                if ((childPosition - Position) == Vector3Int.zero)
                {
                    return Parent.Path(Position);
                }

                return StaticTools.ExpandMassive(Parent.Path(Position), position);
            }
            else
            {
                return new Vector3Int[1] { position };
            }
        }
    }

    public static int Distance(Vector3 a, Vector3 b) => Mathf.RoundToInt(Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)) * 10);
}
