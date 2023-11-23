document.addEventListener("DOMContentLoaded", function () {
    var audio = document.getElementById('audio');

    if (audio) {
        audio.onplay = function () {
            audio.volume = 0.10;
        };
    }
});

function redirectToUpdateMix() {
    var updateMixUrl = '/Mix/UpdateMix/';
}
