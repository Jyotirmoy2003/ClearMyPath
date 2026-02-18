mergeInto(LibraryManager.library, {
     FetchLeaderboard: function () {
        FetchLeaderboardData();
    },
    UploadSession: function (jsonPtr) {
        var jsonString = UTF8ToString(jsonPtr);
        UploadSessionToFirebase(jsonString);
    },

    CopyToClipboard: function (textPtr) {
        var text = UTF8ToString(textPtr);
        navigator.clipboard.writeText(text).then(function () {
            console.log("Copied to clipboard:", text);
        }).catch(function (err) {
            console.error("Clipboard copy failed:", err);
        });
    },

    ExitFullscreen: function () {
        if (document.fullscreenElement) {
            document.exitFullscreen();
        }
    },

    ReloadPage: function () {
        window.location.reload();
    }

});
