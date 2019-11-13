function LimitText(textBoxID, limit, isLimit) {
    var maxwords = limit;
    w = document.getElementById(textBoxID);
    x = document.getElementById('show_remaining_words' + textBoxID);
    
    var CusHeight = limit + 40;

    if (limit == 0)
        CusHeight = 50 + 40;
    
    if (isLimit == "True") {
        $("#" + textBoxID + "").attr("style", "height: " + CusHeight + "px; font-size: 14px !important;");
        w.value = w.value.replace("	", " ").replace("	", " ");
        wordList = w.value.replace(/\n/g, " ").split(" ");

        var wordContent = "";
        var r = 0;
        for (var i = 0, len = wordList.length; i < len; i++) {
            if (/^[a-zA-Z0-9]/.test(wordList[i]))
                r++;

            wordContent += wordList[i] + ' ';

            if (r == maxwords)
                break;
        }

        w.value = w.value.substr(0, wordContent.length);
        x.innerHTML = maxwords - r;
    }
    else {
        $("#" + textBoxID + "").attr("style", "font-size: 14px !important;");
    }
}


function PreviewImagesEntryForm(TextBox, IsPreview, ImageDetail) {
    View = TextBox + "_View";
    Edit = TextBox + "_Edit";
    try{
        IDView = document.getElementById(View);
        IDEdit = document.getElementById(Edit);

        $(IDView).empty();
        $(IDView).append(ImageDetail);

        if (IsPreview == "True") {
            IDEdit.classList.add("DisableControl");
            //IDView.classList.remove("DisableControl");
        }
        else {
            IDEdit.classList.remove("DisableControl");
            //IDView.classList.add("DisableControl");
        }
    }
    catch(err){}
}

function myFunction(lblDelete, lblmax, btnUpload) {
    //console.log("Test");
}

function EditEntryFormFileUpload(lblDelete, lblmax, btnUpload, btnEdit, lblPrev, btnCancel, isEnable) {

    IDPrev = document.getElementById(lblPrev);

    IDcancel = document.getElementById(btnCancel);

    IDDelete = document.getElementById(lblDelete);
    IDEdit = document.getElementById(btnEdit);

    IDmax = document.getElementById(lblmax);
    IDUpload = document.getElementById(btnUpload);
    //console.log(isEnable);
    if (!isEnable) {
        IDmax.classList.remove('DisableControl');
        IDUpload.classList.remove("DisableControl");
        IDcancel.classList.remove("DisableControl");

        IDPrev.classList.add("DisableControl");
        IDDelete.classList.add("DisableControl");
        IDEdit.classList.add("DisableControl");
    }
    else {
        IDcancel.classList.add("DisableControl");
        IDmax.classList.add('DisableControl');
        IDUpload.classList.add("DisableControl");

        IDPrev.classList.remove("DisableControl");
        IDDelete.classList.remove("DisableControl");
        IDEdit.classList.remove("DisableControl");
    }
}

function LabelMsg(textBoxID, Msg) {
    var ID = "#" + textBoxID;
    $(ID).text(Msg);
}
