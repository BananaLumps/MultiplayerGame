using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomDropdownController : MonoBehaviour
{
    public GameObject contextMenuPrefab;
    private GameObject contextMenuInstance;
    public Button dropdownButton;

    void Start()
    {
        // Hide the context menu initially
        contextMenuInstance = Instantiate(contextMenuPrefab, transform);
        contextMenuInstance.SetActive(false);

        // Add listener to the dropdown button
        dropdownButton.onClick.AddListener(ToggleContextMenu);
    }

    void ToggleContextMenu()
    {
        contextMenuInstance.SetActive(!contextMenuInstance.activeSelf);
        if (contextMenuInstance.activeSelf)
        {
            ShowContextMenu();
        }
    }

    public void ShowContextMenu()
    {
        // Clear existing options
        foreach (Transform child in contextMenuInstance.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new options
        List<string> options = GetContextMenuOptions();
        foreach (string option in options)
        {
            GameObject optionButton = new GameObject(option);
            optionButton.transform.SetParent(contextMenuInstance.transform);
            Button button = optionButton.AddComponent<Button>();
            Text buttonText = optionButton.AddComponent<Text>();
            buttonText.text = option;
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            button.onClick.AddListener(() => OnOptionSelected(option));
        }
    }

    private List<string> GetContextMenuOptions()
    {
        // Replace with your logic to get context menu options
        return new List<string> { "Option 1", "Option 2", "Option 3" };
    }

    private void OnOptionSelected(string option)
    {
        Debug.Log("Selected: " + option);
        contextMenuInstance.SetActive(false);
    }
}
