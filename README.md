@model SBP.ViewModels.FinInstrumentInfoViewModel

@{
    ViewBag.Title = "Принятое решение по заявке";
    var checkedv = (Model.BaseMsbRequest.KKFisChecked || Model.BaseMsbRequest.KKFSisChecked) ? "checked" : "";
    var displayv = (Model.BaseMsbRequest.KKFisChecked || Model.BaseMsbRequest.KKFSisChecked) ? "display:block" : "display:none";
    var hideTerm = (Model.RequestedConditions[Model.index].FIType == "Возобновляемая кредитная линия" || Model.RequestedConditions[Model.index].FIType == "Невозобновляемая кредитная линия") ? "block" : "hidden";
}

<div class="col-md-12 req object">
    @Html.HiddenFor(x => x.index, new { @class = "requestedIndex" })
    <div class="widget">
        <!-- Widget title -->
        <div class="widget-head">
            <div class="pull-left"><i class="fa fa-table"></i> Запрашиваемые условия (@Model.RequestedConditions[Model.index].DuplicationType)</div>
            <div class="widget-icons pull-right">
                <a href="#" class="wminimize"><i class="fa fa-chevron-up"></i></a>
            </div>
            <div class="clearfix"></div>
        </div>
        @Html.ValidationSummary(true)
        @if (Model.RequestedConditions[Model.index].DuplicationType == "Новое финансирование")
        {
            <div class="widget-content referrer">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-md-12">
                            @if (Model.index == 0)
                            {
                                <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[0].Borrower, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                            }
                        </div>
                    </div>
                    <div class="col-md-6">@Html.HiddenFor(m => Model.RequestedConditions[Model.index].ClientsCount)</div>
                    <hr />
                    @Html.Partial("Clientsall", new SBP.ViewModels.FinInstrumentInfoViewModel()
                    {
                         RequestedConditions = Model.RequestedConditions,
                         AvailabilityPeriodProperties = Model.AvailabilityPeriodProperties,
                         TranshButtonProperties = Model.TranshButtonProperties,
                         index = Model.index,
                    })
                    <hr />
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].QuestionEssence, "", Model.QuestionEssenceProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                    @*@<div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].CoBorrower, "", Model.QuestionEssenceProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                        <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].MortgageCoBorrower, "", Model.QuestionEssenceProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>*@
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].CollateralDeal, "", Model.CollateralDealProperties.readOnly, "", "", new { @style = " resize:vertical" })</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].BorrowerInfo, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].CoBorrowerInfo, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].Deferral, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].StateSupportType, new SBP.ViewModels.Directory().StateSupportTypeList, "", Model.StateSupportProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].StateProgramType, new SBP.ViewModels.Directory().GetReferencesByName("state"), "", Model.StateProgramTypeProperties.readOnly, "")</div>
                    @if (Model.CreditProgramProperties.readOnly)
                    {
                        <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].CreditProgram, "list-alt", Model.CreditProgramProperties.readOnly)</div>
                    }
                    else
                    {
                        <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].CreditProgramID, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.V_program_TYPE"), "", Model.CreditProgramProperties.readOnly, "")</div>
                    }

                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].FIType, new SBP.ViewModels.Directory().FITypes, "", Model.FITypeProperties.readOnly, "FIType")</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].TargetAppointment, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].FISum, "list-alt", Model.FISumPropertiesProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].FITerm, "list-alt", Model.FITermPropertiesProperties.readOnly)</div>
                    <div class="col-md-6" @hideTerm>@Html.HtmlText(m => Model.RequestedConditions[Model.index].TranshmaxTerm, "list-alt", Model.FITermPropertiesProperties.readOnly, "", "TranshmaxTerm")</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].PercentageRate, "list-alt", Model.PercentageRateProperties.readOnly, "", "PercentageRate")</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].AvailabilityPeriod, "list-alt", Model.AvailabilityPeriodProperties.readOnly, "", "AvailabilityPeriod")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].Currency, new SBP.ViewModels.Directory().GetReferencesByName("curr"), "", Model.RedemptionMethodProperties.readOnly, "")</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].GracePeriod, "list-alt", Model.AvailabilityPeriodProperties.readOnly, "", "GracePeriod")</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].RemunerationGracePeriod, "list-alt", Model.AvailabilityPeriodProperties.readOnly, "", "RemunerationGracePeriod")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].OrderOfRedemption, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.v_ref_cred_repay_rew_rate_bs"), "", Model.OrderOfRedemptionProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].MainDebtRedemption, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.v_ref_cred_main_repay_rate_bs"), "", Model.OrderOfRedemptionProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].RedemptionMethod, new SBP.ViewModels.Directory().GetReferencesByName("repay"), "", Model.RedemptionMethodProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    @*<div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].OverflowIndication, "list-alt", Model.OverflowIndicationProperties.readOnly)</div>*@
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].OverflowIndication, new SBP.ViewModels.Directory().OverflowIndicationList, "", Model.SourseOfCreditProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].SourseOfCredit, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.v_ref_loan_source"), "", Model.SourseOfCreditProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].TargetGroup, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].ProjectImplementationPlace, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.v_bussiness_place"), "", Model.RedemptionMethodProperties.readOnly, "")</div>
                    @*<div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].SpecConditionsBefore, "", Model.SpecConditionsBeforeProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>*@
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].WayOfMastering, "", Model.WayOfMasteringProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                    @*<div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].SpecConditionsAfter, "", Model.SpecConditionsAfterProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                        <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].Classification, new SBP.ViewModels.Directory().GetReferencesByName("class"), "", Model.RedemptionMethodProperties.readOnly, "")</div>
                        <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].PeriodExecution, "list-alt", Model.RedemptionMethodProperties.readOnly, "", "PeriodExecution")</div>*@
                    @if (Model.RedemptionMethodProperties.readOnly)
                    {
                        <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].CreditIndustry, new SBP.ViewModels.Directory().GetReferenceValueList2("VIEW_UCLA.v_otrasl_oked"), "", Model.RedemptionMethodProperties.readOnly, "")</div>
                    }
                    else
                    {
                        <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].CreditIndustry, new SBP.ViewModels.Directory().GetReferenceValueList2("VIEW_UCLA.v_otrasl_oked"), "", Model.RedemptionMethodProperties.readOnly, "autocomplete-list")</div>
                    }
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].CreditIndustryCode, "", Model.RedemptionMethodProperties.readOnly, "", "industrycode")</div>
                    <div class="col-md-6">
                        @Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].Commissions, "", Model.QuestionEssenceProperties.readOnly, "", "", new { @style = "height:auto" })
                        @if (!Model.QuestionEssenceProperties.readOnly)
                        {
                            <span>@Html.CheckBoxFor(m => Model.RequestedConditions[Model.index].IsCommissionsRequested, new { id = "RequestedConditions_" + Model.index + "__IsCommissionsRequested" }) требуется</span>
                        }
                        else
                        {
                            <span>@Html.CheckBoxFor(m => Model.RequestedConditions[Model.index].IsCommissionsRequested, new { id = "RequestedConditions_" + Model.index + "__IsCommissionsRequested", disabled = true }) требуется</span>
                        }
                    </div>
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].Fines, "", Model.QuestionEssenceProperties.readOnly, "", "", new { @style = "height:auto" })</div>
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].Km_Comment, "", Model.Km_CommentProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].GreenProject, new SBP.ViewModels.Directory().GreenProject, "", Model.GreenProjectProperties.readOnly, "GreenProject")</div>    @*Зеленый проект*@@*Зеленый проект*@
                    <div class="col-md-6">@Html.HiddenFor(m => Model.RequestedConditions[Model.index].TranshCount)</div>
                    <div class="col-md-12">
                        <div class="_transh">
                            @Html.Partial("TranshInfoData", new SBP.ViewModels.FinInstrumentInfoViewModel()
                            {
                                RequestedConditions = Model.RequestedConditions,
                                AvailabilityPeriodProperties = Model.AvailabilityPeriodProperties,
                                TranshButtonProperties = Model.TranshButtonProperties,
                                index = Model.index,
                            })
                        </div>
                        @if (!Model.TranshButtonProperties.readOnly)
                        {
                            <div style="margin-left: 30px"><button type="button" class="btn btn-sm btn-primary AddTranshInfo" style="height: 30px; width: 120px; margin-bottom: 18px; margin-top: 10px;">Добавить транш</button></div>
                        }
                        <div class="row"></div>
                        <div>&nbsp;</div>
                    </div>
                    <div class="col-md-6">@Html.HiddenFor(m => Model.RequestedConditions[Model.index].SpecialConditionsCount)</div>
                    <div class="col-md-12">
                        <div class="specConditions">
                            @Html.Partial("SpecialConditionsInfo", new SBP.ViewModels.FinInstrumentInfoViewModel()
                            {
                           RequestedConditions = Model.RequestedConditions,
                           AvailabilityPeriodProperties = Model.AvailabilityPeriodProperties,
                           TranshButtonProperties = Model.TranshButtonProperties,
                           index = Model.index,
                            })
                        </div>
                        @if (!Model.TranshButtonProperties.readOnly)
                        {
                            <div style="margin-left: 30px"><button type="button" class="btn btn-sm btn-primary AddSpecConditions" style="height: 30px; width: 120px; margin-bottom: 18px; margin-top: 10px;">Добавить ОУ</button></div>
                        }
                        <div class="row"></div>
                        <div>&nbsp;</div>
                    </div>
                </div>
                <div>&nbsp;</div>
            </div>

        }
        else
        {
            <div class="widget-content referrer">
                <div class="clearfix">
                    <div class="row">
                        <div class="col-md-12">
                            @if (Model.index == 0)
                            {
                                <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[0].Borrower, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
                            }
                        </div>
                    </div>
                    <div>&nbsp;</div>
                    <hr />
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].QuestionEssence, "", Model.QuestionEssenceProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                    <div class="col-md-6">@Html.HtmlTxtArea(m => Model.RequestedConditions[Model.index].UnexecutedSpecConditions, "", Model.UnexecutedSpecConditionsProperties.readOnly, "", "", new { @style = "resize:vertical" })</div>
                    @*<div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].UnexecutedSpecConditions, "list-alt", Model.UnexecutedSpecConditionsProperties.readOnly)</div>*@
                    @if (Model.CreditProgramProperties.readOnly)
                    {
                        <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].CreditProgram, "list-alt", Model.CreditProgramProperties.readOnly)</div>
                    }
                    else
                    {
                        <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].CreditProgramID, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.V_program_TYPE"), "", Model.CreditProgramProperties.readOnly, "")</div>
                    }
                    <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].TargetAppointment, "list-alt", Model.RestructingTypeProperties.readOnly)</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].RestructingType, new SBP.ViewModels.Directory().RestructingTypeList, "", Model.RestructingTypeProperties.readOnly, "")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].Causes, new SBP.ViewModels.Directory().CausesList, "", Model.CausesProperties.readOnly, "input-sm")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].SourseOfCredit, new SBP.ViewModels.Directory().GetReferenceValueList("VIEW_UCLA.v_ref_loan_source"), "", Model.SourseOfCreditProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].StateSupportType, new SBP.ViewModels.Directory().StateSupportTypeList, "", Model.StateSupportProperties.readOnly, "ClaApprovedConditionInfo_Currencies ChangeAnaliticsUI")</div>
                    <div class="col-md-6">@Html.HtmlSelect(m => Model.RequestedConditions[Model.index].CreditIndustry, new SBP.ViewModels.Directory().GetReferenceValueList2("VIEW_UCLA.v_otrasl_oked"), "", Model.RedemptionMethodProperties.readOnly, "")</div>
                    <div>&nbsp;</div>
                </div>
                <div>&nbsp;</div>
            </div>
        }
        <div class="col-md-6" style="display:none">@Html.HtmlText(m => Model.RequestedConditions[Model.index].Hidden, "list-alt", Model.TargetAppointmentProperties.readOnly)</div>
        @if (Model.DeleteButtonProperties.isVisible && Model.index != 0)
        {
            <input type="button" value="Удалить" class="btn btn-danger btn-sm DeleteBtn" style="float: right; height: 25px; width: 100px; margin-bottom: 8px; margin-top: 8px;" />
        }
    </div>
