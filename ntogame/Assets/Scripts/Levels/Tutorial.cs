using UnityEngine;
using Tutorial;

namespace Levels
{
    public class Tutorial : Level
    {
        [SerializeField] private SynchronizationCheck SynchronizationCheck;
        [SerializeField] private LevelDialogue[] Dialogues;
        [SerializeField] private WirePillar[] Pillars;
        [SerializeField] private Transform[] PillarPoints;
        [SerializeField] private WireBlock WireBlock;
        [SerializeField] private TutorialLevelInfo LevelInfo;

        public void Synchronized()
        {
            LevelInfo.synchronized = true;
            LevelInfo.dialogues[0] = true;

            SaveMain();
        }

        public void WasDialogue(int index)
        {
            LevelInfo.dialogues[index] = true;

            SaveMain();
        }

        public override string GetLevelInfo()
        {
            int[] connections = new int[4];
            for(int i = 0; i < connections.Length; i++)
            {
                connections[i] = StaticTools.IndexOf(Pillars, Pillars[i]._Connection[1]);
            }

            LevelInfo.connections = connections;

            return JsonUtility.ToJson(LevelInfo);
        }

        public override void ReadLevelInfo(string info)
        {
            LevelInfo = JsonUtility.FromJson<TutorialLevelInfo>(info);
            if(LevelInfo == null)
            {
                LevelInfo = new TutorialLevelInfo();
            }

            if(LevelInfo.synchronized)
            {
                SynchronizationCheck.Skip();
            }

            for(int i = 1; i < Dialogues.Length; i++)
            {
                Dialogues[i]._Startable = !LevelInfo.dialogues[i];
            }

            for(int i = 0; i < LevelInfo.connections.Length; i++)
            {
                if(LevelInfo.connections[i] > -1)
                {
                    WireBlock.StartWiring(PillarPoints[LevelInfo.connections[i]], PillarPoints[i]);
                    WireBlock.UpdateLines();
                    WireBlock.StopWiring(PillarPoints[LevelInfo.connections[i]]);
                }
            }
        }

        [System.Serializable]
        private class TutorialLevelInfo
        {
            public bool synchronized = false;
            public bool[] dialogues = new bool[5] {false, false, false, false, false};
            public int[] connections = new int[4] { -1, -1, -1, -1 };
        }
    }
}