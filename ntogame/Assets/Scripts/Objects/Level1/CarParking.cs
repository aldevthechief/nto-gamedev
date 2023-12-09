using UnityEngine;
using System.Collections;

namespace Level1
{
    public class CarParking : MonoBehaviour, IInteractable
    {
        [SerializeField] private LevelDialogue Dialogue1;
        [SerializeField] private LevelDialogue Dialogue2;
        [SerializeField] private LevelDialogue Dialogue3;
        [SerializeField] private LevelDialogue Dialogue4;
        [SerializeField] private Outline Outline;
        [SerializeField] private CarMap CarMap;

        [SerializeField] private AudioSource CarSound;
        [SerializeField] private AudioSource AvarySound;

        [SerializeField] private LayerMask Layer;
        [SerializeField] private Transform RayPoint;

        [SerializeField] private GameObject[] Fari;

        [SerializeField] private DialogueSystem DialogueSystem;
        [SerializeField] private GateOpener GateOpener;

        [SerializeField] private Vector3 StartPoint;
        [SerializeField] private Vector3 EndRotation;
        [SerializeField] private float Speed;

        [SerializeField] private bool Driving;
        [SerializeField] private bool Broken;
        [SerializeField] private bool Used;
        [SerializeField] private bool Minigaming;

        public bool _Driving
        {
            get
            {
                return Driving;
            }
            set
            {
                Driving = value;

                if (Driving)
                {
                    transform.position = StartPoint;
                    DriveWay(CarMap._Way);
                }
            }
        }
        public bool _Broken
        {
            get
            {
                return Broken;
            }
            set
            {
                Broken = value;

                if (value)
                {
                    foreach (GameObject fara in Fari)
                    {
                        fara.SetActive(false);
                    }

                    CarSound.Stop();
                }
            }
        }
        public bool _Used
        {
            get
            {
                return Used;
            }
            set
            {
                Used = value;

                if(value && !Broken)
                {
                    foreach (GameObject fara in Fari)
                    {
                        fara.SetActive(false);
                    }

                    CarSound.Stop();
                }
            }
        }
        public bool _Minigaming
        {
            get
            {
                return Minigaming;
            }
            set
            {
                Minigaming = value;
            }
        }

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

            if (Broken)
            {
                Dialogue4.StartDialogue();
            }
            else if (Used)
            {
                Dialogue3.StartDialogue();
            }
            else if (GateOpener._IsOpen)
            {
                if (Minigaming)
                {
                    CarMap.Show();
                }
                else
                {
                    Dialogue2.StartDialogue();

                    GateOpener._Interactable = false;

                    DialogueSystem.OnDialogueEnd += Minigame;
                }
            }
            else
            {
                Dialogue1.StartDialogue();
            }
        }

        public void Minigame()
        {
            Minigaming = true;
            DialogueSystem.OnDialogueEnd -= Minigame;

            CarMap.Show();
        }

        public void DriveWay(Vector3Int[] way)
        {
            for(int i = 0; i < way.Length; i++)
            {
                way[i].x *= -1;
                way[i].z = way[i].y;
                way[i].y = 0;
            }
            StartCoroutine(Drive(way));
        }

        public void SetUsed(bool state)
        {
            if (Used)
            {
                transform.rotation = Quaternion.Euler(EndRotation); 
                
                foreach (GameObject fara in Fari)
                {
                    fara.SetActive(false);
                }
            }

            Used = state;
        }

        private IEnumerator Drive(Vector3Int[] way)
        {
            Used = true;
               Driving = true;

            Vector3 direction = StartPoint - transform.position;
            float magnitude = direction.magnitude;
            while (magnitude > 0.1f)
            {
                transform.position += direction / magnitude * Speed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg - 90, 0);

                direction = StartPoint - transform.position;
                magnitude = direction.magnitude;

                yield return new WaitForEndOfFrame();
            }

            transform.position = StartPoint;

            int currentWay = 0;
            magnitude = 10;
            while(currentWay < way.Length)
            {
                direction = way[currentWay];

                RaycastHit raycast;
                if(Physics.Raycast(RayPoint.position, transform.right, out raycast, Speed * Time.deltaTime, Layer))
                {
                    EZCameraShake.CameraShaker.Instance.ShakeOnce(4 * 1.25f, 4 * 1.25f, 0.1f, 0.5f);
                    Driving = false;
                    Broken = true;

                    transform.position += direction * (raycast.distance + 0.5f);

                    foreach (GameObject fara in Fari)
                    {
                        fara.SetActive(false);
                    }

                    AvarySound.Play();
                    CarSound.Stop();
                    yield break;
                }

                transform.position += direction * Speed * Time.deltaTime;
                magnitude -= Speed * Time.deltaTime;

                if(magnitude <= 0)
                {
                    magnitude = 10;
                    currentWay++;
                }

                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg - 90, 0);

                yield return new WaitForEndOfFrame();
            }

            foreach(GameObject fara in Fari)
            {
                fara.SetActive(false);
            }

            CarSound.Stop();

            Driving = false;
        }
    }
}
