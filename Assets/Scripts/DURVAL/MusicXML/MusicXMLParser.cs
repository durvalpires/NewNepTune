using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine;

public static class MusicXMLParser
{
    /// <summary>
    /// MusicXMLのテキストから、音符オブジェクトに変換します。
    /// </summary>
    /// <returns>The score partwise.</returns>
    /// <param name="text">Text.</param>
    public static Score GetScorePartwise(string text)
    {
        var doc = new XmlDocument();
        doc.LoadXml(text);

        var score = new Score();
        var partIdList = GetScorePartIdList(doc);
        score.ScoreParts = partIdList.Select(partId =>
        {
            return new ScorePart()
            {
                MeasureList = GetMeasures(partId, doc).ToArray(),
            };
        }).ToList();
        score.Tempo = GetScoreTempo(doc);

        return score;
    }

    private static int GetScoreTempo(XmlDocument doc)
    {
        var directionElements = doc.SelectNodes("score-partwise/part/measure/direction");
        if (directionElements == null || directionElements.Count == 0)
        {
            Debug.LogWarning("No direction elements found, defaulting to 120 BPM");
            return 120;
        }

        foreach (XmlNode directionNode in directionElements)
        {
            var soundNode = directionNode.SelectSingleNode("sound");
            if (soundNode != null && soundNode.Attributes["tempo"] != null)
            {
                var tempo = soundNode.Attributes["tempo"].InnerText;
                return int.Parse(tempo);
            }
            else{
                XmlNode metronomeNode = directionNode.SelectSingleNode("direction-type/metronome");

                if (metronomeNode != null)
                {
                    string beatUnit = metronomeNode.SelectSingleNode("beat-unit")?.InnerText;
                    string perMinute = metronomeNode.SelectSingleNode("per-minute")?.InnerText;

                    if (!string.IsNullOrEmpty(perMinute))
                    {
                        int bpm = int.Parse(perMinute);
                        Debug.Log($"BPM: {bpm}, Beat Unit: {beatUnit}");
                        return bpm; // Exit after finding the BPM
                    }
                }
            }
        }

        // If no tempo is found in any direction
        Debug.LogWarning("No tempo found in direction elements, defaulting to 120 BPM");
        return 120;
    }

    private static IEnumerable<Measure> GetMeasures(string partId, XmlDocument doc)
    {
        var measureNodes = doc.SelectNodes($"score-partwise/part[@id='{partId}']/measure");

        foreach (XmlNode measureNode in measureNodes)
        {
            var measure = new Measure();
            measure.Number = Int64.Parse(measureNode.Attributes["number"].InnerText);
            measure.Attribute = GetAttribute(measureNode.SelectSingleNode("attributes"));
            measure.Children = GetMeasureChildren(measureNode).ToArray();
            yield return measure;
        }
    }

    private static IEnumerable<IMeasureChild> GetMeasureChildren(XmlNode measureNode)
    {
        foreach (XmlNode measureChildNode in measureNode)
        {
            if (measureChildNode.Name == "note")
            {
                yield return GetScoreNote(measureChildNode);
            }
            else if (measureChildNode.Name == "backup")
            {
                yield return GetBackup(measureChildNode);
            }
        }
    }

    private static IEnumerable<ScoreNote> GetNotes(XmlNode measureNode)
    {
        var noteNodes = measureNode.SelectNodes("note");

        foreach (XmlNode noteNode in noteNodes)
        {
            yield return GetScoreNote(noteNode);
        }
    }

