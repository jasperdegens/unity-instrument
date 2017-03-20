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

        [Range(0, 15)]
        public int midiChannelOut = 0;

        [Range(-4, 4)]
        public int minOctave = -4;
        [Range(-4, 4)]
        public int maxOctave = 4;
        

        public bool debug = true;

        public int octave
        {
            get { return _octave; }
            set
            {
                if (value >= -4 && value < 4)
                {
                    _octave = Mathf.Clamp(value, minOctave, maxOctave);
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
        protected ScaleManager Scales;
        protected ChordManager Chords;

        // Position Properties
        protected int currInterval = 0;
        protected int _key = 0;

        protected int _octave = 0; // Range(-4, 4) inclusive, middle c => octave = 0

		// Note off arrays to check timings
		// when noteStatus[i] == -1, that means note is off
		// when noteStatus[i] <= 0, that means note should be turned off
		// when noteStatus[i] >0, note is waiting to be turned off
		private float[] noteOffCountdown;
		private List<int> activeNotes = new List<int>();

        /*********************** Sound Output Props ************************/
        protected OscPortSocket socket;
        protected string OSC_PATH = "/midi";

        #endregion




        #region Unity Functions

        public virtual void Start()
        {
            // Setup musical data
			Scales = ScriptableObject.CreateInstance<ScaleManager>();
			Chords = ScriptableObject.CreateInstance<ChordManager> ();


			// Setup noteOff "queue"
			noteOffCountdown = new float[127];
			for (int i = 0; i < 127; i++) {
				noteOffCountdown [i] = -1;
			}

            // Setup output modes
			if(gameObject != null){
            	socket = gameObject.GetComponent<OscPortSocket>(); // no reveice handler by default
			} else {
				socket = new OscPortSocket ();
			}

        }

		public virtual void Update(){

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

		public virtual void PlayNote(int interval, float duration = 1.5f)
        {
            PlayNote(interval, noteMode);
        }

		public virtual void PlayNote(int interval, NoteMode mode, float duration = 1.5f)
        {
            int noteNum = GetSingleNote(interval, mode);


			ActivateNote (noteNum, duration);


			// if midi we need note on and note off
			if (outputMode == OutputMode.MIDI) {

			}

            if (debug)
            {
                Debug.Log("Single Note: " + noteNum);
            }
        }

		public virtual void PlayChord(string chordName, float duration = 1.5f)
        {
            int[] chordNotes = GetChordNotes(chordName);

            for (int i = 0; i < chordNotes.Length; i++)
            {
                PlayNote(chordNotes[i], NoteMode.STATIC_MODE, duration);
            }
        }


        // TODO: ALL OF THIS
        public virtual void NoteOn(int noteNum)
        {
            switch (outputMode)
            {
                case OutputMode.OSC:

                    MidiCommand com = MidiCommandHelper.NoteOnCommand(midiChannelOut, noteNum, 127);
                    OscOutput(com);

                    break;


			case OutputMode.MIDI:
				
				MidiOut.SendNoteOn ((MidiChannel)midiChannelOut, noteNum, 1);
				break;

			default:
				break;

            }
        }

		public virtual void NoteOff(int noteNum)
		{

			switch (outputMode)
			{
			case OutputMode.OSC:
                    MidiCommand com = MidiCommandHelper.NoteOffCommand(midiChannelOut, noteNum);
                    OscOutput(com);
    				break;


			case OutputMode.MIDI:
				MidiOut.SendNoteOff (MidiChannel.Ch1, noteNum);
				break;

			default:
				break;

			}
		}




        /**************** Adjust Instument Parameters ****************/

        public virtual void SetKey(int key)
        {
            this.key = key;
        }

        public virtual void SetKey(string key)
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

        /******************** OUTPUT HELPERS ************************/
        private void OscOutput(MidiCommand command)
        {
            var osc = new Osc.MessageEncoder(OSC_PATH);
            osc.Add(JsonUtility.ToJson(command));
            print(command);
            socket.Send(osc);
        }



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
                pos = Scales.GetNoteFromScale(interval, minOctave, maxOctave);
            }
            if (mode == NoteMode.RELATIVE_MODE)
            {
                currInterval += interval;
                int finalInterval = currInterval; // octave offset (middle c at 0 octave)
                pos = Scales.GetNoteFromScale(finalInterval, minOctave, maxOctave);
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