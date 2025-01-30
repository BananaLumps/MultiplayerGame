using Base.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Base.ModularUI.DataBinding
{
    public class TestScript : MonoBehaviour
    {
        //// Start is called before the first frame update
        //public InventoryController inventory;
        //public TMP_Dropdown sourcesDropdown, bindsDropdown;
        //void Start()
        //{
        //    inventory = gameObject.AddComponent<InventoryController>();
        //    inventory.Init("Player");
        //    DataBindManager.Init();
        //    DataBindManager.AddDataSource("Player/Inventory", inventory);
        //    DataBindManager.AddDataSource("Player/Inventory/Items", inventory.Items);
        //    DataBindManager.AddDataSource("Player/Inventory/Items/0", inventory.Items[0]);
        //    sourcesDropdown.options = new List<TMP_Dropdown.OptionData>();
        //    foreach (string source in DataBindManager.DataSources.Keys)
        //    {
        //        sourcesDropdown.options.Add(new TMP_Dropdown.OptionData(source));
        //    }
        //    foreach (DataBindInfo dataBind in DataBindManager.DataBinds[DataBindType.Text].Values)
        //    {
        //        bindsDropdown.options.Add(new TMP_Dropdown.OptionData(dataBind.SubmenuPath));
        //    }
        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
        public GameObject dropdownPrefab; // Reference to a Dropdown prefab
        public Transform canvasTransform; // Reference to the Canvas transform

        void Start()
        {
            List<string> paths = new List<string> { "Player/Inventory/Item", "Player/Inventory/Weapon", "Player/Stats/Health", "Player/Stats/Attack", "Player/Stats/Defense" };
            GenerateDropdownMenu(paths);
        }

        void GenerateDropdownMenu(List<string> paths)
        {
            Dictionary<string, TMP_Dropdown> dropdowns = new Dictionary<string, TMP_Dropdown>();

            foreach (string path in paths)
            {
                string[] parts = path.Split('/');
                Transform parentTransform = canvasTransform;

                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i];
                    if (!dropdowns.ContainsKey(part))
                    {
                        GameObject dropdownObj = Instantiate(dropdownPrefab, parentTransform);
                        TMP_Dropdown dropdown = dropdownObj.GetComponent<TMP_Dropdown>();
                        dropdowns.Add(part, dropdown);

                        dropdown.options.Add(new TMP_Dropdown.OptionData(part));
                        dropdown.onValueChanged.AddListener(delegate { SubmenuValueChanged(dropdown); });
                    }

                    parentTransform = dropdowns[part].transform;
                }
            }
        }

        void SubmenuValueChanged(TMP_Dropdown change)
        {
            // Add functionality for submenu items here
        }

    }
}
