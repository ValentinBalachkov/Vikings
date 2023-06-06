using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _wood, _stowne;
    [SerializeField] private AudioSource _woodAudio, _stoneAudio;
    void Start()
    {
        _wood.Stop();
    }

    public void WoodParticle()
    {
        _wood.Play();
        _woodAudio.Play();
    }
    
    public void StownParticle()
    {
        _stowne.Play();
        _stoneAudio.Play();
    }
}
