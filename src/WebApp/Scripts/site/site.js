/// <reference path="~/Scripts/jquery-3.0.0.js" />

function convertFileSize(fileSize) {
    var kb = 1024;
    var mb = 1024 * 1024;
    var gb = 1024 * 1024 * 1024;
    if (fileSize < kb) {
        return fileSize + " Bytes";
    }
    else if (fileSize < mb) {
        return Math.round(fileSize / kb) + " KB";
    }
    else if (fileSize < gb) {
        return Math.round((fileSize / mb) * 100) / 100 + " MB";
    }
    else {
        return Math.round((fileSize / gb) * 100) / 100 + " GB";
    }
}
