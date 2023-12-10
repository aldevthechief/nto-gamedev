using UnityEngine;

namespace Level1
{
    public class PlatformWheel : MonoBehaviour, IInteractable
    {
        [SerializeField] private Outline Outline;
        [SerializeField] private Animator Animator;
        [SerializeField] private LevelDialogue FirstDialogue;
        [SerializeField] private LevelDialogue SecondDialogue;
        [SerializeField] private LevelDialogue ThirdDialogue;

        [SerializeField] private AudioSource RollSound;
        [SerializeField] private AudioSource BrokeSound;

        [SerializeField] private GameObject RestartTrigger;

        [SerializeField] private int Stage; //0 - опущено 1 - поднято и сломано 2 - опущено терминалом

        public int _Stage
        {
            get
            {
                return Stage;
            }
            set
            {
                Stage = value;

                StopAllCoroutines();
                StartCoroutine(PlaySound());
                if (Stage == 1)
                {
                    Animator.SetBool("Downed", false);
                    Animator.Play("PlatformUpped");
                }
                else
                {
                    Animator.SetBool("Downed", true);
                    Animator.Play("PlatformDowned");

                    if (Stage == 2)
                    {
                        RestartTrigger.SetActive(false);
                    }
                }
            }
        }

        public void SetOutline(bool enabled)
        {
            Outline.enabled = enabled;
        }

        public void Interact()
        {
            if (Stage > 0)
            {
                SecondDialogue.StartDialogue();
                return;
            }

            StopAllCoroutines();
            StartCoroutine(PlaySound());

            BrokeSound.Play();

            Stage = 1;
            Animator.SetBool("Downed", false);
            FirstDialogue.StartDialogue();
        }

        public void Down()
        {
            if (Stage > 1)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(PlaySound());

            Stage = 2;
            Animator.SetBool("Downed", true);
            RestartTrigger.SetActive(false);
        }

        private System.Collections.IEnumerator PlaySound()
        {
            RollSound.Play();
            yield return new WaitForSeconds(8);
            RollSound.Stop();
        }
    }
}
