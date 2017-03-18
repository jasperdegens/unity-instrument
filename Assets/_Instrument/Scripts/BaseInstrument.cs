using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osc;

namespace jasper.Music
{


    public enum NoteMode
    {
        RELATIVE_MODE,
        STATIC_MODE
    }

    public enum OutputMode
    {
        MIDI,
        AUDIOSOURCE,
        OSC
    }



    [RequireComponent(typeof(OscPortSocket))]
    public class BaseInstrument : MonoBehaviour
    {



        /* TODO:
             - Should have midi out mode??
             - What are we doing about sound??
        */

        #region Public Variables

        public NoteMode noteMode = NoteMode.STATIC_MODE;
        public OutputMode outputMode = OutputMode.OSC;
        public bool isDebugMode = true;

        public int octave
        {
            get { return _octave; }
            set
            {
                if (value >= -4 && value < 4)
                {
                    _octave = value;
                }
            }
        }

        public int key// 0 = c, 1 = d, etc
        {
            get { return _key; }
            set
            {
                if (value > 0 && value < 12)
                {
                    _key = value;
                }
            }
        }

        #endregion


        #region Private Variables

        // Music Managers
        private ScaleManager Scales;
        private ChordManager Chords;

        // Position Properties
        private int currInterval = 0;
        private int _key = 0;

        private int _octave = 0; // Range(-4, 4) inclusive, middle c => octave = 0



        /*********************** Sound Output Props ************************/
        OscPortSocket socket;


        #endregion




        #region Unity Functions

        void Start()
        {
            // Setup musical data
            Scales = gameObject.AddComponent<ScaleManager>();
            Chords = gameObject.AddComponent<ChordManager>();


            // Setup output modes
            socket = gameObject.GetComponent<OscPortSocket>(); // no reveice handler by default

        }

        #endregion




        #region Public Functions

        public void PlayNote(int interval)
        {
            PlayNote(interval, noteMode);
        }

        public void PlayNote(int interval, NoteMode mode)
        {
            int noteNum = GetSingleNote(interval, mode);

            if (isDebugMode)
            {
                Debug.Log("Single Note: " + noteNum);
            }
        }

        public void PlayChord(string chordName)
        {
            int[] chordNotes = GetChordNotes(chordName);

            for (int i = 0; i < chordNotes.Length; i++)
            {
                PlayNote(chordNotes[i], NoteMode.STATIC_MODE);
            }
        }


        // TODO: ALL OF THIS
        public void OutputNote(int noteNum)
        {
            switch (outputMode)
            {
                case OutputMode.OSC:
                    JsonUtility.ToJson("hellp");
                    break;
            }
        }





        /**************** Adjust Instument Parameters ****************/

        public void SetKey(int key)
        {
            this.key = key;
        }

        public void SetKey(string key)
        {
            int newKey = 0;
            switch (key)
            {
                case "c":
                    newKey = 0;
                    break;

                case "d":
                    newKey = 1;
                    break;

                case "e":
                    newKey = 2;
                    break;

                default:
                    newKey = 2;
                    break;
            }

            SetKey(newKey);
        }

        #endregion

        #region Private Functions

        // just in case note num has other functions
        private int NoteNumToMidiNum(int noteNum)
        {
            return noteNum;
        }

        private int GetSingleNote(int interval, NoteMode mode)
        {
            ScalePosition pos = new ScalePosition();
            // Handle Static Mode
            if (mode == NoteMode.STATIC_MODE)
            {
                pos = Scales.GetNoteFromScale(interval);
            }
            if (mode == NoteMode.RELATIVE_MODE)
            {
                currInterval += interval;
                int finalInterval = currInterval; // octave offset (middle c at 0 octave)
                pos = Scales.GetNoteFromScale(finalInterval);
                octave = pos.octaveChange; // need to update octave if changed
            }

            // center around 4th octave (middle c)
            return NoteFromScalePosition(pos);
        }

        private int[] GetChordNotes(string chord)
        {
            int[] notes = Chords.GetChord(chord);
            if (notes != null)
            {
                return notes;
            }
            return new int[0];
        }

        private int NoteFromScalePosition(ScalePosition pos)
        {
            int oct = Mathf.Clamp(5 + pos.octaveChange, 1, 10);
            int note = pos.noteNum + key + oct * 12;
            return Mathf.Clamp(note, 0, 127);
        }

        #endregion
    }
}