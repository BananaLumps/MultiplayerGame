using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Base.ModularUI
{
    public class PropertyWindowController : MonoBehaviour
    {
        public UIObjectController target;
        public TMP_InputField nameText, displayText, xPosition, yPosition, width, height;
        public TMP_Dropdown dataBind, eventBind, dataSource;
        public Toggle isDraggable;
        public bool isDirty = true;

        private void Start()
        {
            nameText.onEndEdit.AddListener(NamePropertyChanged);
            displayText.onEndEdit.AddListener(DisplayTextPropertyChanged);
            xPosition.onEndEdit.AddListener(PositionXPropertyChanged);
            yPosition.onEndEdit.AddListener(PositionYPropertyChanged);
            width.onEndEdit.AddListener(WidthPropertyChanged);
            height.onEndEdit.AddListener(HeightPropertyChanged);
            dataBind.onValueChanged.AddListener(DataBindDropdownChanged);
            eventBind.onValueChanged.AddListener(EvenBindDropdownChanged);
            dataSource.onValueChanged.AddListener(DataSourceDropdownChanged);
            isDraggable.onValueChanged.AddListener(DraggableToggleChanged);
        }

        private void DraggableToggleChanged(bool arg0)
        {
            target.IsDraggable = arg0;
            isDirty = true;
        }

        private void DataSourceDropdownChanged(int arg0)
        {
            throw new NotImplementedException();
        }

        private void EvenBindDropdownChanged(int arg0)
        {
            throw new NotImplementedException();
        }

        private void DataBindDropdownChanged(int arg0)
        {
            throw new NotImplementedException();
        }

        private void HeightPropertyChanged(string arg0)
        {
            target.GetComponentInChildren<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, float.Parse(arg0));
            isDirty = true;
        }

        private void WidthPropertyChanged(string arg0)
        {
            target.GetComponentInChildren<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, float.Parse(arg0));
            isDirty = true;
        }

        private void PositionYPropertyChanged(string arg0)
        {
            target.transform.position = new Vector3(target.transform.position.x, float.Parse(arg0), target.transform.position.z);
            isDirty = true;
        }

        private void PositionXPropertyChanged(string arg0)
        {
            target.transform.position = new Vector3(float.Parse(arg0), target.transform.position.y, target.transform.position.z);
            isDirty = true;
        }

        private void DisplayTextPropertyChanged(string arg0)
        {
            target.displayText = arg0;
            isDirty = true;
        }

        private void NamePropertyChanged(string arg0)
        {
            target.gameObject.name = arg0;
            isDirty = true;
        }

        private void Update()
        {
            if (target == null)
                return;
            if (isDirty)
            {
                nameText.text = target.gameObject.name;
                displayText.text = target.displayText;
                xPosition.text = target.transform.position.x.ToString();
                yPosition.text = target.transform.position.y.ToString();
                width.text = target.GetComponentInChildren<RectTransform>().rect.width.ToString();
                height.text = target.GetComponentInChildren<RectTransform>().rect.height.ToString();
                isDraggable.isOn = target.IsDraggable;
                isDirty = false;
            }
            //dataBind.value = (int)target.dataBind;
            //eventBind.value = (int)target.eventBind;
            //dataSource.value = (int)target.dataSource;
        }
    }
}
