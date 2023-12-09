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

        [SerializeField] private bool Broken;
        [SerializeField] private bool Downed;

        public bool _Broken
        {
            get
            {
                return Broken;
            }
            set
            {
                StopAllCoroutines();
                StartCoroutine(PlaySound());

                Broken = value;
                Animator.SetBool("Downed", !value);
            }
        }
        public bool _Downed 
        {
            get
            {
                return Downed;
            }
            set
            {
                StopAllCoroutines();
                StartCoroutine(PlaySound());

                Downed = value;
                Animator.SetBool("Downed", value);
                RestartTrigger.SetActive(!value);
            }
        }

        public void SetOutline(bool enabled)
        {
            Outline.enabled = enabled;
        }

        public void Interact()
        {
            if (Broken)
            {
                SecondDialogue.StartDialogue();
                return;
            }

            StopAllCoroutines();
            StartCoroutine(PlaySound());

            BrokeSound.Play();

            Broken = true;
            Animator.SetBool("Downed", false);
            FirstDialogue.StartDialogue();
        }

        public void Down()
        {
            if (Downed)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(PlaySound());

            Downed = true;
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
