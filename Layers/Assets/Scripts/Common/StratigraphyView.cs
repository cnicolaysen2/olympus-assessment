
using System;
using UnityEngine;
using UnityEngine.UI;

public class StratigraphyView : MonoBehaviour
{
    [SerializeField]
    private Sprite[] stratigraphyStates;

    [SerializeField]
    private Image placeholder;

    [SerializeField]
    private Button showHideButton;

    [SerializeField]
    private Button age2Button;

    [SerializeField]
    private Button age3Button;

    [SerializeField]
    private Button age4Button;

    public event Action<int> AgeSelection = (age) => { };

    // Start is called before the first frame update
    void Awake()
    {
        this.showHideButton.onClick.AddListener( ShowHide );

        this.age2Button.onClick.AddListener(SelectAge2);
        this.age3Button.onClick.AddListener(SelectAge3);
        this.age4Button.onClick.AddListener(SelectAge4);
        
    }

    private void SelectAge2()
    {
        if(isShown)
        {
            placeholder.sprite = stratigraphyStates[2];
            AgeSelection(2);
        }
    }


    private void SelectAge3()
    {
        if (isShown)
        {
            placeholder.sprite = stratigraphyStates[3];
            AgeSelection(3);
        }
    }
    private void SelectAge4()
    {
        if (isShown)
        {
            placeholder.sprite = stratigraphyStates[4];
            AgeSelection(4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isShown;
    private void ShowHide()
    {
        if (isShown)
        {
            placeholder.sprite = null;
            placeholder.color = new Color(0, 0, 0, 0);
        }
        else
        {
            placeholder.sprite = stratigraphyStates[1];
            placeholder.color = new Color(1, 1, 1, 1);
        }
        isShown = !isShown;
        AgeSelection(0);
    }
}
