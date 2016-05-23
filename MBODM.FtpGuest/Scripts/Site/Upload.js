/// <reference path="~/Scripts/jquery-2.2.3.min.js" />

$(document).ready(function () {
    $("#file").change(function () {
        var file = $("#file")[0].files[0];
        if (file) {
            $("#fileName").html("<span><b>Name: </b></span><span>" + file.name + "</span>");
            $("#fileSize").html("<span><b>Größe: </b></span><span>" + convertFileSize(file.size) + "</span>");
            $("#statusText").text("");
            $("#progressBar").hide();
        }
    });
    $("#button").click(function () {
        var file = $("#file")[0].files[0];
        if (file) {
            uploadAndTransfer(file.size);
        }
    });
});

function uploadAndTransfer(fileSize) {
    var allBytes = 0;
    var ftpBytes = 0;
    var uploadBytes = 0;
    var previousText = "";
    var xhr = new XMLHttpRequest();
    xhr.upload.onprogress = function (e) {
        if (e.lengthComputable) {
            $("#file").attr("disabled", true);
            allBytes = e.total + fileSize;
            uploadBytes = e.loaded;
            var percent = (uploadBytes / allBytes) * 100;
            setProgressBar(percent);
        }
    };
    xhr.upload.onload = function () {
        // Nothing
    };
    xhr.upload.onerror = function () {
        uploadAndTransferEnd("Ein Upload-Fehler ist aufgetreten.");
    };
    xhr.onprogress = function () {
        if (xhr.responseText) {
            var actualText = xhr.responseText.substring(previousText.length);
            previousText = xhr.responseText;
            var parts = actualText.split("D");
            parts.forEach(function (part) {
                if (part != "" && part != "D" && part != "E") {
                    ftpBytes += parseInt(part);
                }
            });
            var percent = ((uploadBytes + ftpBytes) / allBytes) * 100;
            setProgressBar(percent);
        }
    };
    xhr.onload = function () {
        uploadAndTransferEnd(xhr.responseText.split("Error:")[1]);
    };
    xhr.onerror = function () {
        uploadAndTransferEnd("Ein FTP-Fehler ist aufgetreten.");
    };
    uploadAndTransferStart();
    xhr.open("POST", "Upload/UploadFile", true);
    xhr.setRequestHeader("Cache-Control", "no-cache");
    xhr.send(new FormData($("#form")[0]));
}

function uploadAndTransferStart() {
    $("#statusText").text("Hochladen...");
    setProgressBar(0);
    $("#progressBar").show();
    $("#button").attr("disabled", true);
}

function uploadAndTransferEnd(error) {
    $("#file").attr("disabled", false);
    $("#button").attr("disabled", false);
    if (error) {
        $("#progressBar").hide();
        $("#statusText").text(error.trim());
    }
    else {
        $("#statusText").text("Die Datei wurde erfolgreich hochgeladen.");
    }
}

function setProgressBar(percent) {
    var innerWidth = (percent * $("#progressBar").width()) / 100;
    $("#progressBarInnerBlock").width(innerWidth);
}
