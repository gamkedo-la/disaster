using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_AlphaOvertime : MonoBehaviour {
    /**************
     * Adjust the alpha over a duration of time. Creates fade in and out effects. 
     * 
     * Add UI gameobjects that have a color field that you would like adjusted. 
     * ***********/
    [SerializeField]
    private Text[] m_TextToAlpha;   //Array of text objects to alpha over time
    [SerializeField]
    private Image[] m_ImageToAlpha; //Array of image objects to alpha over time

    private float[] m_StartTextAlpha; //Array of floats to track the starting alpha value of the objects stored in the Text array
    private float[] m_StartImageAlpha;//Array of floats to track the starting alpha value of the objects stored in the Image array

    [SerializeField]
    private float m_alphaDuration = 1.0f; //Time it takes to alpha from start to finish
    [SerializeField]
    private bool m_reverseFade = false; //By default we are fading to 0 alpha. If this bool is true, we are fading to 100 alpha

    private bool isFading = false; //Determines if we are currenlty in the progress of fading.
    private bool m_disableAfterFade = false; //Disable this gameobject after fade is done
    
    private float elapsedTime;

    // Use this for initialization
    void Start () {
    }
	
    void SetupAlphaArrays()
    {
        if (m_TextToAlpha != null && m_TextToAlpha.Length > 0)
        {
            m_StartTextAlpha = new float[m_TextToAlpha.Length];
            //Setup m_StartTextAlpha array based on the values in m_TextToAlpha
            for (int i = 0; i < m_TextToAlpha.Length; i++)
            {
                if(m_reverseFade) //if reverseFade is set, get the difference between 1.0f and the current alpha. As we are fading to 1.0f instead of 0.0f
                    m_StartTextAlpha[i] = 1.0f - m_TextToAlpha[i].color.a;
                else
                    m_StartTextAlpha[i] = m_TextToAlpha[i].color.a;
            }
        }

        if (m_ImageToAlpha != null && m_ImageToAlpha.Length > 0)
        {
            m_StartImageAlpha = new float[m_ImageToAlpha.Length];
            //Setup m_StartTextAlpha array based on the values in m_TextToAlpha
            for (int i = 0; i < m_ImageToAlpha.Length; i++)
            {
                if (m_reverseFade) //if reverseFade is set, get the difference between 1.0f and the current alpha. As we are fading to 1.0f instead of 0.0f
                    m_StartImageAlpha[i] = 1.0f - m_ImageToAlpha[i].color.a;
                else
                    m_StartImageAlpha[i] = m_ImageToAlpha[i].color.a;
            }
        }
    }

	// Update is called once per frame
	void LateUpdate () {
	    if(isFading)
        {
            FadeTextArrays();
            FadeImageArrays();
            elapsedTime += Time.deltaTime;
            if (elapsedTime > m_alphaDuration)
            {
                isFading = false;

                if (m_disableAfterFade)
                {
                    gameObject.SetActive(false);
                    ResetFade();
                }
            }
        }
	}

    void FadeTextArrays()
    {
        float deltaChange = GetDeltaPercentage();
        if(m_TextToAlpha != null && m_TextToAlpha.Length > 0 && m_StartTextAlpha != null && m_TextToAlpha.Length == m_StartTextAlpha.Length)
        {
            for (int i = 0; i < m_TextToAlpha.Length; i++)
            {
                Color nextColor = m_TextToAlpha[i].color;
                if(m_reverseFade)
                {
                    nextColor.a += m_StartTextAlpha[i] * deltaChange;
                }
                else
                {
                    nextColor.a -= m_StartTextAlpha[i] * deltaChange;
                }
                Mathf.Clamp(nextColor.a, 0.0f, 1.0f);
                m_TextToAlpha[i].color = nextColor;
            }
        }
    }

    void FadeImageArrays()
    {
        float deltaChange = GetDeltaPercentage();
        if (m_ImageToAlpha != null && m_ImageToAlpha.Length > 0 && m_StartImageAlpha != null && m_ImageToAlpha.Length == m_StartImageAlpha.Length)
        {
            for (int i = 0; i < m_ImageToAlpha.Length; i++)
            {
                Color nextColor = m_ImageToAlpha[i].color;
                if (m_reverseFade)
                {
                    nextColor.a += m_StartImageAlpha[i] * deltaChange;
                }
                else
                {
                    nextColor.a -= m_StartImageAlpha[i] * deltaChange;
                }
                Mathf.Clamp(nextColor.a, 0.0f, 1.0f);
                m_ImageToAlpha[i].color = nextColor;
            }
        }
    }

    void ResetAlphaImageArrays()
    {
        if (m_ImageToAlpha != null && m_ImageToAlpha.Length > 0 && m_StartImageAlpha != null && m_ImageToAlpha.Length == m_StartImageAlpha.Length)
        {
            for (int i = 0; i < m_ImageToAlpha.Length; i++)
            {
                Color nextColor = m_ImageToAlpha[i].color;
                if (m_reverseFade)
                {
                    nextColor.a = 1.0f - m_StartImageAlpha[i];
                }
                else
                {
                    nextColor.a = m_StartImageAlpha[i];
                }
                Mathf.Clamp(nextColor.a, 0.0f, 1.0f);
                m_ImageToAlpha[i].color = nextColor;
            }
        }
    }

    void ResetAlphaTextArrays()
    {
        if (m_TextToAlpha != null && m_TextToAlpha.Length > 0 && m_StartTextAlpha != null && m_TextToAlpha.Length == m_StartTextAlpha.Length)
        {
            for (int i = 0; i < m_TextToAlpha.Length; i++)
            {
                Color nextColor = m_TextToAlpha[i].color;
                if (m_reverseFade)
                {
                    nextColor.a = 1.0f - m_StartTextAlpha[i];
                }
                else
                {
                    nextColor.a = m_StartTextAlpha[i];
                }
                Mathf.Clamp(nextColor.a, 0.0f, 1.0f);
                m_TextToAlpha[i].color = nextColor;
            }
        }
    }



    //Get the percentage of time that has passed since last frame, based on total duration of fade
    float GetDeltaPercentage()
    {
        //get a percentage of change that needs to occur, since last LateUpdate
        if(m_alphaDuration != 0)
            return Time.deltaTime / m_alphaDuration;
        return 0.0f;
    }

    public void StartFade()
    {
        SetupAlphaArrays();
        isFading = true;
        elapsedTime = 0.0f;
    }

    public void ResetFade()
    {
        ResetAlphaTextArrays();
        ResetAlphaImageArrays();
    }

    public void DisableAfterFade(bool flag)
    {
        m_disableAfterFade = flag;
    }
}
