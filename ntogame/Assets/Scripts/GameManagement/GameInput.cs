using UnityEngine;

public class GameInput : MonoBehaviour
{
    /*
     * следит за состоянием блокировки
     */

    private static MonoBehaviour[] Blockators = new MonoBehaviour[0]; // объекты, которые блокируют общее состояние блокировки

    public static bool _Locked => Blockators.Length > 0;

    public static void RegisterBlockator(MonoBehaviour blockator, bool remove) // добавление и удаление блокаторов
    {
        int index = StaticTools.IndexOf(Blockators, blockator);

        if (remove)
        {
            if(index > -1)
            {
                Blockators = StaticTools.ReduceMassive(Blockators, index);
            }
            else
            {
                Debug.LogError($"{blockator} не является блокатором");
            }
        }
        else if(index <= -1)
        {
            Blockators = StaticTools.ExpandMassive(Blockators, blockator);
        }
        else
        {
            Debug.LogError($"{blockator} уже является блокатором");
        }
    }

    public static bool AmIBlocked(MonoBehaviour blockator) => StaticTools.Contains(Blockators, blockator) ? true : _Locked; // возвращает true блокаторам, если они такими являются

    private void OnDestroy() // очистка блокаторов
    {
        Blockators = new MonoBehaviour[0];
    }
}