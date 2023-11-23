using System;
using System.Collections.Generic;
using SerenitySoundscape.Models;

namespace SerenitySoundscape
{
    public interface ISoundRepo
    {
        public List<Sound> AllSounds();
        public List<Sound> GetSoundsByID(List<int> selectedSoundIDs);
        public void AddMixToDatabase(Mix newMix);
        public List<Mix> GetMixList();
        public Mix GetMixByID(int mixID);
        public void UpdateMix(Mix mix);
        public void DeleteMix(int id);
    }
}