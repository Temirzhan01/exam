Ниже это содержание PartialView которых может быть несколько внутри View проекта типа ASP.NET MVC 

<div class="col-md-12 table object" style="border: 1px solid #4F7199; padding-bottom:10px;">
    <div class="row">
        <div class="col-md-12 FiNumber">ФИ - @Model.BaseClaFinInstrument[Model.index].ClaApprovedConditionInfo.CreditLineNumber@Model.BaseClaFinInstrument[Model.index].ClaApprovedConditionInfo.TranshNumber@Model.BaseClaFinInstrument[Model.index].ClaApprovedConditionInfo.OBZNumber@Model.BaseClaFinInstrument[Model.index].ClaApprovedConditionInfo.BTGNumber</div> // содержание этого нужно 
    </div>

    <div class="col-md-2 pull-right">
        <input type="button" value="Удалить ФИ" class="btn-xs DeleteStartGesvObject btn-danger" />
        @if (Model.BaseClaFinInstrument[Model.index].FinInstrumentKind == "CL" && string.IsNullOrEmpty(Model.BaseClaFinInstrument[Model.index].ParentCL))
        {
        <input type="button" value="Добавить подлимит" class="btn-xs AddUnderLimitFinIns btn-primary" /> //при нажатии этой кнопки, необходимо получить содержание .FiNumber
        }
    </div>
</div>


    $(document).on("click", ".AddUnderLimitFinIns", function () {
        $("#BaseClaCommisAccInfo_CommisAccNum").attr("disabled", "disabled");
        $(".CommisAmount").attr("disabled", "disabled");

        if ($("#DataCollection").valid()) {
            var target = $(this);
            var data = new Object();
            data.value = target.closest(".FiNumber").val();

            target.closest("#StartFinInsturment").find(".table").removeAttr("hidden");
            target.closest("#StartFinInsturment").find(".IsStateProgram").prop("checked", false);
            target.removeClass("margin-top-10");
            target.closest(".clearfix").find(".StateProgramm").attr("hidden", "hidden");

            $.get("/" + base_url + "/Base/AddFinInstrumentKMCLUnderLimit", data, function (partial) {
                $("#BaseClaCommisAccInfo_CommisAccNum").removeAttr("disabled");
                $(".CommisAmount").removeAttr("disabled");
                $(document).find("#GesvObjects").append(partial);
            });
        }
        else {
            alert("Заполните пожалуйста все обязательные поля и все условия!!!");
            $("#BaseClaCommisAccInfo_CommisAccNum").removeAttr("disabled");
            $(".CommisAmount").removeAttr("disabled");
        }
    });
