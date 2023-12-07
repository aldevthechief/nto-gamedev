using UnityEngine;
using System.Collections;

namespace Tutorial
{ 
    public class SynchronizationCheck : MonoBehaviour
    {
        [SerializeField] private InputHandler InputHandler;
        [SerializeField] private Level Level;
        [SerializeField] private DialogueSystem DialogueSystem;
        private int Stage = 0;
        private bool Desynchronize = false;

        private void Start()
        {
            DialogueSystem._Skipable = false;

            InputHandler.MetaKeyDown += PlayerPressKey;
        }

        public void PlayerPressKey()
        {
            switch (Stage)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        Stage++;
                        Next();
                    }
                    else
                    {
                        Desynch();
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        Stage++;
                        Next();
                    }
                    else
                    {
                        Desynch();
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        Stage++;
                        Next();
                    }
                    else
                    {
                        Desynch();
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Stage++;
                        Next();
                    }
                    else
                    {
                        Desynch();
                    }
                    break;
                case 4:
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Stage++;
                        Next();
                        StartCoroutine(EndMiniGame());
                    }
                    else
                    {
                        Desynch();
                    }
                    break;
                case 5:
                    Desynch();
                    break;
            }
        }

        private void Next()
        {
            if (Desynchronize)
            {
                return;
            }

            DialogueSystem._Skipable = true;

            if (DialogueSystem._Writing)
            {
                DialogueSystem.Next();
            }

            DialogueSystem.Next();
            DialogueSystem._Skipable = false;
        }

        public void Desynch()
        {
            if (Desynchronize)
            {
                return;
            }

            Desynchronize = true;
            DialogueSystem._Skipable = true;
            DialogueSystem.StartDialogue(new PhraseInfo[] {new PhraseInfo(null, "...", new string[] {"ошибка синхронизации, запуск повторного виртуализирования"}, new float[] {40})});
            DialogueSystem._Skipable = false;
            StartCoroutine(Restart());
        }

        private IEnumerator EndMiniGame()
        {
            yield return new WaitForSeconds(1.5f);

            InputHandler.MetaKeyDown -= PlayerPressKey;

            DialogueSystem._Skipable = true;
            DialogueSystem.Skip();
            DialogueSystem.Next();

            Destroy(gameObject);
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(1.2f);

            Level.Restart();
        }
    }
}
