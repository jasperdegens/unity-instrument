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
		public OutputMode outputMode = OutputMode.MIDI;
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

		// Note off arrays to check timings
		// when noteStatus[i] == -1, that means note is off
		// when noteStatus[i] <= 0, that means note should be turned off
		// when noteStatus[i] >0, note is waiting to be turned off
		private float[] noteOffCountdown;
		private List<int> activeNotes = new List<int>();

        /*********************** Sound Output Props ************************/
        private OscPortSocket socket;

        #endregion




        #region Unity Functions

        void Start()
        {
            // Setup musical data
            Scales = gameObject.AddComponent<ScaleManager>();
            Chords = gameObject.AddComponent<ChordManager>();

			// Setup noteOff "queue"
			noteOffCountdown = new float[127];
			for (int i = 0; i < 127; i++) {
				noteOffCountdown [i] = -1;
			}

            // Setup output modes
            socket = gameObject.GetComponent<OscPortSocket>(); // no reveice handler by default

        }

		public void Update(){

			// check note off queue
			for (int i = 0; i < activeNotes.Count; i++) {
				int noteNum = activeNotes [i];
				float noteOffTime = noteOffCountdown [noteNum];
				noteOffTime -= Time.deltaTime;


				// if time is less than 0 turn note off and dequeue
				if (noteOffTime <= 0) {
					NoteOff (noteNum);
					activeNotes.RemoveAt (i); // TODO: need to check if this will screw up for loop and cause seg fault
				}

				noteOffCountdown[noteNum] = noteOffTime;
			}

		}

        #endregion




        #region Public Functions

		public void PlayNote(int interval, float duration = 1.5f)
        {
            PlayNote(interval, noteMode);
        }

		public void PlayNote(int interval, NoteMode mode, float duration = 1.5f)
        {
            int noteNum = GetSingleNote(interval, mode);


			ActivateNote (noteNum, duration);


			// if midi we need note on and note off
			if (outputMode == OutputMode.MIDI) {

			}

            if (isDebugMode)
            {
                Debug.Log("Single Note: " + noteNum);
            }
        }

		public void PlayChord(string chordName, float duration = 1.5f)
        {
            int[] chordNotes = GetChordNotes(chordName);

            for (int i = 0; i < chordNotes.Length; i++)
            {
                PlayNote(chordNotes[i], NoteMode.STATIC_MODE, duration);
            }
        }


        // TODO: ALL OF THIS
        public void NoteOn(int noteNum)
        {
            switch (outputMode)
            {
                case OutputMode.OSC:
                    JsonUtility.ToJson("hellp");
                    break;


			case OutputMode.MIDI:
				
				MidiOut.SendNoteOn (MidiChannel.Ch1, noteNum, 1);
				break;

			default:
				break;

            }
        }

		public void NoteOff(int noteNum)
		{

			switch (outputMode)
			{
			case OutputMode.OSC:
				JsonUtility.ToJson("hellp");
				break;


			case OutputMode.MIDI:
				MidiOut.SendNoteOff (MidiChannel.Ch1, noteNum);
				break;

			default:
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

		// Turn on note and add to noteOff pool
		void ActivateNote(int noteNum, float duration){

			NoteOn (noteNum);

			if (activeNotes.Exists(elem => elem == noteNum)) {
				noteOffCountdown [noteNum] = duration;

			} else {
				activeNotes.Add (noteNum);
				noteOffCountdown [noteNum] = duration;
			}
		}


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