/* ************************************
 * Sequencer.cs
 * By Jasper Degens
 * 26-03-2017
 * 
 * This is an example of leveraging an
 * instrument in a larger program. This
 * is a basic step sequencer that can take
 * a sequence of notes as an input.
 * 
 * ************************************/


using UnityEngine;
using System.Collections;

namespace jasper.Music.Sequencer{

	public enum BeatDivision{
		QUARTER_NOTE,
		EIGHT_NOTE,
		SIXTEENTH_NOTE,
		TRIPLET
	}  

	[RequireComponent(typeof(BaseInstrument))]
	public class Sequencer : MonoBehaviour {

        #region Public Properties
        public int[] NoteSequence;
		public bool randomize = false;
		public BeatDivision noteMode;

		[Range(10, 250)]
		public int bpm = 120;

		[Range(0, 1)]
		public float swing = 0;

		[Range(1, 12)]
		public int timeSignature = 4;

		public int currentBeat = -1;

        public bool debug = false;

        #endregion


        #region Private Properties

        protected BaseInstrument instrument;

        // Time tracking props
        protected double sampleRate;
        protected double nextTick;
		protected bool running = false;
        protected double tickIntervalInSamples = 0;

		protected int totalBeats = 0;
        protected double swingOffset = 0;
        protected int currNotePosition = 0;

        protected int currBpm;
        protected int currTimeSig;
        protected float currSwing;
        protected BeatDivision currNoteMode;

		#endregion

		// Use this for initialization
		public virtual void Start () {
			instrument = gameObject.GetComponent<BaseInstrument> ();

            NoteSequence = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

			sampleRate = AudioSettings.outputSampleRate;
		}

		// Update is called once per frame
		public virtual void Update () {

			if (currBpm != bpm || currNoteMode != noteMode) {
				currBpm = bpm;
				currNoteMode = noteMode;
				tickIntervalInSamples = 60.0f / bpm / (float)GetBeatDivision(noteMode);
			}

			if(running){
				if (nextTick <= AudioSettings.dspTime) {
					// Trigger note
					HandleBeat();

					nextTick += tickIntervalInSamples;

					// add swing???
					if(totalBeats % 2 == 1){
						nextTick += swing * 0.9f * tickIntervalInSamples; // max at 90% between beats
					}
				}



			
			}
		}

		public virtual void ToggleSequencer(){
            running = !running;
            if (running)
				StartSequencer ();
			else
				StopSequencer ();
		}

		public virtual void StartSequencer(){

			currBpm = bpm;
			currTimeSig = timeSignature;
			currSwing = swing;
			currNoteMode = noteMode;

			tickIntervalInSamples = 60.0f / bpm / (float)GetBeatDivision(noteMode);
			nextTick = AudioSettings.dspTime;
			running = true;
		}

		public virtual void StopSequencer(){
			running = false;
		}

		private int GetBeatDivision(BeatDivision div){

			switch (div) {

			case BeatDivision.EIGHT_NOTE:
				return 2;

			case BeatDivision.QUARTER_NOTE:
				return 1;

			case BeatDivision.SIXTEENTH_NOTE:
				return 4;

			case BeatDivision.TRIPLET:
				return 3;

			default:
				return 4;

			}

		}

		protected virtual void HandleBeat(){

			currentBeat++;
			totalBeats++;
			if (currentBeat > timeSignature) {
				currentBeat = 0;
			}
			int noteIndex = 0;
			if (randomize) {
				noteIndex = Random.Range (0, NoteSequence.Length);
			} else {
				if (currNotePosition > NoteSequence.Length - 1)
					currNotePosition = 0;

				noteIndex = currNotePosition;
				currNotePosition++;
			}
			instrument.PlayNote (NoteSequence[noteIndex]);

            if (debug)
            {
                print(currentBeat);
            }
        }



	}

}