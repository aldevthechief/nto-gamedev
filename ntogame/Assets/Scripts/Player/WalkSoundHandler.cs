using UnityEngine;

public class WalkSoundHandler : MonoBehaviour
{
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private AudioSource Source;

    [SerializeField] private AudioClip[] BetonWalkSound;
    [SerializeField] private AudioClip[] DirtWalkSound;
    [SerializeField] private AudioClip[] WaterWalkSound;
    [SerializeField] private AudioClip[] MetalWalkSound;
    [SerializeField] private AudioClip[] WoodWalkSound;

    [SerializeField] private AudioClip BetonLandSound;
    [SerializeField] private AudioClip DirtLandSound;
    [SerializeField] private AudioClip WaterLandSound;
    [SerializeField] private AudioClip MetalLandSound;
    [SerializeField] private AudioClip WoodLandSound;

    [SerializeField] private float Volume;
    [SerializeField] private float Pitch;
    [SerializeField] private float Razbros;

    private Platform.MaterialType ReturnMaterialType()
    {
        RaycastHit raycast;
        if (Physics.Raycast(transform.position, Vector3.down, out raycast, 5, LayerMask))
        {
            Platform platform = raycast.transform.GetComponent<Platform>();
            if (platform != null)
            {
                return platform._Material;
            }
        }

        return Platform.MaterialType.None;
    }

    public void PlayWalkSound()
    {
        switch (ReturnMaterialType())
        {
            case Platform.MaterialType.None:
                return;
            case Platform.MaterialType.Beton:
                Source.clip = BetonWalkSound[Random.Range(0, BetonWalkSound.Length)];
                break;
            case Platform.MaterialType.Dirt:
                Source.clip = DirtWalkSound[Random.Range(0, DirtWalkSound.Length)];
                break;
            case Platform.MaterialType.Water:
                Source.clip = WaterWalkSound[Random.Range(0, WaterWalkSound.Length)];
                break;
            case Platform.MaterialType.Metal:
                Source.clip = MetalWalkSound[Random.Range(0, MetalWalkSound.Length)];
                break;
            case Platform.MaterialType.Wood:
                Source.clip = WoodWalkSound[Random.Range(0, WoodWalkSound.Length)];
                break;
        }

        Source.volume = Volume;
        Source.pitch = Random.Range(Pitch - Razbros, Pitch + Razbros);

        Source.Play();
    }

    public void PlayLandSound()
    {
        switch (ReturnMaterialType())
        {
            case Platform.MaterialType.None:
                return;
            case Platform.MaterialType.Beton:
                Source.clip = BetonLandSound;
                break;
            case Platform.MaterialType.Dirt:
                Source.clip = DirtLandSound;
                break;
            case Platform.MaterialType.Water:
                Source.clip = WaterLandSound;
                break;
            case Platform.MaterialType.Metal:
                Source.clip = MetalLandSound;
                break;
            case Platform.MaterialType.Wood:
                Source.clip = WoodLandSound;
                break;
        }

        Source.volume = Volume;
        Source.pitch = Random.Range(Pitch - Razbros, Pitch + Razbros);

        Source.Play();
    }
}