﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class LayerMixerNode : CollectionNode<LayerMixerNode.LayerMixerChild>, ILayerMixerNode
        {
            public class LayerMixerChildController<T> : ChildController<T, LayerMixerChild>, ILayerMixable<T> 
                where T : NodeBase
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
                public LayerMixerNode ParentNode;

                public LayerMixerChild(LayerMixerNode parent, NodeBase node) : base(parent, node)
                {
                    priority = 0;
                    isAdditive = false;
                    avatarMask = emptyMask;
                    ParentNode = parent;
                }
                
                public void ApplyAvatarMask()
                {
                    foreach (var c in connections)
                    {
                        if (c.Key == (int) StreamType.Animation)
                        {
                            var layerMixer = (AnimationLayerMixerPlayable) ParentNode.animationPlayable;
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
                            var layerMixer = (AnimationLayerMixerPlayable) ParentNode.animationPlayable;
                            layerMixer.SetLayerAdditive((uint) c.Value, isAdditive);
                        }
                        else if (c.Key == (int) StreamType.IK)
                        {
                            var ikMixer = (ScriptPlayable<IKAnimationBehaviorLayerMixer>) ParentNode.ikPlayable;
                            ikMixer.GetBehaviour().SetLayerAdditive(c.Value, isAdditive);
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
                    ParentNode.UpdateGraph();
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

            protected override Playable CreateIKPlayable(PlayableGraph graph, GameObject owner)
            {
                return ScriptPlayable<IKAnimationBehaviorLayerMixer>.Create(graph);
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

            public ILayerMixable<IMixerNode> AddMixer(float priority = 0)
            {
                return AddNode(new MixerNode(playableGraph, ownerObject), priority);
            }

            public ILayerMixable<ILayerMixerNode> AddLayerMixer(float priority = 0)
            {
                return AddNode(new LayerMixerNode(playableGraph, ownerObject), priority);
            }

            public ILayerMixable<ISwitcherNode> AddSwitcher(float priority = 0)
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

            public ILayerMixable<IAnimationIKNode> AddIK(float priority = -1)
            {
                var node = AddNode(new AnimationIKNode(playableGraph), priority);
                // TODO: IK mask
                return node;
            }
            
            private ILayerMixable<T> AddNode<T>(T node, float priority) where T : NodeBase
            {
                return new LayerMixerChildController<T>(this, node, CreateChild(node))
                {
                    Priority = priority
                };
            }
        }
    }
}