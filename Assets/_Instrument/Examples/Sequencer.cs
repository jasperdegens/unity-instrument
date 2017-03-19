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
		public BeatDivision noteMode;

		[Range(10, 250)]
		public int bpm = 120;

		[Range(0, 1)]
		public float swing = 0;

		[Range(1, 12)]
		public int timeSignature = 4;

		public int currentBeat = -1;

		#endregion


		#region Private Properties

		private BaseInstrument instrument;

		// Time tracking props
		private double sampleRate;
		private double nextTick;
		private bool running = false;
		private double tickIntervalInSamples = 0;

		private int totalBeats = 0;
		private double swingOffset = 0;

		private int currBpm;
		private int currTimeSig;
		private float currSwing;
		private BeatDivision currNoteMode;

		#endregion

		// Use this for initialization
		void Start () {
			instrument = gameObject.GetComponent<BaseInstrument> ();

			sampleRate = AudioSettings.outputSampleRate;
		}

		// Update is called once per frame
		void Update () {

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
					nextTick += swing * 0.75f * tickIntervalInSamples; // max at 75% between beats
				}



			
			}
		}

		public void HandleButton(){
			if (running)
				StopSequencer ();
			else
				StartSequencer ();
		}

		public void StartSequencer(){

			currBpm = bpm;
			currTimeSig = timeSignature;
			currSwing = swing;
			currNoteMode = noteMode;

			tickIntervalInSamples = 60.0f / bpm / (float)GetBeatDivision(noteMode);
			nextTick = AudioSettings.dspTime;
			running = true;
		}

		public void StopSequencer(){
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

		private void HandleBeat(){

			currentBeat++;
			totalBeats++;
			if (currentBeat > timeSignature) {
				currentBeat = 0;
			}

			print (currentBeat);
			instrument.PlayNote (0);



		}

	}

}