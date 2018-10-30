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
        private class LayerMixerNode : CollectionNode<LayerMixerNode.LayerMixerChild>, ILayerMixer
        {
            public class LayerMixerChildController<T> : ChildController<T, LayerMixerChild>, ILayerMixable<T> 
                where T : NodeBase
            {
                public float Weight
                {
                    get { return Child.weight; }
                    set { Child.weight = value; }
                }

                public float Priority
                {
                    get { return Child.priority; }
                    set
                    {
                        Child.priority = value;
                        Child.ApplyPriority();
                    }
                }

                public bool IsAdditive
                {
                    get { return Child.isAdditive; }
                    set
                    {
                        Child.isAdditive = value;
                        Child.ApplyIsAdditive();
                    }
                }

                public AvatarMask Mask
                {
                    get { return Child.avatarMask; }
                    set
                    {
                        Child.avatarMask = value;
                        Child.ApplyAvatarMask();
                    }
                }

                public LayerMixerChildController(CollectionNode<LayerMixerChild> parent, T node, LayerMixerChild child) 
                    : base(parent, node, child) { }
            }

            public class LayerMixerChild : CollectionChild, IComparable
            {
                private static readonly AvatarMask emptyMask = new AvatarMask();
                
                public float priority;
                public bool isAdditive;
                public AvatarMask avatarMask;
                public LayerMixerNode parentNode;

                public LayerMixerChild(LayerMixerNode parent, NodeBase node) : base(parent, node)
                {
                    priority = 0;
                    isAdditive = false;
                    avatarMask = emptyMask;
                    parentNode = parent;
                }
                
                public void ApplyAvatarMask()
                {
                    foreach (var c in connections)
                    {
                        if (c.Key == (int) StreamType.Animation)
                        {
                            var layerMixer = (AnimationLayerMixerPlayable) parentNode.animationPlayable;
                            layerMixer.SetLayerMaskFromAvatarMask((uint) c.Value, avatarMask);
                        }
                    }
                }

                public void ApplyIsAdditive()
                {
                    foreach (var c in connections)
                    {
                        if (c.Key == (int) StreamType.Animation)
                        {
                            var layerMixer = (AnimationLayerMixerPlayable) parentNode.animationPlayable;
                            layerMixer.SetLayerAdditive((uint) c.Value, isAdditive);
                        }
                    }
                }

                public override void ApplyAllParams()
                {
                    base.ApplyAllParams();
                    ApplyAvatarMask();
                    ApplyIsAdditive();
                }

                public void ApplyPriority()
                {
                    parentNode.UpdateGraph();
                }
                
                public int CompareTo(object obj)
                {
                    return priority.CompareTo(((LayerMixerChild) obj).priority);
                }
            }

            public LayerMixerNode(PlayableGraph graph, GameObject owner) : base(graph, owner) { }

            protected override Playable CreateAnimationPlayable(PlayableGraph graph, GameObject owner)
            {
                return AnimationLayerMixerPlayable.Create(graph);
            }

            protected override LayerMixerChild CreateContainer(NodeBase node)
            {
                return new LayerMixerChild(this, node);
            }

            protected override void UpdateGraph()
            {
                childNodes.Sort();
                base.UpdateGraph();
            }

            public ILayerMixable<IMixer> AddMixer(float priority = 0)
            {
                throw new NotImplementedException();
            }

            public ILayerMixable<ILayerMixer> AddLayerMixer(float priority = 0)
            {
                return AddNode(new LayerMixerNode(playableGraph, ownerObject), priority);
            }

            public ILayerMixable<ISwitcher> AddSwitcher(float priority = 0)
            {
                return AddNode(new SwitcherNode(playableGraph, ownerObject), priority);
            }

            public ILayerMixable<IAnimationClipNode> AddAnimationClip(AnimationClip c, float priority = 0)
            {
                return AddNode(new AnimationClipNode(playableGraph, c), priority);
            }

            public ILayerMixable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller, float priority = 0)
            {
                return AddNode(new AnimationControllerNode(playableGraph, controller), priority);
            }

            public ILayerMixable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks, float priority = 0)
            {
                return AddNode(new TimelineNode(playableGraph, ownerObject, tracks), priority);
            }

            public ILayerMixable<IAnimationControllerNode> AddIK(float priority = -1)
            {
                throw new NotImplementedException();
            }
            
            private ILayerMixable<T> AddNode<T>(T node, float priority) where T : NodeBase
            {
                return new LayerMixerChildController<T>(this, node, Connect(node))
                {
                    Priority = priority
                };
            }
        }
    }
}