using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface ILayerMixerNode : ICollectionNode
    {
        ILayerMixable<IMixerNode> AddMixer(float priority = 0);
        ILayerMixable<ILayerMixerNode> AddLayerMixer(float priority = 0);
        ILayerMixable<ISwitcherNode> AddSwitcher(float priority = 0);

        ILayerMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float priority = 0);
        ILayerMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float priority = 0);
        ILayerMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float priority = 0);
        ILayerMixable<IAnimationIKNode> AddIK(float priority = 0);
    }
    
    public interface ILayerMixable<out T> : IChild<T> where T : INode
    {
        float Weight { get; set; }
        float Priority { get; set; }
        bool IsAdditive { get; set; }
        AvatarMask Mask { get; set; }
    }
}