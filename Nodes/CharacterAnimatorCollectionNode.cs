using System;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private abstract class CollectionNode<T> : NodeBase
            where T : CollectionNode<T>.CollectionChild
        {
            private static readonly StreamType[] mixerOutputTypes =
            {
                StreamType.Animation,
                StreamType.IK
            };

            public abstract class ChildController<N, C> : IChild<N>
                where N : NodeBase
                where C : CollectionChild
            {
                private CollectionNode<T> parent;

                public N Node { get; }
                public C Child { get; }

                public ChildController(CollectionNode<T> parent, N node, C child)
                {
                    this.parent = parent;
                    Node = node;
                    Child = child;
                }
                
                public void Remove()
                {
                    parent.DestroyChild(Node);
                }
            }

            public abstract class CollectionChild
            {
                public float weight;
                
                public NodeBase node;
                public CollectionNode<T> parent;
                public List<KeyValuePair<int, int>> connections;

                public CollectionChild(CollectionNode<T> parent, NodeBase node)
                {
                    this.node = node;
                    this.parent = parent;
                    
                    connections = new List<KeyValuePair<int, int>>();
                    weight = 1f;
                }

                public virtual void ApplyAllParams()
                {
                    ApplyWeight();
                }

                public virtual void ApplyWeight()
                {
                    SetNodeWeight(weight);
                }

                public void SetNodeWeight(float nodeWeight)
                {
                    for (var i = 0; i < connections.Count; i++)
                    {
                        var c = connections[i];
                        parent.playables[c.Key].SetInputWeight(c.Value, nodeWeight);
                    }
                }
            }

            protected List<T> childNodes;
            protected GameObject ownerObject;
            protected PlayableGraph playableGraph;
            protected Playable passtroughPlayable;
            protected Playable animationPlayable;
            protected Playable ikPlayable;
            protected Playable[] playables;
            
            public CollectionNode(PlayableGraph graph, GameObject owner)
            {
                playableGraph = graph;
                ownerObject = owner;
                childNodes = new List<T>();

                passtroughPlayable = Playable.Create(graph, mixerOutputTypes.Length);
                passtroughPlayable.SetOutputCount(mixerOutputTypes.Length);
                passtroughPlayable.SetTraversalMode(PlayableTraversalMode.Passthrough);

                animationPlayable = CreateAnimationPlayable(graph, owner);
                passtroughPlayable.ConnectInput(0, animationPlayable, 0, 1f);

                ikPlayable = CreateIKPlayable(graph, owner);
                passtroughPlayable.ConnectInput(1, ikPlayable, 0, 1f);
                
                playables = new[]
                {
                    animationPlayable,
                    ikPlayable
                };
            }

            protected virtual Playable CreateAnimationPlayable(PlayableGraph graph, GameObject owner)
            {
                return AnimationMixerPlayable.Create(graph);
            }
            
            protected virtual Playable CreateIKPlayable(PlayableGraph graph, GameObject owner)
            {
                return ScriptPlayable<IKAnimationBehaviorMixer>.Create(graph);
            }

            public override Playable Playable => passtroughPlayable;
            public override StreamType[] OutputTypes => mixerOutputTypes;

            public T CreateChild(NodeBase node)
            {
                var index = GetNodeIndex(node);

                if (index == -1)
                {
                    var container = CreateContainer(node);
                    childNodes.Add(container);
                    UpdateGraph();
                    return container;
                }

                return childNodes[index];
            }

            protected abstract T CreateContainer(NodeBase node);

            public void DestroyChild(NodeBase node)
            {
                var index = GetNodeIndex(node);

                if (index >= 0)
                {
                    var child = childNodes[index];
                    childNodes.RemoveAt(index);
                    playableGraph.DestroySubgraph(child.node.Playable);
                    UpdateGraph();
                }
            }

            protected virtual void UpdateGraph()
            {
                var animationInput = 0;
                var ikInput = 0;

                DisconnectAllInputs(animationPlayable);
                DisconnectAllInputs(ikPlayable);

                foreach (var child in childNodes)
                {
                    child.connections.Clear();
                    
                    var playable = child.node.Playable;
                    var outputCount = child.node.OutputTypes.Length;
                    var outputTypes = child.node.OutputTypes;

                    for (var o = 0; o < outputCount; o++)
                    {
                        switch (outputTypes[o])
                        {
                            case StreamType.Animation:
                                PrepareInputForConnection(animationPlayable, animationInput);
                                animationPlayable.ConnectInput(animationInput, playable, o);
                                child.connections.Add(new KeyValuePair<int, int>((int) StreamType.Animation, animationInput));
                                animationInput++;
                                break;
                            
                            case StreamType.IK:
                                PrepareInputForConnection(ikPlayable, ikInput);
                                ikPlayable.ConnectInput(ikInput, playable, o);
                                child.connections.Add(new KeyValuePair<int, int>((int) StreamType.IK, ikInput));
                                ikInput++;
                                break;
                            
                            default:
                                Debug.LogError("Unknown output type encountered: " + outputTypes[o]);
                                break;
                        }
                    }

                    child.ApplyAllParams();
                }

                RemoveUnusedInputs(animationPlayable, animationInput);
                RemoveUnusedInputs(ikPlayable, ikInput);
            }

            private void PrepareInputForConnection(Playable playable, int index)
            {
                if (playable.GetInputCount() <= index)
                {
                    playable.SetInputCount(playable.GetInputCount() + 1);
                }
            }

            private void DisconnectAllInputs(Playable p)
            {
                for (var i = p.GetInputCount() - 1; i >= 0; i--)
                {
                    p.DisconnectInput(i);
                }
            }

            private void RemoveUnusedInputs(Playable p, int usedInputs)
            {
                if (p.GetInputCount() <= usedInputs)
                {
                    return;
                }

                for (var i = usedInputs; i < p.GetInputCount(); i++)
                {
                    p.DisconnectInput(i);
                }

                p.SetInputCount(usedInputs);
            }

            private int GetNodeIndex(NodeBase node)
            {
                for (var i = 0; i < childNodes.Count; i++)
                {
                    if (childNodes[i].node == node)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }
    }
}