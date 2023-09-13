using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private int index = 0;
    private string[] clipsName;

    private Animation anim;
    public Animation Anim {
        get {
            if (anim == null) anim = GetComponent<Animation>();
            return anim;
        }
    }

    public void Init(int _index, string[] _clipsName = null) {
        index = _index;
        if (_clipsName != null) clipsName = _clipsName;
        if (Anim.playAutomatically) Play();
    }

    public void SetClips(string[] _clipsName) {
        clipsName = _clipsName;
    }

    public void Play() {
        string animName = clipsName[0];
        if (index < clipsName.Length) animName = clipsName[index];
        Anim.Play(animName);
    }
}
