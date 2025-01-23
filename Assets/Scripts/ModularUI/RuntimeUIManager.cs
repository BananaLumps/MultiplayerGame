using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Base.ModularUI
{
    public class RuntimeUIManager : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject buttonPrefab;
        public GameObject textPrefab;
        public GameObject imagePrefab;

        private List<GameObject> uiElements = new List<GameObject>();

        void Start()
        {
            if (canvas == null)
            {
                canvas = FindObjectOfType<Canvas>();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                CreateButton();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                CreateText();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                CreateImage();
            }
        }

        public void CreateButton()
        {
            GameObject button = Instantiate(buttonPrefab, canvas.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));
            button.GetComponentInChildren<Text>().text = "Button";
            uiElements.Add(button);
        }

        public void CreateText()
        {
            GameObject text = Instantiate(textPrefab, canvas.transform);
            text.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));
            text.GetComponent<Text>().text = "Text";
            uiElements.Add(text);
        }

        public void CreateImage()
        {
            GameObject image = Instantiate(imagePrefab, canvas.transform);
            image.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));
            uiElements.Add(image);
        }

        public void EditElement(GameObject element, Vector2 position, Vector2 size, string text = null)
        {
            RectTransform rectTransform = element.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = size;

            if (element.GetComponentInChildren<Text>() != null && text != null)
            {
                element.GetComponentInChildren<Text>().text = text;
            }
        }
    }
}
