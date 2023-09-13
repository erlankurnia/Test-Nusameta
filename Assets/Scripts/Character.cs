using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public delegate void OnPlay(int index);

    private int index = 0;
    
    private Animation anim;
    public Animation Anim {
        get {
            if (anim == null) anim = GetComponent<Animation>();
            return anim;
        }
    }

    public OnPlay onPlay;

    public void Init(int _index, bool autoPlay = true) {
        index = _index;
        if (autoPlay) onPlay(index);
    }
}
