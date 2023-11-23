﻿namespace SerenitySoundscape.Models
{
    public class Sound
    {
        public Sound()
        {

        }

        public int SoundID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FilePath { get; set; }
        public string ImagePath { get; set; }
        public bool IsSelected { get; set; }

    }
}
