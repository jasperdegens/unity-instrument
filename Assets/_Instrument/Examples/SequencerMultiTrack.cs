﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.Music.Sequencer{

	public class SequencerMultiTrack : Sequencer {

		public struct TrackConfiguration
		{
			public int MidiChannelOutput;
			public OutputMode outMode;

			TrackConfiguration(int output = 0, OutputMode mode = OutputMode.MIDI){
				MidiChannelOutput = output;
				outMode = mode;
			}

		};

	    public int tracks = 4;
	    public int steps = 16;
		public TrackConfiguration[] trackConfig;

	    private GridObject[,] grid;
	    private int _currPosition = 0;
	    private int currPosition
	    {
	        get
	        {
	            return _currPosition;
	        }
	        set
	        {
	            _currPosition = value % steps;
	        }
	    }


	    public override void Start()
	    {
	        base.Start();

			trackConfig = new TrackConfiguration[tracks];
	        grid = new GridObject[tracks,steps];

	        // center around transform midpoint
	        float startZ = (steps / 2.0f) + 0.5f;
	        float startX = -(tracks / 2.0f) + 0.5f;
	        Vector3 startPos = new Vector3(startX, 0, startZ);
	        for (int i = 0; i < tracks; i++)
	        {
	            for (int j = 0; j < steps; j++)
	            {
	                var go = GameObject.CreatePrimitive(PrimitiveType.Quad);

	                go.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Color"));
					//go.transform.rotation = Quaterni
	                go.transform.position = startPos + Vector3.right * i - Vector3.back * j;
	                go.transform.localScale *= 0.8f;
					go.transform.parent = gameObject.transform;
	                GridObject gridObj = go.AddComponent<GridObject>();
	                grid[i, j] = gridObj;
	            }
	        }

	    }

	    public override void Update()
	    {
	        if (Input.GetKeyDown(KeyCode.Space))
	        {
	            ToggleSequencer();
	        }
	        base.Update();
	    }

	    protected override void HandleBeat()
	    {
	        for (int i = 0; i < tracks; i++)
	        {
	            var g = grid[i, currPosition];
	            if (g.isActive)
	            {
	                g.TriggerGrid();
					instrument.outputMode = trackConfig [i].outMode;
					instrument.midiChannelOut = trackConfig [i].MidiChannelOutput;
	                instrument.PlayNote(g.interval);
	            }
	            else
	            {
	                g.TriggerGrid();
	            }
	        }



	        currentBeat++;
	        totalBeats++;
	        if (currentBeat > timeSignature)
	        {
	            currentBeat = 0;
	        }
	        currPosition++;
	        currNotePosition++;

	        if (debug)
	        {
	            print(currentBeat);
	        }

	    }
	}
}