    private static ScoreNote GetScoreNote(XmlNode noteNode)
    {
        var scoreNote = new ScoreNote()
        {
            Pitch = GetPitch(noteNode.SelectSingleNode("pitch")),
            Duration = noteNode.SelectSingleNode("duration") != null ? 
                    Int32.Parse(noteNode.SelectSingleNode("duration").InnerText) : 0,
            Type = noteNode.SelectSingleNode("type") != null ? 
                noteNode.SelectSingleNode("type").InnerText : "unknown",
            Stem = noteNode.SelectSingleNode("stem") != null ? 
                noteNode.SelectSingleNode("stem").InnerText : "unknown",
            Voice = noteNode.SelectSingleNode("voice") != null ? 
                    Int32.Parse(noteNode.SelectSingleNode("voice").InnerText) : 1,
            Staff = noteNode.SelectSingleNode("staff") != null ? 
                    Int32.Parse(noteNode.SelectSingleNode("staff").InnerText) : 1,
            IsRest = noteNode.SelectSingleNode("rest") != null,
            IsChord = noteNode.SelectSingleNode("chord") != null,
        };

        var tieNodes = noteNode.SelectNodes("tie");
        if (tieNodes != null && tieNodes.Count > 0)
        {
            scoreNote.TieList = new List<Tie>();
            foreach (XmlNode tieNode in tieNodes)
            {
                var tie = GetTie(tieNode);
                if (tie != null)
                {
                    scoreNote.TieList.Add(tie.Value);
                }
            }
        }

        var beamNodes = noteNode.SelectNodes("beam");
        if (beamNodes != null && beamNodes.Count > 0)
        {
            scoreNote.BeamList = new List<Beam>();
            foreach (XmlNode beamNode in beamNodes)
            {
                var beam = GetBeam(beamNode);
                if (beam != null)
                {
                    scoreNote.BeamList.Add(beam.Value);
                }
            }
        }

        return scoreNote;
    }

    private static Pitch? GetPitch(XmlNode pitchNode)
    {
        if (pitchNode == null)
        {
            return null;
        }

        return new Pitch()
        {
            Step = pitchNode.SelectSingleNode("step")?.InnerText ?? "C",
            Octave = pitchNode.SelectSingleNode("octave") != null ? 
                    Int32.Parse(pitchNode.SelectSingleNode("octave").InnerText) : 4,
            Alter = pitchNode.SelectSingleNode("alter") != null ? 
                    (int?)Int32.Parse(pitchNode.SelectSingleNode("alter").InnerText) : null,
        };
    }

    private static Tie? GetTie(XmlNode tieNode)
    {
        if (tieNode == null)
        {
            return null;
        }

        return new Tie()
        {
            Type = tieNode.Attributes["type"].InnerText,
        };
    }

    private static Beam? GetBeam(XmlNode beamNode)
    {
        if (beamNode == null)
        {
            return null;
        }

        return new Beam()
        {
            Type = beamNode.InnerText,
            Number = Int32.Parse(beamNode.Attributes["number"].InnerText),
        };
    }

    private static Backup GetBackup(XmlNode noteNode)
    {
        return new Backup()
        {
            Duration = Int32.Parse(noteNode.SelectSingleNode("duration").InnerText),
        };
    }

    private static MeasureAttribute? GetAttribute(XmlNode attributesNode)
    {
        if (attributesNode == null)
        {
            return null;
        }
        return new MeasureAttribute()
        {
            Divisions = attributesNode.SelectSingleNode("divisions") == null ? null : Int32.Parse(attributesNode.SelectSingleNode("divisions").InnerText),
            Time = attributesNode.SelectSingleNode("time") == null ? null : new MusicalTime()
            {
                Beats = Int32.Parse(attributesNode.SelectSingleNode("time/beats").InnerText),
                BeatType = Int32.Parse(attributesNode.SelectSingleNode("time/beat-type").InnerText),
            },
        };
    }

    // private static Pitch? GetPitch(XmlNode pitchNode)
    // {
    //     if (pitchNode == null)
    //     {
    //         return null;
    //     }
    //     return new Pitch()
    //     {
    //         Step = pitchNode.SelectSingleNode("step").InnerText,
    //         Octave = Int32.Parse(pitchNode.SelectSingleNode("octave").InnerText),
    //         Alter = pitchNode.SelectSingleNode("alter") == null ? null : Int32.Parse(pitchNode.SelectSingleNode("alter").InnerText),
    //     };
    // }

    private static IEnumerable<string> GetScorePartIdList(XmlDocument doc)
    {
        var scorePartNodes = doc.SelectNodes("score-partwise/part-list/score-part");
        //var scorePartNodes = doc.SelectNodes("score-partwise");
        foreach (XmlNode scorePartNode in scorePartNodes)
        {
            var partId = scorePartNode.Attributes["id"].InnerText;
            yield return partId;
        }
    }
}