namespace SerenitySoundscape.Models
{
    public class Mixer
    {
        public Mixer()
        {

        }
        public Mixer(List<Sound> selectedSounds, Mix newMix)
        {
            SelectedSounds = selectedSounds;
            NewMix = newMix;
        }

        public List<Sound> SelectedSounds { get; set; }
        public Mix NewMix { get; set; }
    }
}
