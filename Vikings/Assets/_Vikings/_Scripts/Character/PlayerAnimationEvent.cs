using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _wood, _stowne;
    [SerializeField] private AudioSource _woodAudio, _stoneAudio;
    [SerializeField] private GameObject _axe, pick;

    private void Start()
    {
        _wood.Stop();
        _stowne.Stop();
    }

    public void DisableEffects()
    {
        
        _wood.Stop();
        _stowne.Stop();
        _stoneAudio.Stop();
        _woodAudio.Stop();
        _wood.gameObject.SetActive(false);
        _stowne.gameObject.SetActive(false);
        _axe.SetActive(false);
        pick.SetActive(false);
    }

    public void WoodParticle()
    {
        _axe.SetActive(true);
        pick.SetActive(false);
       
        _wood.gameObject.SetActive(true);
        _wood.Play();
        _woodAudio.Play();
    }
    
    public void StoneParticle()
    {
        _axe.SetActive(false);
        pick.SetActive(true);
        
        _stowne.gameObject.SetActive(true);
        _stowne.Play();
        _stoneAudio.Play();
    }
}
