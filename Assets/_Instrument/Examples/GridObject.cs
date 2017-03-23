using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {

    public int interval
    {
        get
        {
            return _interval;
        }
        set
        {
            _interval = Mathf.Clamp(value, intervalMin, intervalMax);
        }
    }
    public int intervalMin = -4; // inclusive
    public int intervalMax = 4;

    private int _interval = 0;

    public Color[] intervalColors;

    public bool isActive = false;

    private Color startColor;
    private Color destColor;
    private Color baseColor;
    private Material mat;

    private Coroutine colorFade;
    private void Start()
    {

        intervalColors = new Color[]
        {
            Color.red,
            new Color(0.5f, 0, 1, 1),
            Color.magenta,
            new Color(1f, 0.5f, 0, 1),
            new Color(1, .8f, 0, 1),
            Color.green,
            Color.cyan,
            new Color(0, 0.5f, 1, 1),
            Color.blue,
        };

        mat = gameObject.GetComponent<Renderer>().material;
        startColor = mat.color;
        destColor = new Color(1, .8f, 0, 1);
        baseColor = destColor;
        
    }


    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeInterval(1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ChangeInterval(-1);
        }
    }

    private void ChangeInterval(int changeAmount)
    {
        if (!isActive)
        {
            Activate();
            return;
        }
        if (colorFade != null) StopCoroutine(colorFade);

        if (interval + changeAmount > intervalMax || interval + changeAmount < intervalMin)
        {
            interval = 0;
            isActive = false;
        }
        interval += changeAmount;
        SetColor();
    }

    private void Activate()
    {
        isActive = true;
        interval = 0;
        SetColor();        
    }
    
    private void SetColor()
    {
        if (isActive)
        {
            int colorIndex = Mathf.Clamp(intervalColors.Length / 2 + interval, 0, intervalColors.Length - 1);
            mat.color = intervalColors[colorIndex];
            destColor = mat.color;
            
        }
        else
        {
            mat.color = startColor;
        }
    }

    IEnumerator ActivateGrid(Color dest, Color orig)
    {
        float effectTime = 0.5f;
        float currTime = effectTime;
        while (currTime > 0)
        {
            currTime -= Time.deltaTime;
            mat.color = Color.Lerp(orig, dest, currTime / effectTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        mat.color = orig;
        yield return null;
    }

    public void TriggerGrid()
    {
        if (isActive)
        {
            colorFade = StartCoroutine(ActivateGrid(Color.white, destColor));
        } else
        {
            colorFade = StartCoroutine(ActivateGrid(new Color(.6f, .6f, .6f, 1), startColor));
        }
    }
}
