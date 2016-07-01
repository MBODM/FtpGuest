/// <reference path="~/Scripts/jquery-3.0.0.js" />

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
            uploadFile(file.size);
        }
    });
});

function uploadFile(fileSize) {
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
        uploadEnd("Ein Upload-Fehler ist aufgetreten.", xhr);
    };
    xhr.onprogress = function () {
        if (xhr.responseText) {
            var actualText = xhr.responseText.substring(previousText.length);
            previousText = xhr.responseText;
            var parts = actualText.split("D");
            parts.forEach(function (part) {
                switch (part) {
                    case "":
                    case "D":
                    case "F":
                        // Nothing
                        break;
                    case "E":
                        uploadEnd("Ein Controller-Fehler ist aufgetreten.", xhr);
                        break;
                    default:
                        ftpBytes += parseInt(part);
                        break;
                }
            });
            var percent = ((uploadBytes + ftpBytes) / allBytes) * 100;
            setProgressBar(percent);
        }
    };
    xhr.onload = function () {
        uploadEnd(null, xhr);
    };
    xhr.onerror = function () {
        uploadEnd("Ein FTP-Fehler ist aufgetreten.", xhr);
    };
    uploadStart();
    xhr.open("POST", "Upload/UploadFile", true);
    xhr.setRequestHeader("Cache-Control", "no-cache");
    xhr.send(new FormData($("#form")[0]));
}

function uploadStart() {
    $("#statusText").css("color", "dimgray");
    $("#statusText").text("Hochladen...");
    setProgressBar(0);
    $("#progressBar").show();
    $("#button").attr("disabled", true);
}

function uploadEnd(error, xhr) {
    $("#file").attr("disabled", false);
    $("#button").attr("disabled", false);
    xhr.upload.onprogress = null;
    xhr.upload.onload = null;
    xhr.upload.onerror = null;
    xhr.onprogress = null;
    xhr.onload = null;
    xhr.onerror = null;
    if (error) {
        $("#progressBar").hide();
        $("#statusText").css("color", "red");
        $("#statusText").text(error);
    }
    else {
        $("#statusText").css("color", "dimgray");
        $("#statusText").text("Die Datei wurde erfolgreich hochgeladen.");
    }
}

function setProgressBar(percent) {
    var innerWidth = (percent * $("#progressBar").width()) / 100;
    $("#progressBarInnerBlock").width(innerWidth);
}
