using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class ApplyName : UIMinigame
    {
        [SerializeField] private InputField InputField;
        [SerializeField] private SynchronizationCheck SynchronizationCheck;

        public override void Hide()
        {
           
        }

        public void Apply()
        {
            if(InputField.text.Length < 1 || InputField.text.Length > 16)
            {
                return;
            }

            SaveHandler._Instance._PlayerName = InputField.text;

            InputHandler.MetaKeyDown -= Hide;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gameObject.SetActive(false);

            SynchronizationCheck.StartWork();
        }
    }
}
