using UnityEngine;
using UnityEngine.UI;

namespace Level1
{
    public class CarMap : UIMinigame
    {
        [SerializeField] private GameObject TilePrefab;
        [SerializeField] private GameObject SquarePrefab;
        [SerializeField] private CarParking Car;
        [SerializeField] private RectTransform Content;
        private RectTransform[] Squares = new RectTransform[0];
        private CarmapTile[][] Tiles = new CarmapTile[0][];
        private bool Initialized = false;
        private bool Played = false;

        public bool[][] _Tiles 
        {
            get
            {
                bool[][] map = new bool[7][];
                for(int y = 0; y < Tiles.Length; y++)
                {
                    bool[] map1D = new bool[7];
                    for(int x = 0; x < Tiles[y].Length; x++)
                    {
                        map1D[x] = Tiles[y][x]._Block;
                    }

                    map[y] = map1D;
                }

                return map;
            }
            set
            {
                Initialize();
            }
        }

        private void Start()
        {
            if (!Initialized)
            {
                Initialize();
            }
        }

        public override void Show()
        {
            if (Played)
            {
                return;
            }

            base.Show();
        }

        public void Initialize()
        {
            Initialized = true;

            Tiles = new CarmapTile[7][];
            for(int y = 0; y < 7; y++)
            {
                CarmapTile[] tiles = new CarmapTile[7];
                for(int x = 0; x < 7; x++)
                {
                    CarmapTile tile = Instantiate(TilePrefab, Content).GetComponent<CarmapTile>();
                    tile.SetInfo(this, (x == 1 && y == 2) || (x == 3 && y ==4) || (x == 4 && y == 2));
                    tiles[x] = tile;

                    tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 + x * 100, -50 - y * 100);
                }

                Tiles[y] = tiles;
            }

            BuildWay();
        }

        public void Play()
        {
            Vector3Int[] way = new AStar(_Tiles, new Vector3Int(0, 6), new Vector3Int(0, 0))._Path;
            if(way == null)
            {
                return;
            }

            Played = true;

            Car.DriveWay(way);

            InputHandler.MetaKeyDown -= Hide;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameObject.SetActive(false);
        }

        public void BuildWay()
        {
            Vector3Int[] way = new AStar(_Tiles, new Vector3Int(0, 6), new Vector3Int(0, 0))._Path;

            foreach(RectTransform square in Squares)
            {
                Destroy(square.gameObject);
            }

            Squares = new RectTransform[0];

            if(way == null)
            {
                return;
            }

            Vector3Int position = new Vector3Int(0, 6);
            foreach (Vector3Int direction in way)
            {
                RectTransform square = Instantiate(SquarePrefab, Content).GetComponent<RectTransform>();
                square.GetComponent<Image>().raycastTarget = false;
                square.GetComponent<Image>().color = Color.white / 2;

                square.anchoredPosition = new Vector2(position.x * 100 - 300, 300 - position.y * 100);

                Squares = StaticTools.ExpandMassive(Squares, square);
                position += direction;
            }
            {
                RectTransform square = Instantiate(SquarePrefab, Content).GetComponent<RectTransform>();
                square.GetComponent<Image>().raycastTarget = false;
                square.GetComponent<Image>().color = Color.white / 2;

                square.anchoredPosition = new Vector2(position.x * 100 - 300, 300 - position.y * 100);

                Squares = StaticTools.ExpandMassive(Squares, square);
            }
        }
    }
}
