using UnityEngine;
using UnityEngine.UI;



public class UIAugmentSelect : MonoBehaviour
{
    [SerializeField] GameObject bloodButtons;
    [SerializeField] GameObject mistButtons;
    [SerializeField] GameObject metalButtons;
    [SerializeField] GameObject soulButtons;

    [SerializeField] GameObject mistmistText;
    [SerializeField] GameObject bloodbloodText;
    [SerializeField] GameObject soulsoulText;
    [SerializeField] GameObject metalmetalText;
    [SerializeField] GameObject metalbloodText;
    [SerializeField] GameObject metalmistText;
    [SerializeField] GameObject metalsoulText;
    [SerializeField] GameObject soulmistText;
    [SerializeField] GameObject soulbloodText;
    [SerializeField] GameObject mistbloodText;


    [SerializeField] GameObject[] textHolder;
    [SerializeField] GameObject[] buttonHolder;


    //Clearing Functions


    public void ClearText()
    {
        foreach (GameObject text in textHolder)
        {
            text.SetActive(false);
        }
    }

    public void ClearButtons()
    {
        foreach (GameObject button in buttonHolder)
        {
            button.SetActive(false);
        }
    }


    /// Level 1 Buttons
    
    public void OpenBloodMenu()
    {
        ClearButtons();
        ClearText();

        

        bloodButtons.SetActive(true);
    }

    public void OpenMistMenu()
    {
        ClearButtons();
        ClearText();

        

        mistButtons.SetActive(true);
    }

    public void OpenSoulMenu()
    {
        ClearButtons();
        ClearText();

        

        soulButtons.SetActive(true);
    }

    public void OpenMetalMenu()
    {
        ClearButtons();
        ClearText();

       

        metalButtons.SetActive(true);
    }

    public void ClearAll()
    {
        ClearButtons();
        ClearText();
    }

    /// End Level 1 Buttons
    

    /// Start Level 2 Buttons

    public void MistMist()
    {
        ClearText();

        mistmistText.SetActive(true); 
    }

    public void BloodBlood()
    {
        ClearText();

        bloodbloodText.SetActive(true);
    }

    public void SoulSoul()
    {
        ClearText();

        soulsoulText.SetActive(true);
    }

    public void MetalMetal()
    {
        ClearText();

        metalmetalText.SetActive(true);
    }

    public void MetalBlood()
    {
        ClearText();

        metalbloodText.SetActive(true);
    }

    public void MetalMist()
    {
        ClearText();

        metalmistText.SetActive(true);
    }

    public void MetalSoul()
    {
        ClearText();

        metalsoulText.SetActive(true);
    }

    public void SoulMist()
    {
        ClearText();

        soulmistText.SetActive(true);
    }

    public void SoulBlood()
    {
        ClearText();

        soulbloodText.SetActive(true);
    }

    public void MistBlood()
    {
        ClearText();

        mistbloodText.SetActive(true);
    }
}
