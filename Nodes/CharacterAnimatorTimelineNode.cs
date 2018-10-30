using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class TimelineNode : NodeBase, ITimelineNode
        {
            private StreamType[] outputTypes;
            private ScriptPlayable<TimelinePlayable> timelinePlayable;

            public override Playable Playable
            {
                get { return timelinePlayable; }
            }

            public override StreamType[] OutputTypes
            {
                get { return outputTypes; }
            }

            public TimelineNode(PlayableGraph graph, GameObject owner, IEnumerable<TrackAsset> tracks)
            {
                var trackAssets = tracks as TrackAsset[] ?? tracks.ToArray();
                timelinePlayable = TimelinePlayable.Create(graph, trackAssets, owner, true, false);
                timelinePlayable.SetOutputCount(trackAssets.Length);
                outputTypes = DetermineOutputTypes(trackAssets);
            }

            private StreamType[] DetermineOutputTypes(TrackAsset[] trackAssets)
            {
                var types = new StreamType[trackAssets.Length];

                for (var i = 0; i < trackAssets.Length; i++)
                {
                    var input = trackAssets[i];
                    var type = StreamType.Unknown;

                    if (input is AnimationTrack)
                    {
                        type = StreamType.Animation;
                    }

                    types[i] = type;
                }
                
                return types;
            }
        }
    }
}