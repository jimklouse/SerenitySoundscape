using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.CRUD;
using SerenitySoundscape.Models;
using System.Diagnostics;

namespace SerenitySoundscape.Controllers
{
    public class MixController : Controller
    {

        public MixController(ISoundRepo soundAccess)
        {
            _soundAccess = soundAccess;
        }

        private readonly ISoundRepo _soundAccess;

        public IActionResult Index()
        {
            var allSounds = _soundAccess.AllSounds();

            return View(allSounds);
        }

        public IActionResult CreateMix(List<int> selectedSoundIDs)
        {
            var selectedSounds = _soundAccess.GetSoundsByID(selectedSoundIDs);
            var newMix = new Mix(selectedSounds);

            var viewModel = new Mixer(selectedSounds, newMix);

            return View(viewModel);
        }

        public IActionResult AddMixToDatabase(Mix newMix)
        {
            _soundAccess.AddMixToDatabase(newMix);

            return RedirectToAction("MySoundscape");
        }

        public IActionResult MySoundscape()
        {
            var getMixList = _soundAccess.GetMixList();

            return View(getMixList);
        }
        public IActionResult PlayMix(int id)
        {
            var getMixByID = _soundAccess.GetMixByID(id);

            return View(getMixByID);
        }

        public IActionResult UpdateMix(int id)
        {
            var getMixByID = _soundAccess.GetMixByID(id);

            if (getMixByID == null)
            {
                return View("MixNotFound");
            }

            return View(getMixByID);
        }

        public IActionResult UpdateMixInDatabase(Mix mix)
        {
            _soundAccess.UpdateMix(mix);

            return RedirectToAction("MySoundscape");
        }

        public IActionResult DeleteMix(int id)
        {
            _soundAccess.DeleteMix(id);

            return RedirectToAction("MySoundscape");
        }

    }
}