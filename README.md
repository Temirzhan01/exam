                @for (int i = 0; i < Model.BaseClaFinInstrument.Count; i++)
                {
                    <div class="ClaGesv">
                        @Html.Partial("ClaGesv", new CLAMB.ViewModels.ClaFinInstrumentInfoViewModel
                           {
                               BaseClaFinInstrument = Model.BaseClaFinInstrument,
                               index = i,
                               BaseClaCommisAccInfo = Model.BaseClaCommisAccInfo
                           })
                    </div>
                }


$(document).on("click", "#SaveStateProgram", function () {
        var data = new Object();
        var target = $(this)
        data.fiId = target.closest(".ClaGesvObject").index();
        data.stateProgramType = target.closest(".ClaGesvObject").find(".StateProgram_StateProgramType").val();
        data.stateSupportType = target.closest(".ClaGesvObject").find(".StateProgram_StateSupportType").val();

        target.closest(".ClaGesvObject").find(".StateProgram").attr("hidden", "hidden");
        target.closest(".ClaGesvObject").find(".IsStateProgram").prop("checked", false);

        $.get("/" + base_url + "/Base/SaveStateProgram", data, function (partial) {
            target.closest(".ClaGesv").html(partial);
            target.closest("#StartFinInsturment").find(".table").removeAttr("hidden");
        });
    });
