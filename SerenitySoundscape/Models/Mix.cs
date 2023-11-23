using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace SerenitySoundscape.Models
{
    public class Mix
    {
        public Mix()
        {

        }

        public Mix(List<Sound> selectedSounds)
        {
            SelectedSounds = selectedSounds;
        }

        public int MixID { get; set; }
        public string Name { get; set; }
        public List<int> SelectedSoundIDs { get; set; }
        public List<string> SelectedSoundNames { get; set; }
        public List<Sound> SelectedSounds { get; set; }
        public List<Sound> NotSelectedSounds { get; set; }
        public List<int> NewSoundIDs { get; set; }

    }
}
