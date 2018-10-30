using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface IMixerNode : ICollectionNode
    {
        IMixable<IMixerNode> AddMixer(float weight = 1);
        IMixable<ILayerMixerNode> AddLayerMixer(float weight = 1);
        IMixable<ISwitcherNode> AddSwitcher(float weight = 1);

        IMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float weight = 1);
        IMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float weight = 1);
        IMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float weight = 1);
        IMixable<IAnimationIKNode> AddIK(float weight = 1);
    }
    
    public interface IMixable<out T> : IChild<T> where T : INode
    {
        float Weight { get; set; }
    }
}