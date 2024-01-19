using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Modules
{
    public class FindBookTitle : MonoBehaviour
    {
        #region UI Elements

        [Header("Inputs")] 
        [SerializeField] private TMP_InputField bookNameInput; 

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
        
        private void SearchBookName()
        {
            if (bookNameInput.text != "")
            {
                EventManager.Instance.OnSearchBookName.Invoke(bookNameInput.text);
            }
            else
            {
                SendWarning("Lütfen bir kitap ismi yazınız.");
            }
        }

        #endregion

        #region UI Functions

        private void ButtonInitialize()
        {
            searchButton.onClick.AddListener(SearchBookName);
            
        }
        
        private void SendWarning(string message)
        {
            warningText.text = message;
        }

        #endregion
    }
}
