

function commonSubmit(url, reUrl, formId, btnId, rules, preSubmit) {
    return callBackSubmit(url, formId, btnId, rules, function (result) {
        if (result.code != 1) {
            //错误
            showErrorMsg(result.msg);
            return;
        }
        window.location = reUrl;
    }, preSubmit);

};

function callBackSubmit(url, formId, btnId, rules, callBack, preSubmit) {
    var validator = initValidate(formId, rules);
    var btnSubmit = $('#' + btnId).ladda();

    btnSubmit.on("click", '', function (e) {
        var form = $('#' + formId);
        if (!form.valid()) {
            validator.focusInvalid();
            return false;
        }

        if (preSubmit) {
            var pre = preSubmit();
            if (!pre) {
                return false;
            }
        }
        // Start loading
        btnSubmit.ladda('start');
        $.ajax({
            url: url,
            type: "Post",
            data: form.serialize(),
            success: function (result) {
                btnSubmit.ladda('stop');
                callBack(result);

            },
        });
        e.preventDefault();
    });

    return validator;
}

function callBackSubmitNotClick(url, formId, btnId, rules, callBack) {
    var validator = initValidate(formId, rules);
    var btnSubmit = $('#' + btnId).ladda();


    var form = $('#' + formId);
    if (!form.valid()) {
        validator.focusInvalid();
        return false;
    }

    // Start loading
    btnSubmit.ladda('start');
    $.ajax({
        url: url,
        type: "Post",
        data: form.serialize(),
        success: function (result) {
            btnSubmit.ladda('stop');
            callBack(result);

        },
    });
    return validator;
}

function initValidate(formId, rules) {
    var validator = $("#" + formId).validate({
        rules: rules,
        errorElement: "em",
        errorPlacement: function (error, element) {
            // Add the `help-block` class to the error element
            error.addClass("help-block");

            // Add `has-feedback` class to the parent div.form-group
            // in order to add icons to inputs
            element.parent().addClass("has-feedback");

            if (element.prop("type") === "checkbox") {
                error.insertAfter(element.parent("label"));
            } else {
                error.insertAfter(element);
            }

            // Add the span element, if doesn't exists, and apply the icon classes to it.
            if (!element.next("span")[0]) {
                $("<span class='glyphicon glyphicon-remove form-control-feedback'></span>").insertAfter(element);
            }
        },
        success: function (label, element) {
            // Add the span element, if doesn't exists, and apply the icon classes to it.
            if (!$(element).next("span")[0]) {
                $("<span class='glyphicon glyphicon-ok form-control-feedback'></span>").insertAfter($(element));
            }
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parent().addClass("has-error").removeClass("has-success");
            $(element).next("span").addClass("glyphicon-remove").removeClass("glyphicon-ok");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parent().addClass("has-success").removeClass("has-error");
            $(element).next("span").addClass("glyphicon-ok").removeClass("glyphicon-remove");
        },
        ignore: ":hidden:not(#summernote),.note-editable.panel-body"
    });

    return validator;
};