using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _wood;
    [SerializeField] private AudioSource _woodAudio;
    void Start()
    {
        _wood.Stop();
    }

    public void WoodParticle()
    {
        _wood.Play();
        _woodAudio.Play();
    }
}
