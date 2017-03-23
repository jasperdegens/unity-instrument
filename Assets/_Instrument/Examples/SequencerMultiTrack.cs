using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jasper.Music.Sequencer;

public class SequencerMultiTrack : Sequencer {

    public GameObject gridPrefab;
    public int steps = 16;
    public int tracks = 4;


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

        grid = new GridObject[tracks,steps];

        // center around transform midpoint
        float startX = -(steps / 2.0f) + 0.5f;
        float startY = (tracks / 2.0f) - 0.5f;
        Vector3 startPos = new Vector3(startX, startY, 0);
        for (int i = 0; i < tracks; i++)
        {
            for (int j = 0; j < steps; j++)
            {
                var go = Instantiate(gridPrefab, gameObject.transform);
                go.transform.position = startPos + Vector3.down * i + Vector3.right * j;
                grid[i, j] = go.GetComponent<GridObject>();
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
