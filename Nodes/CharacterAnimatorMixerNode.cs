using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class MixerNodeNode : CollectionNode<MixerNodeNode.MixerChild>, IMixerNode
        {
            public class MixerChildController<T> : ChildController<T, MixerChild>, IMixable<T> where T : NodeBase
            {
                public float Weight
                {
                    get { return Child.weight; }
                    set
                    {
                        Child.weight = value;
                        Child.ApplyWeight();
                    }
                }
                
                public MixerChildController(CollectionNode<MixerChild> parent, T node, MixerChild child) 
                    : base(parent, node, child) { }
            }

            public class MixerChild : CollectionChild
            {
                public MixerChild(MixerNodeNode parent, NodeBase node) : base(parent, node) { }
            }

            public MixerNodeNode(PlayableGraph graph, GameObject owner) : base(graph, owner) { }

            protected override Playable CreateAnimationPlayable(PlayableGraph graph, GameObject owner)
            {
                return AnimationLayerMixerPlayable.Create(graph);
            }

            protected override MixerChild CreateContainer(NodeBase node)
            {
                return new MixerChild(this, node);
            }

            public IMixable<IMixerNode> AddMixer(float weight = 1)
            {
                return AddNode(new MixerNodeNode(playableGraph, ownerObject), weight);
            }

            public IMixable<ILayerMixerNode> AddLayerMixer(float weight = 1)
            {
                return AddNode(new LayerMixerNode(playableGraph, ownerObject), weight);
            }

            public IMixable<ISwitcherNode> AddSwitcher(float weight = 1)
            {
                return AddNode(new SwitcherNode(playableGraph, ownerObject), weight);
            }

            public IMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float weight = 1)
            {
                return AddNode(new AnimationClipNode(playableGraph, c), weight);
            }

            public IMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float weight = 1)
            {
                return AddNode(new AnimationControllerNode(playableGraph, controller), weight);
            }

            public IMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float weight = 1)
            {
                return AddNode(new TimelineNode(playableGraph, ownerObject, tracks), weight);
            }

            public IMixable<IAnimationIKNode> AddIK(float weight = 1)
            {
                return AddNode(new AnimationIKNode(playableGraph), weight);
            }
            
            private IMixable<T> AddNode<T>(T node, float weight) where T : NodeBase
            {
                return new MixerChildController<T>(this, node, Connect(node))
                {
                    Weight = weight
                };
            }
        }
    }
}