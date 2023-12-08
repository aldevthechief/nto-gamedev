using UnityEngine;
using System.Collections;

namespace Level1
{
    public class CarParking : MonoBehaviour, IInteractable
    {
        [SerializeField] private LevelDialogue Dialogue1;
        [SerializeField] private LevelDialogue Dialogue2;
        [SerializeField] private LevelDialogue Dialogue3;
        [SerializeField] private Outline Outline;

        [SerializeField] private GameObject[] Fari;

        [SerializeField] private DialogueSystem DialogueSystem;
        [SerializeField] private GateOpener GateOpener;

        [SerializeField] private Vector3[] Points;
        [SerializeField] private Vector3 EndRotation;
        [SerializeField] private float Speed;

        [SerializeField] private bool Driving;
        [SerializeField] private bool Used;

        public void SetOutline(bool enabled) 
        {
            if (Driving)
            {
                Outline.enabled = false;
                return;
            }

            Outline.enabled = enabled;
        }

        public void Interact()
        {
            if (Driving)
            {
                return;
            }

            if (Used)
            {
                Dialogue3.StartDialogue();
            }
            else if (GateOpener._IsOpen)
            {
                Dialogue2.StartDialogue();

                GateOpener._Interactable = false;

                DialogueSystem.OnDialogueEnd += DriveCar;
            }
            else
            {
                Dialogue1.StartDialogue();
            }
        }

        public void DriveCar()
        {
            StartCoroutine(Drive());
            Used = true;

            DialogueSystem.OnDialogueEnd -= DriveCar;
        }

        public void SetUsed(bool state)
        {
            if (Used)
            {
                transform.position = Points[Points.Length - 1];
                transform.rotation = Quaternion.Euler(EndRotation); 
                
                foreach (GameObject fara in Fari)
                {
                    fara.SetActive(false);
                }
            }

            Used = state;
        }

        private IEnumerator Drive()
        {
            Driving = true;

            int currentWay = 0;
            while(currentWay < Points.Length)
            {
                Vector3 direction = Points[currentWay] - transform.position;
                float magnitude = direction.magnitude;
                transform.position += direction / magnitude * Speed * Time.deltaTime;
                if(magnitude < 0.3f)
                {
                    transform.position = Points[currentWay];
                    currentWay++;
                }

                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90, 0);

                yield return new WaitForEndOfFrame();
            }

            foreach(GameObject fara in Fari)
            {
                fara.SetActive(false);
            }

            Driving = false;
        }
    }
}
