using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface ISwitcherNode : ICollectionNode
    {
        ISwitchable<IMixerNode> AddMixer();
        ISwitchable<ILayerMixerNode> AddLayerMixer();
        ISwitchable<ISwitcherNode> AddSwitcher();

        ISwitchable<IAnimationClipNode> AddAnimationClip(AnimationClip c);
        ISwitchable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller);
        ISwitchable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks);
        ISwitchable<IAnimationIKNode> AddIK();
    }
    
    public interface ISwitchable<out T> : IChild<T> where T : INode
    {
        float CrossFadeDuration { get; set; }
        void Enable();
    }
}