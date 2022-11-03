namespace AzDoMVCApp.Models
{

    [Serializable]
    public class Wave
    {

        public string WaveValue { get; set; }

        public Wave()
        {
            WavesList = new List<ItemList>();
        }
            
        public void PopulateWaves()
        {
            //Fill up waves 
            WavesList = new List<ItemList>()
            {
                new ItemList { Text = "Wave 1", Value = "Wave1" },
                new ItemList { Text = "Wave 2", Value = "Wave2" },
                new ItemList { Text = "Wave 3", Value = "Wave3" },
                new ItemList { Text = "Wave 4", Value = "Wave4" },
                new ItemList { Text = "Wave 5", Value = "Wave5" }
            };
        }

         public List<ItemList> WavesList { get; set; }


    }

    [Serializable]
    public class ItemList 
    {

        public string Text { get; set; }
        public string Value { get; set; }

        public bool Selected { get; set; }
    }
}
