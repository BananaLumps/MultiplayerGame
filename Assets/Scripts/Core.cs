using Base.Client.Player;
using Base.Inventory;
using Base.Skills;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
namespace Base
{

    public class Core : MonoBehaviour
    {
        static Core _instance = null;
        //Items by ID
        static Dictionary<string, ItemObject> _items = new Dictionary<string, ItemObject>();
        //Icons by Item ID
        Dictionary<string, Texture2D> _icons = new Dictionary<string, Texture2D>();
        //Skills by ID
        Dictionary<string, SkillBase> _skills = new Dictionary<string, SkillBase>();
        public GameObject PlayerPrefab;
        public Player LocalPlayer;
        public Base.Server.Server Server;

        public static Core Instance
        {
            get
            {
                return _instance;
            }
        }
        public static Dictionary<string, ItemObject> Items => _items;
        public Dictionary<string, Texture2D> Icons => _icons;
        public Dictionary<string, SkillBase> Skills => _skills;

        // Start is called before the first frame update
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            //Commented out for UI Testing, Need to add back in after testing
            // PlayerPrefab = GameObject.Find("Player");
            //  Server = GameObject.Find("NetworkScripts").GetComponent<Base.Server.Server>();
            foreach (ItemObject i in AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/Items/itemstone.asset"))
            {
                Items.Add(i.ID, i);
            }

        }
        private void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            _icons.Clear();
            _items.Clear();
        }
    }
}