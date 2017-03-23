using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace jasper.Music
{

    public enum ScaleTypes
    {
        Major,
        Minor,
        HarmonicMinor,
        Blues,
		Kenken,
		Custom1
    }
    
    public struct ScalePosition
    {
        public int noteNum;
        public int octaveChange;
        public ScalePosition(int note, int octave)
        {
            noteNum = note;
            octaveChange = octave;
        }
    }

    public class ScaleManager : MusicDataStore
    {

        private BaseMusicData currScale;
        private string currScaleName;
        

		public ScaleManager(){
			this.Setup();
		}

		private void Setup()
        {
            CreateDefaultScales();
            currScaleName = "major";
            currScale = dataStore[currScaleName];

        }

        public ScalePosition GetNoteFromScale(int scalePosition, int minOctave = -4, int maxOctave = 5, bool wrapMode = true){
            int octave = (scalePosition / currScale.length);
            int pos = scalePosition % currScale.length;
            if(pos < 0) // test if position is a decreasing interval
            {
                pos = currScale.length + pos;
                octave -= 1;
            }
            if (wrapMode)
            {
                if (octave > maxOctave)
                {
                    octave = minOctave + octave % (maxOctave - minOctave);
                } else if (octave < minOctave)
                {
                    octave = maxOctave - octave % (maxOctave - minOctave);
                }
            } else {
                octave = Mathf.Clamp(octave, minOctave, maxOctave);
            }
            
            int note = currScale.notes[pos];
            return new ScalePosition(note, octave);
        }

		public void SetScale(ScaleTypes type){
			switch (type) {
			case ScaleTypes.Major:
				SetScale ("major");
				break;
			case ScaleTypes.Minor:
				SetScale ("minor");
				break;
			case ScaleTypes.HarmonicMinor:
				SetScale ("harmonicMinor");
				break;
			case ScaleTypes.Blues:
				SetScale ("blues");
				break;

			case ScaleTypes.Kenken:
				SetScale ("kenken");
				break;
			case ScaleTypes.Custom1:
				SetScale ("custom1");
				break;

			default:
				break;
			}

		}

        public void SetScale(string name)
        {
            BaseMusicData scale;
            if (dataStore.TryGetValue(name, out scale))
            {
                currScale = scale;
                currScaleName = scale.name;
            }
        }

        public void AddScale(string name, int[] notes)
        {
            AddMusicData(name, notes);
        }

        public bool RemoveScale(string name)
        {
            return RemoveMusicData(name);
        }

        void CreateDefaultScales()
        {
            int[] major = new int[] { 0, 2, 4, 5, 7, 9, 11 };
            int[] minor = new int[] { 0, 2, 3, 5, 7, 9, 10 };
            int[] harmonicMinor = new int[] { 0, 2, 3, 5, 7, 8, 11 };
            int[] blues = new int[] { 0, 3, 5, 6, 7, 10 };
			int[] kenken = new int[] { 0, 2, 5, 7, 9, 12, 14 };
			int[] custom1 = new int[] { 0, 10, 2, 4, 5, 9, 7, 13, -2 };


            AddScale("major", major);
            AddScale("minor", minor);
            AddScale("harmonicMinor", harmonicMinor);
            AddScale("blues", blues);
			AddScale ("kenken", kenken);
			AddScale ("custom1", custom1);
        }

    }

}