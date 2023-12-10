using UnityEngine;
using Level1;

namespace Levels
{
    public class Level1 : Level
    {
        [SerializeField] private LevelDialogue StartDialogue;
        [SerializeField] private DialogueSystem DialogueSystem;

        [SerializeField] private PlatformWheel PlatformWheel;

        [SerializeField] private Animator Gates;
        [SerializeField] private CarParking Car;
        [SerializeField] private CarMap CarMap;

        [SerializeField] private GameObject[] SaveTriggers;
        [SerializeField] private int TriggerIndex;

        [SerializeField] private bool ShowStartDialogue = true;

        public override string GetLevelInfo()
        {
            Level1Data data = new Level1Data();

            data.startDialogue = ShowStartDialogue;

            data.platformStage = PlatformWheel._Stage;

            data.gatesOpen = Gates.GetBool("isOpen");
            data.carDriving = Car._Driving;
            data.carBroken = Car._Broken;
            data.carUsed = Car._Used;
            data.carMinigaming = Car._Minigaming;
            data.carX = Car.transform.position.x;
            data.carZ = Car.transform.position.z;

            bool[][] tiles = CarMap._Tiles;
            data.tilesy0 = tiles[0];
            data.tilesy1 = tiles[1];
            data.tilesy2 = tiles[2];
            data.tilesy3 = tiles[3];
            data.tilesy4 = tiles[4];
            data.tilesy5 = tiles[5];
            data.tilesy6 = tiles[6];

            data.triggerIndex = TriggerIndex;

            return JsonUtility.ToJson(data);
        }

        public override void ReadLevelInfo(string info)
        {
            Level1Data data = JsonUtility.FromJson<Level1Data>(info);
            if(data == null)
            {
                data = new Level1Data();
            }

            ShowStartDialogue = data.startDialogue;

            PlatformWheel._Stage = data.platformStage;

            bool[][] tiles = new bool[7][];
            tiles[0] = data.tilesy0;
            tiles[1] = data.tilesy1;
            tiles[2] = data.tilesy2;
            tiles[3] = data.tilesy3;
            tiles[4] = data.tilesy4;
            tiles[5] = data.tilesy5;
            tiles[6] = data.tilesy6;
            CarMap._Tiles = tiles;
            Car.transform.position = new Vector3(data.carX, 1.434f, data.carZ);

            Gates.SetBool("isOpen", data.gatesOpen);
            Car._Driving = data.carDriving;
            Car._Broken = data.carBroken;
            Car._Used = data.carUsed;
            Car._Minigaming = data.carMinigaming;


            SetTriggerIndex(data.triggerIndex, false);
        }

        private void Start()
        {
            if (ShowStartDialogue)
            {
                StartDialogue.StartDialogue();
                DialogueSystem.OnDialogueEnd += DialogueShowed;
            }
        }

        public void DialogueShowed()
        {
            DialogueSystem.OnDialogueEnd -= DialogueShowed;
            ShowStartDialogue = false;

            SaveHandler._Instance.MainSave();
        }

        private void SetTriggerIndex(int value, bool save)
        {
            TriggerIndex = value;

            for (int i = 0; i < SaveTriggers.Length; i++)
            {
                SaveTriggers[i].SetActive(i > TriggerIndex);
            }

            SaveTriggers[3].SetActive(TriggerIndex == 2);

            if (value > -1 && save)
            {
                SaveHandler._Instance.MainSave();
            }
        }

        public void SetTriggerIndex(int value) => SetTriggerIndex(value, true);

        public class Level1Data
        {
            public bool startDialogue = true;

            public int platformStage = 0;

            public bool gatesOpen = false;
            public bool carDriving = false;
            public bool carBroken = false;
            public bool carUsed = false;
            public bool carMinigaming = false;
            public float carX = 427.48f, carZ = -6.67f;

            public bool[] tilesy0 = new bool[7];
            public bool[] tilesy1 = new bool[7];
            public bool[] tilesy2 = new bool[7];
            public bool[] tilesy3 = new bool[7];
            public bool[] tilesy4 = new bool[7];
            public bool[] tilesy5 = new bool[7];
            public bool[] tilesy6 = new bool[7];

            public int triggerIndex = -1;
        }
    }
}