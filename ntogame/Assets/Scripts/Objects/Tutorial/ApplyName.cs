using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class ApplyName : UIMinigame
    {
        [SerializeField] private InputField InputField;
        [SerializeField] private SynchronizationCheck SynchronizationCheck;
        [SerializeField] private Levels.Tutorial Tutorial;

        public override void Hide()
        {
           
        }

        public void Apply()
        {
            if(InputField.text.Length < 1 || InputField.text.Length > 16)
            {
                return;
            }

            Tutorial.NameMade();

            SaveHandler._Instance._PlayerName = InputField.text;

            InputHandler.MetaKeyDown -= Hide;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameObject.SetActive(false);

            SynchronizationCheck.StartWork();
        }

        private void Update()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
