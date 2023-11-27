using UnityEngine;

public delegate void SimpleVoid();

public class InputHandler : MonoBehaviour
{
    /*
     * следите за нажатиями игрока с помощью этого обработчика
     */

    public event SimpleVoid MetaKeyDown = null; // высшее событие нажатия кнопки, который блокирует нижние две. После сделанного дела ВСЕГДА ОТПИСЫВАЙТЕСЬ от ивента, иначе вся игра заблочится

    public event SimpleVoid OnKeyDown = null; // нажатие
    public event SimpleVoid OnKeyHold = null; // зажатие

    public bool _InputAllowed => MetaKeyDown == null; // можно использовать если в классе имеется Update и от него нельзя избавится

    private void Update()
    {
        if(MetaKeyDown != null)
        {
            if (Input.anyKeyDown)
            {
                MetaKeyDown.Invoke();
            }
            return;
        }

        if (Input.anyKeyDown)
        {
            if(OnKeyDown != null)
            {
                OnKeyDown.Invoke();
            }
        }

        if (Input.anyKey)
        {
            if (OnKeyHold != null)
            {
                OnKeyHold.Invoke();
            }
        }
    }
}
