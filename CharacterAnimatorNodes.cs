using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface INode
    {
        double Speed { get; set; }
    }

    public interface IAnimationClipNode : INode
    {
        AnimationClip Clip { get; }
        bool FootIk { get; set; }
        bool PlayableIk { get; set; }
    }

    public interface IAnimationControllerNode : INode
    {
        RuntimeAnimatorController Controller { get; }

        ControllerValue<bool> GetBoolParam(string name);
        ControllerValue<int> GetIntParam(string name);
        ControllerValue<float> GetFloatParam(string name);
        ControllerValue<bool> GetTriggerValue(string name);

        // TODO: shitton of animator methods
    }

    public interface IAnimationIKNode : INode
    {
        // TODO: IK methods
    }

    public interface ITimelineNode : INode
    {
        // TODO: timeline methods
    }
    
    public interface ILayerMixer : INode
    {
        ILayerMixable<IMixer> AddMixer(float priority = 0);
        ILayerMixable<ILayerMixer> AddLayerMixer(float priority = 0);
        ILayerMixable<ISwitcher> AddSwitcher(float priority = 0);

        ILayerMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float priority = 0);
        ILayerMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float priority = 0);
        ILayerMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float priority = 0);
        ILayerMixable<IAnimationControllerNode> AddIK(float priority = 0);
    }

    public interface IMixer : INode
    {
        IMixable<IMixer> AddMixer(float weight = 1);
        IMixable<ILayerMixer> AddLayerMixer(float weight = 1);
        IMixable<ISwitcher> AddSwitcher(float weight = 1);

        IMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float weight = 1);
        IMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float weight = 1);
        IMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float weight = 1);
        IMixable<IAnimationControllerNode> AddIK(float weight = 1);
    }

    public interface ISwitcher : INode
    {
        ISwitchable<IMixer> AddMixer();
        ISwitchable<ILayerMixer> AddLayerMixer();
        ISwitchable<ISwitcher> AddSwitcher();

        ISwitchable<IAnimationClipNode> AddAnimationClip(AnimationClip c);
        ISwitchable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller);
        ISwitchable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks);
        ISwitchable<IAnimationControllerNode> AddIK();
    }
    
    public interface IChild<out T> where T : INode
    {
        T Node { get; }
        void Remove();
    }

    public interface ILayerMixable<out T> : IChild<T> where T : INode
    {
        float Weight { get; set; }
        float Priority { get; set; }
        bool IsAdditive { get; set; }
        AvatarMask Mask { get; set; }
    }

    public interface IMixable<out T> : IChild<T> where T : INode
    {
        float Weight { get; set; }
    }

    public interface ISwitchable<out T> : IChild<T> where T : INode
    {
        float CrossFadeDuration { get; set; }
        void Enable();
    }
}