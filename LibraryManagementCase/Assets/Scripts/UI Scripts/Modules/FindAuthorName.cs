using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Modules
{
    public class FindAuthorName : MonoBehaviour
    {
        #region UI Elements

        [Header("Inputs")] 
        [SerializeField] private TMP_InputField authorNameInput; 

        [Header("Buttons")] 
        [SerializeField] private Button searchButton;
        
        [Header("Text Areas")]
        [SerializeField] private TextMeshProUGUI warningText;
    
        #endregion
        
        #region Main Functions

        private void Start()
        {
            ButtonInitialize();
        }
        
        private void SearchAuthorName()
        {
            if (authorNameInput.text != "")
            {
                EventManager.Instance.OnSearchAuthorName.Invoke(authorNameInput.text);
            }
            else
            {
                SendWarning("Lütfen bir yazar ismi yazınız.");
            }
        }

        #endregion
        
        #region UI Functions

        private void ButtonInitialize()
        {
            searchButton.onClick.AddListener(SearchAuthorName);
            
        }
        
        private void SendWarning(string message)
        {
            warningText.text = message;
        }
        #endregion
    }
}
