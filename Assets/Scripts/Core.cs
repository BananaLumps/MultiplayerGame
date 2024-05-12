using System.Collections.Generic;
using UnityEngine;
namespace Base
{

    public class Core : MonoBehaviour
    {
        static Core _instance = null;
        Dictionary<string, Item> _items = new Dictionary<string, Item>();
        Dictionary<string, Texture2D> _icons = new Dictionary<string, Texture2D>();

        public static Core Instance
        {
            get
            {
                return _instance;
            }
        }
        public Dictionary<string, Item> Items => _items;
        public Dictionary<string, Texture2D> Icons => _icons;



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
        }
        private void Start()
        {
            //Load Items and Icons
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}