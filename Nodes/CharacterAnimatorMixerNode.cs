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
        private class MixerNode : CollectionNode<MixerNode.MixerChild>, IMixer
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
                public MixerChild(MixerNode parent, NodeBase node) : base(parent, node) { }
            }

            public MixerNode(PlayableGraph graph, GameObject owner) : base(graph, owner) { }

            protected override Playable CreateAnimationPlayable(PlayableGraph graph, GameObject owner)
            {
                return AnimationLayerMixerPlayable.Create(graph);
            }

            protected override MixerChild CreateContainer(NodeBase node)
            {
                return new MixerChild(this, node);
            }

            public IMixable<IMixer> AddMixer(float priority = 0)
            {
                return AddNode(new MixerNode(playableGraph, ownerObject), priority);
            }

            public IMixable<ILayerMixer> AddLayerMixer(float priority = 0)
            {
                return AddNode(new LayerMixerNode(playableGraph, ownerObject), priority);
            }

            public IMixable<ISwitcher> AddSwitcher(float priority = 0)
            {
                return AddNode(new SwitcherNode(playableGraph, ownerObject), priority);
            }

            public IMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float priority = 0)
            {
                return AddNode(new AnimationClipNode(playableGraph, c), priority);
            }

            public IMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float priority = 0)
            {
                return AddNode(new AnimationControllerNode(playableGraph, controller), priority);
            }

            public IMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float priority = 0)
            {
                return AddNode(new TimelineNode(playableGraph, ownerObject, tracks), priority);
            }

            public IMixable<IAnimationControllerNode> AddIK(float priority = -1)
            {
                throw new NotImplementedException();
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