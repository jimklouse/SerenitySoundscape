using Dapper;
using Google.Protobuf.WellKnownTypes;
using SerenitySoundscape.Models;
using System.Data;

namespace SerenitySoundscape
{
    public class SoundRepo : ISoundRepo
    {
        private readonly IDbConnection _connection;

        public SoundRepo(IDbConnection connection)
        {
            _connection = connection;
        }

        public List<Sound> AllSounds()
        {
            return _connection.Query<Sound>("SELECT * FROM sounds;").ToList();
        }

        public List<Sound> GetSoundsByID(List<int> selectedSoundIDs)
        {
            var selectedSounds = new List<Sound>();
            foreach (int soundID in selectedSoundIDs)
            {
                var sound = _connection.QuerySingle<Sound>("SELECT * FROM sounds WHERE SoundID = @soundID;",
                    new { SoundID = soundID });

                selectedSounds.Add(sound);
            }
            return selectedSounds;
        }

        public void AddMixToDatabase(Mix newMix)
        {
            var name = newMix.Name;

            _connection.Execute("INSERT INTO mix (NAME) VALUES (@name);",
                new { name = newMix.Name });

            int mixID = _connection.QuerySingle<int>("SELECT * FROM mix WHERE Name = @name;",
                new { Name = name });

            foreach (var soundID in newMix.SelectedSoundIDs)
            {

                _connection.Execute("INSERT INTO mixsound (MixID, SoundID) VALUES (@MixID, @SoundID);",
                    new { MixID = mixID, SoundID = soundID });
            }
        }

        public List<Mix> GetMixList()
        {
            var namedMixes = new List<Mix>();
            var allMixes = _connection.Query<Mix>("SELECT * From mix;").ToList();

            foreach (var mix in allMixes)
            {
                if (!string.IsNullOrEmpty(mix.Name))
                {
                    mix.SelectedSoundIDs = _connection.Query<int>("SELECT SoundID FROM mixsound WHERE MixID = @MixIDParam;",
                        new { MixIDParam = mix.MixID }).ToList();

                    mix.SelectedSounds = _connection.Query<Sound>("SELECT * FROM sounds WHERE SoundID IN @SoundIDs;",
                        new { SoundIDs = mix.SelectedSoundIDs }).ToList();

                    namedMixes.Add(mix);

                    foreach (var namedMix in namedMixes)
                    {
                        namedMix.SelectedSounds.OrderBy(sound => sound.Category).ToList();
                    }
                }
            }
            return namedMixes;
        }

        public Mix GetMixByID(int id)
        {
            var mixID = id;
            var mix = _connection.QuerySingle<Mix>("SELECT * FROM mix WHERE MixID = @mixID;",
                new { MixID = mixID });

            mix.SelectedSoundIDs = _connection.Query<int>("SELECT SoundID FROM mixsound WHERE MixID = @mixID;",
                new { MixID = mixID }).ToList();

            var allSounds = AllSounds();
            var selectedSounds = new List<Sound>();
            var notSelectedSounds = new List<Sound>();

            foreach (var sound in allSounds)
            {
                sound.IsSelected = false;

                foreach (var selectedSoundID in mix.SelectedSoundIDs)
                {
                    if (sound.SoundID == selectedSoundID)
                    {
                        sound.IsSelected = true;
                        selectedSounds.Add(sound);
                    }
                }
            }
            foreach (var sound in allSounds)
            {
                if (!sound.IsSelected)
                {
                    notSelectedSounds.Add(sound);
                }
            }
            mix.SelectedSounds = selectedSounds.OrderBy(sound => sound.Category).ToList();
            mix.NotSelectedSounds = notSelectedSounds.OrderBy(sound => sound.Category).ToList();

            return mix;
        }

        public void UpdateMix(Mix mix)
        {
            var name = mix.Name;
            var mixID = mix.MixID;

            _connection.Execute("UPDATE mix SET Name = @name WHERE MixID = @mixID",
                new { MixID = mixID, Name = name });

            _connection.Execute("DELETE FROM mixsound WHERE MixID = @mixID;",
                new { MixID = mixID });

            foreach (var soundID in mix.NewSoundIDs)
            {
                _connection.Execute("INSERT INTO mixsound (MixID, SoundID) VALUES (@mixID, @soundID);",
                    new { MixID = mixID, SoundID = soundID });
            }

        }

        public void DeleteMix(int id)
        {
            var mixID = id;
            _connection.Execute("DELETE FROM mixsound WHERE MixID = @mixID",
                new { MixID = mixID });
            _connection.Execute("DELETE FROM mix WHERE MixID = @mixID;",
                new { MixID = mixID });
        }

    }
}