</div>
    function fillObj() {
        var array = [];
        var len = $('.object').length;
        for (var i = 0; i < len; i++) {
            var _obj = new Object();
            var lenTransh = $("#RequestedConditions_" + i + "__TranshCount").val();
            var arrayTransh = [];
            for (var j = 0; j < lenTransh; j++) {
                var _objTransh = new Object();
                _objTransh.TranshTerm = $("#RequestedConditions_" + i + "__TranshInfo_" + j + "__TranshTerm").val();
                _objTransh.TranshCurrency = $("#RequestedConditions_" + i + "__TranshInfo_" + j + "__TranshCurrency").val();
                _objTransh.TranshSum = $("#RequestedConditions_" + i + "__TranshInfo_" + j + "__TranshSum").val();
                arrayTransh.push(_objTransh);
            }
            var lenSpecCondition = $("#RequestedConditions_" + i + "__SpecialConditionsCount").val();
            var arraySpecCondition = [];
            for (var k = 0; k < lenSpecCondition; k++) {
                var _objSpecCondition = new Object();
                _objSpecCondition.SpecConditionsType = $("#RequestedConditions_" + i + "__SpecialConditions_" + k + "__SpecConditionsType").val();
                _objSpecCondition.SpecConditionsCategory = $("#RequestedConditions_" + i + "__SpecialConditions_" + k + "__SpecConditionsCategory").val();
                _objSpecCondition.SpecConditionsText = $("#RequestedConditions_" + i + "__SpecialConditions_" + k + "__SpecConditionsText").val();
                _objSpecCondition.SpecConditionsData = $("#RequestedConditions_" + i + "__SpecialConditions_" + k + "__SpecConditionsData").val();
                arraySpecCondition.push(_objSpecCondition);
            }
            var lenClients = $("#RequestedConditions_" + i + "__ClientsCount").val();
            var arrayClients = [];
            for (var c = 0; c < lenClients; c++) {
                var _objClients = new Object();
                _objClients.IsChecked = $("input[name*='RequestedConditions[" + i + "].Clients[" + c + "].IsChecked']:checked").val();
                arrayClients.push(_objClients);
            }
            _obj.Borrower = $("#RequestedConditions_0__Borrower").val();
            _obj.QuestionEssence = $("#RequestedConditions_" + i + "__QuestionEssence").val();
            _obj.MortgageCoBorrower = $("#RequestedConditions_" + i + "__MortgageCoBorrower").val();
            _obj.CoBorrower = $("#RequestedConditions_" + i + "__CoBorrower").val();
            _obj.CollateralDeal = $("#RequestedConditions_" + i + "__CollateralDeal").val();
            _obj.BorrowerInfo = $("#RequestedConditions_" + i + "__BorrowerInfo").val();
            _obj.CoBorrowerInfo = $("#RequestedConditions_" + i + "__CoBorrowerInfo").val();
            _obj.Deferral = $("#RequestedConditions_" + i + "__Deferral").val();
            _obj.TargetGroup = $("#RequestedConditions_" + i + "__TargetGroup").val();
            _obj.ProjectImplementationPlace = $("#RequestedConditions_" + i + "__ProjectImplementationPlace").val();
            _obj.CreditProgram = $("#RequestedConditions_" + i + "__CreditProgramID option:selected").text();
            _obj.CreditProgramID = $("#RequestedConditions_" + i + "__CreditProgramID").val();
            _obj.StateProgramType = $("#RequestedConditions_" + i + "__StateProgramType").val();
            _obj.FIType = $("#RequestedConditions_" + i + "__FIType").val();
            _obj.TargetAppointment = $("#RequestedConditions_" + i + "__TargetAppointment").val();
            _obj.FISum = $("#RequestedConditions_" + i + "__FISum").val();
            _obj.FITerm = $("#RequestedConditions_" + i + "__FITerm").val();
            _obj.TranshmaxTerm = $("#RequestedConditions_" + i + "__TranshmaxTerm").val();
            _obj.PercentageRate = $("#RequestedConditions_" + i + "__PercentageRate").val();
            _obj.Fines = $("#RequestedConditions_" + i + "__Fines").val();
            _obj.Commissions = $("#RequestedConditions_" + i + "__Commissions").val();
            _obj.IsCommissionsRequested = $("#RequestedConditions_" + i + "__IsCommissionsRequested").is(":checked")
            _obj.RedemptionMethod = $("#RequestedConditions_" + i + "__RedemptionMethod").val();
            _obj.OrderOfRedemption = $("#RequestedConditions_" + i + "__OrderOfRedemption").val();
            _obj.MainDebtRedemption = $("#RequestedConditions_" + i + "__MainDebtRedemption").val();
            _obj.WayOfMastering = $("#RequestedConditions_" + i + "__WayOfMastering").val();
            _obj.AvailabilityPeriod = $("#RequestedConditions_" + i + "__AvailabilityPeriod").val();
            _obj.Currency = $("#RequestedConditions_" + i + "__Currency").val();
            _obj.GracePeriod = $("#RequestedConditions_" + i + "__GracePeriod").val();
            _obj.RemunerationGracePeriod = $("#RequestedConditions_" + i + "__RemunerationGracePeriod ").val();
            _obj.SpecConditionsBefore = $("#RequestedConditions_" + i + "__SpecConditionsBefore").val();
            _obj.SpecConditionsAfter = $("#RequestedConditions_" + i + "__SpecConditionsAfter").val();
            _obj.Classification = $("#RequestedConditions_" + i + "__Classification").val();
            _obj.Periodexecution = $("#RequestedConditions_" + i + "__Periodexecution").val();
            _obj.SourseOfCredit = $("#RequestedConditions_" + i + "__SourseOfCredit").val();
            _obj.StateSupportType = $("#RequestedConditions_" + i + "__StateSupportType").val();
            _obj.CreditIndustry = $("#RequestedConditions_" + i + "__CreditIndustry").val();
            _obj.CreditIndustryCode = $("#RequestedConditions_" + i + "__CreditIndustryCode").val();
            _obj.OverflowIndication = $("#RequestedConditions_" + i + "__OverflowIndication").val();
            _obj.EssenceOfChange = $("#RequestedConditions_" + i + "__EssenceOfChange").val();
            _obj.UnexecutedSpecConditions = $("#RequestedConditions_" + i + "__UnexecutedSpecConditions").val();
            _obj.RestructingType = $("#RequestedConditions_" + i + "__RestructingType").val();
            _obj.Causes = $("#RequestedConditions_" + i + "__Causes").val();
            _obj.Km_Comment = $("#RequestedConditions_" + i + "__Km_Comment").val();
            _obj.GreenProject = $("#RequestedConditions_" + i + "__GreenProject").val();    //Зеленый проект
            _obj.TranshInfo = arrayTransh;
            _obj.SpecialConditions = arraySpecCondition;
            _obj.Clients = arrayClients;
            array.push(_obj);
            var data = new Object();
            data.isChecked = $("#RequestedConditions_" + i + "__IsCommissionsRequested").is(":checked")
            $.get(urlBase + "CheckService/IsCommissionDocExist", data, function (result) {
                if (result != "" && result != null)
                {
                    alert(result)
                }
            });
        }
        return array;
    }
});
