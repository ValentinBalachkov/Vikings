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
        _axe.SetActive(false);
        pick.SetActive(false);
    }

    public void WoodParticle()
    {
        Debug.Log("Wood");
        _axe.SetActive(true);
        pick.SetActive(false);
       
        _wood.Play();
        _woodAudio.Play();
    }
    
    public void StoneParticle()
    {
        
        Debug.Log("Stone");
        _axe.SetActive(false);
        pick.SetActive(true);
        
        _stowne.Play();
        _stoneAudio.Play();
    }
}
