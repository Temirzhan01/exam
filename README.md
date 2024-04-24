@model CLAMB.ViewModels.BaseViewModel
@{
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_CredManager.cshtml";
}

@Html.Partial("ClaStart", new CLAMB.ViewModels.ClaStartDataViewModel
{
    BaseClaClientInfo = Model.BaseClaClientInfo,
    BaseClaStartData = Model.BaseClaStartData,
    BaseClaRegForm = Model.BaseClaRegForm,
    BaseClaFinInstrument = Model.BaseClaFinInstrument,
    LeadNumber = Model.LeadNumber,
    BaseClaCommisAccInfo = Model.BaseClaCommisAccInfo,
    BaseClaContacts = Model.BaseClaContacts
})

есть у меня тут вью, в котором вызывается паршал вью.
Внутри данного паршал вью ClaStart, есть кнопка при нажатии которого выполняется скрипт. 

    $(document).on("click", ".DeleteFinInstrument", function () {
        var target = $(this);
        var data = new Object();
        data.id = target.parents("._Partial").index();
        var index = data.id;
        $.get("/" + base_url + "/Base/DeleteFinInstrument", data, function (partial) {
            $(".paginator").find("div:eq(" + index + ")").remove();
            target.parents("._Partial").remove();
            $("._Partial").hide();
            $("._Partial").last().show();
            $(".paginator").find("input").removeClass("btn-default").addClass("btn-primary");
            $(".paginator").find("input").last().removeClass("btn-primary").addClass("btn-default");
        });
    });

    DeleteFinInstrument этот метод выполняется и возвращает паршал вью  ClaStart  
    public PartialViewResult DeleteFinInstrument(string id)
        {
            BaseViewModel bv = _BaseViewModel;
            var cl = bv.BaseClaFinInstrument[int.Parse(id)].ClaApprovedConditionInfo.CreditLineNumber;
            bv.BaseClaFinInstrument.RemoveAll(fi => fi.ParentCL == cl);
            bv.BaseClaFinInstrument.RemoveAt(int.Parse(id));
            _BaseViewModel = bv;
            return PartialView("ClaStart", new ClaStartDataViewModel()
            {
                BaseClaClientInfo = bv.BaseClaClientInfo,
                BaseClaStartData = bv.BaseClaStartData,
                BaseClaRegForm = bv.BaseClaRegForm,
                BaseClaFinInstrument = bv.BaseClaFinInstrument,
                LeadNumber = bv.LeadNumber,
                BaseClaCommisAccInfo = bv.BaseClaCommisAccInfo,
                BaseClaContacts = bv.BaseClaContacts
            });
        }
Вопрос, как мне тогда этот полученный паршал вью обновит? через скрипт или как? 
