using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject model;
    float timer;
    AnimationClip currentClip;
    public int currentClipIndex = 0;
    int lastClipIndex = 0;
    float currentClipTimePerFrame;

    public List<AnimationClip> animationClips;
    public List<BoxColliderSerializables> boxCollidersKeyframes;
    BoxColliderSerializables currentColliders;
    int currentDataIndex = 0;
    public bool looping = true;
    [Range(.1f, 2f)]
    public float animationSpeed = 1f;

    private void Awake()
    {
        timer = 0;
        currentClip = animationClips.Count > 0 ? animationClips[0] : null;
        currentColliders = boxCollidersKeyframes.Count > 0 ? boxCollidersKeyframes[0] : null;
    }
    private void Update()
    {
        if (lastClipIndex != currentClipIndex)
        {
            currentClip = animationClips[currentClipIndex];
            currentColliders = boxCollidersKeyframes[currentClipIndex];
            currentDataIndex = 0;
        }
        if(currentClip != null)
        {
            timer += Time.deltaTime * animationSpeed;
            if (looping && timer >= currentClip.length)
            {
                timer = 0;
            }
            currentClip.SampleAnimation(model, timer);
            SampleAnimationData(timer);
        }
        lastClipIndex = currentClipIndex;
    }

    public AnimationClip GetClip(string name)
    {
        foreach(AnimationClip clip in animationClips)
        {
            if(clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }

    private void SampleAnimationData(float timer)
    {
        int nextDataIndex = (currentDataIndex + 1) % boxCollidersKeyframes.Count;
        while(timer > currentColliders[nextDataIndex].sampleTime)
        {
            currentDataIndex++;
            currentDataIndex %= boxCollidersKeyframes.Count;
            nextDataIndex = currentDataIndex + 1 % boxCollidersKeyframes.Count;
        }

        BoxColliderKeyframe currentKeyframeData = currentColliders[currentDataIndex], nextKeyframeData = currentColliders[nextDataIndex];
        float currentTime = currentKeyframeData.sampleTime, nextTime = nextKeyframeData.sampleTime;
        float interpolateParam = (timer - currentTime) / (nextTime - currentTime);

        BoxColliderSerializable currentData, nextData;
        BoxColliderData interpolateColliderData;
        BoxCollider boxCollider;
        for (int i = 0; i < currentKeyframeData.Count; ++i)
        {
            currentData = currentKeyframeData[i];
            nextData = nextKeyframeData[i];
            interpolateColliderData = currentData.Interpolate(nextData, interpolateParam);
            boxCollider = GameObject.Find(currentData.gameObjectName).GetComponent<BoxCollider>();
            boxCollider.isTrigger = interpolateColliderData.isTrigger;
            boxCollider.center = interpolateColliderData.center;
            boxCollider.size = interpolateColliderData.size;
        }
    }
}
