mergeInto(LibraryManager.library, {

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
    }
});
