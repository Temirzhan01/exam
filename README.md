@model SBP.ViewModels.MsbClientViewModel
@{
    var customTitle = "По проектам, по которым залоговое имущество, принадлежащее Заемщику, по стоимости для целей залога обеспечивает менее 60 (шестьдесят) процентов от суммы обязательств и при этом дополнительно в залог предоставлено имущество, принадлежащее третьим(-ему) лицам(-у), обязательным условием является привлечение в качестве Созаемщиков-Залогодателей-третьих лиц, без права участия их в освоении финансового инструмента. Данное требование не распространяется на проекты, которые одновременно соответствуют нижеследующим условиям:" +
"-лимит по проекту не превышает 10 000 000 (десять миллионов) тенге, " +
"-по проекту залогодателями-третьими лицами являются физические лица. В остальных случаях решение о включении данного условия в условия проекта принимается соответствующим КК";
}

<script>
        $(function () {
            $(document).on("change", "input[type=checkbox]", function () {
                var data = new Object();
                data.index = 1;
                data.isChecked = $("input[name*='IsCodebtorHaveRight']:checked").val()
            $.post(urlBase + "@Model._Controller/SaveIsCodebtorHaveRight", data , function () { });
        });
</script>

<div class="col-md-12">
    <div class="widget">
        <!-- Widget title -->
        <div class="widget-head">
            @*<div class="pull-left"><i class="fa fa-table"></i> Созаемщик</div>*@
            <div class="pull-left"><i class="fa fa-table"></i>Созаемщик/Созаёмщик – Залогодатель<span title1="@customTitle" class="tooltip2"><label class="error2" title="">?</label></span></div>
            <div class="widget-icons pull-right">
                <a href="#" class="wminimize"><i class="fa fa-chevron-up"></i></a>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="widget-content referrer">
            <!-- Widget content -->
            <table class="table table-striped table-bordered table-hover">
                @{
                    SBP.Models.MsbClient firstchief_model = null;
                    SBP.Models.MsbDbCheck dbcheck_model = null;
                    ViewBag.i = 1;

                }

                @foreach (SBP.Models.MsbClient mc in Model.BaseMsbClient.Where(m => m.CliMark == "Codebtor"))
                {
                    <tr><td colspan=2 style="background-color: #e6ebf5">@ViewBag.i@{ViewBag.i++;}. Созаемщик</td></tr>
                    <tr>
                        <td style="width: 350px">Тип клиента</td>
                        <td>@SBP.ViewModels.AdditionalModel.ClientType.Where(m => m.Value == @mc.CliJurFl.ToString()).First().Text</td>
                    </tr>
                    <tr>
                        @if (mc.CliJurFl == 1)
                        {
                            <td>БИН</td>
                        }
                        else
                        {
                            <td>ИИН</td>
                        }
                        <td>@mc.CliBinIin</td>
                    </tr>
                    if (mc.CliJurFl == 2)
                    {
                        <tr>
                            <td>ФИО</td>
                            <td>@mc.LastName @mc.FirstName</td>
                        </tr>
                    }
                    else
                    {
                        <tr>
                            <td>@Html.LabelFor(m => mc.CliName)</td>
                            <td>@mc.CliName</td>
                        </tr>
                    }
                    <tr>
                        @if (mc.CliJurFl == 1)
                        {
                            <td>Дата регистрации</td>
                        }
                        else
                        {
                            <td>Дата рождения</td>
                        }
                        <td>@mc.CliRegBirthdate.ToShortDateString()</td>
                    </tr>
                    if (mc.CliJurFl != 1)
                    {
                        <tr>
                            <td>@Html.LabelFor(m => mc.CliDocNum)</td>
                            <td>@mc.CliDocNum</td>
                        </tr>
                        <tr>
                            <td>@Html.LabelFor(m => mc.CliDocGivenDate)</td>
                            <td>@mc.CliDocGivenDate.ToShortDateString()</td>
                        </tr>
                    }
                    <tr>
                        <td>@Html.LabelFor(m => mc.IsCodebtorHaveRight)</td>
                        <td id="@ViewBag.iIndex" >@Html.CheckBoxFor(m => mc.IsCodebtorHaveRight, false) Да</td>
                    </tr>
                    // Проверка по базам
                    if (!Model._Stage.Equals("StartPage") && !Model._Stage.Equals("GeneralData"))
                    {
                        dbcheck_model = Model.BaseMsbDbCheck.Where(m => m.DbcCliId == mc.CliId).Where(m => m.DbcStatus.Equals("CodebtorCheck")).FirstOrDefault();
                        <tr>
                            <td colspan="2">
                                @if (dbcheck_model == null && Model._Controller.Equals("DBCheck"))
                                {
                                    <div style="margin: 10px 0;">
                                        @Html.ActionLink("Осуществить проверку", "CheckInDb", "DBCheck", new RouteValueDictionary { { "cli_id", mc.CliId.ToString() }, { "cli_mark", "CodebtorCheck" } }, new Dictionary<string, object> { { "class", "check" }, { "title", "Осуществить проверку по базам" } })
                                    </div>
                                }
                                else if (dbcheck_model != null)
                                {
                                    <div style="margin: 10px 0;">
                                        @Html.ActionLink("Результат проверки по базам", "ViewResultInDb", "DBCheck", new RouteValueDictionary { { "cli_id", mc.CliId.ToString() }, { "cli_mark", "CodebtorCheck" }, { "TaskID", Session["TaskID"] }, { "ProcessID", Session["RequestNumber"] } }, new Dictionary<string, object> { { "class", "viewcheck" }, { "Mark", "Созаемщик" }, { "title", "Просмотр результата проверки по базам" } })
                                    </div>
                                }
                            </td>
                        </tr>
                    }

                    // Первый руководитель
                    if (!Model._Stage.Equals("StartPage"))
                    {
                        firstchief_model = Model.BaseMsbClient.Where(m => m.CliMark == "FirstChief").Where(m => m.ChiefOf == mc.CliId).FirstOrDefault();

                        if (firstchief_model != null)
                        {
                            <tr>
                                <td colspan="2">
                                    <div class="widget wgold">
                                        <div class="widget-head">
                                            <div class="pull-left"><i class="fa fa-suitcase"></i> Первый руководитель</div>
                                            <div class="widget-icons pull-right">
                                                <a href="#" class="wminimize"><i class="fa fa-chevron-up"></i></a>
                                            </div>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="widget-content referrer">
                                            <!-- Widget content -->
                                            <table class="table table-striped table-bordered table-hover">
                                                <tr>
                                                    <td style="width: 350px">ФИО</td>
                                                    <td>@firstchief_model.CliName</td>
                                                </tr>
                                                <tr>
                                                    <td>ИИН</td>
                                                    <td>@firstchief_model.CliBinIin</td>
                                                </tr>
                                                <tr>
                                                    <td>Дата рождения</td>
                                                    <td>@firstchief_model.CliRegBirthdate.ToShortDateString()</td>
                                                </tr>
                                                <tr>
                                                    <td>@Html.LabelFor(m => firstchief_model.CliDocNum)</td>
                                                    <td>@firstchief_model.CliDocNum</td>
                                                </tr>
                                                <tr>
                                                    <td>@Html.LabelFor(m => firstchief_model.CliDocGivenDate)</td>
                                                    <td>@firstchief_model.CliDocGivenDate.ToShortDateString()</td>
                                                </tr>
                                                @if (!Model._Stage.Equals("StartPage") && !Model._Stage.Equals("GeneralData"))
                                                {
                                                    dbcheck_model = Model.BaseMsbDbCheck.Where(m => m.DbcCliId == firstchief_model.CliId).Where(m => m.DbcStatus.Equals("FirstChiefCheck")).FirstOrDefault();
                                                    <tr>
                                                        <td colspan="2">
                                                            @if (dbcheck_model == null && Model._Controller.Equals("DBCheck"))
                                                            {
                                                                <div style="margin: 10px 0;">
                                                                    @Html.ActionLink("Осуществить проверку", "CheckInDb", "DBCheck", new RouteValueDictionary { { "cli_id", firstchief_model.CliId.ToString() }, { "cli_mark", "FirstChiefCheck" } }, new Dictionary<string, object> { { "class", "check" }, { "title", "Осуществить проверку по базам" } })
                                                                </div>
                                                            }
                                                            else if (dbcheck_model != null)
                                                            {
                                                                <div style="margin: 10px 0;">
                                                                    @Html.ActionLink("Результат проверки по базам", "ViewResultInDb", "DBCheck", new RouteValueDictionary { { "cli_id", firstchief_model.CliId.ToString() }, { "cli_mark", "FirstChiefCheck" }, { "TaskID", Session["TaskID"] }, { "ProcessID", Session["RequestNumber"] } }, new Dictionary<string, object> { { "class", "viewcheck" }, { "Mark", "Первый руководитель" }, { "title", "Просмотр результата проверки по базам" } })
                                                                    @*@Html.ActionLink("Результат проверки по базам", "ViewResultInDb", "DBCheck", new RouteValueDictionary { { "cli_id", firstchief_model.CliId.ToString() }, { "cli_mark", "FirstChiefCheck" } }, new Dictionary<string, object> { { "class", "viewcheck" }, { "Mark", "Первый руководитель" }, { "title", "Просмотр результата проверки по базам" } })*@
                                                                </div>
                                                            }
                                                        </td>
                                                    </tr>
                                                }
                                            </table>
                                            @if (Model._Controller.Equals("DBCheck"))
                                            {
                                                <div class="widget-foot">
                                                    <div class="pull-right" style="margin-left: 5px;">
                                                        @Html.ActionLink("Удалить", "MsbClientDelete", "Base", new RouteValueDictionary { { "cli_id", firstchief_model.CliId.ToString() }, { "cli_mark", "FirstChief" } }, new Dictionary<string, object> { { "class", "delete pull-right" }, { "BinIin", firstchief_model.CliBinIin }, { "Mark", "Первый руководитель" }, { "title", "Удалить первого руководителя" } })
                                                    </div>
                                                    <div class="pull-right" style="margin-left: 5px;">
                                                        @Html.ActionLink("Изменить", "MsbClientEdit", "Base", new RouteValueDictionary { { "cli_id", firstchief_model.CliId.ToString() }, { "cli_mark", "FirstChief" } }, new Dictionary<string, object> { { "class", "edit pull-right" }, { "Mark", "Первый руководитель" }, { "title", "Изменение данных первого руководителя" } })
                                                    </div>
                                                    <div class="clearfix"></div>
                                                </div>}
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        }
                    }
                    // Конец.Первый руководитель
                    if (Model._Controller.Equals("Home") || Model._Controller.Equals("DBCheck"))
                    {
                        <tr>
                            <td colspan="2" style="padding: 1px 15px; background-color: #e6ebf5">
                                <div class="pull-right" style="margin-left: 5px;">
                                    @Html.ActionLink("Удалить", "MsbClientDelete", "Base", new RouteValueDictionary { { "cli_id", mc.CliId.ToString() }, { "cli_mark", "Codebtor" } }, new Dictionary<string, object> { { "class", "delete pull-right" }, { "BinIin", mc.CliBinIin }, { "Mark", "Созаемщик" }, { "title", "Удалить созаемщика" } })
                                </div>
                                <div class="pull-right" style="margin-left: 5px;">
                                    @Html.ActionLink("Изменить", "MsbClientEdit", "Base", new RouteValueDictionary { { "cli_id", mc.CliId.ToString() }, { "cli_mark", "Codebtor" } }, new Dictionary<string, object> { { "class", "edit pull-right" }, { "Mark", "Созаемщик" }, { "title", "Изменение данных созаемщика" } })
                                </div>
                                @if (firstchief_model == null && !Model._Stage.Equals("StartPage") && mc.CliJurFl == 1)
                                {
                                    <div>
                                        @Html.ActionLink("Добавить первого руководителя", "MsbClientInsert", "Base", new RouteValueDictionary { { "cli_id", mc.CliId.ToString() }, { "cli_mark", "FirstChief" } }, new Dictionary<string, object> { { "class", "insertfc pull-left" }, { "Mark", "Первый руководитель" }, { "title", "Добавить первого руководителя" } })
                                    </div>
                                }
                            </td>
                        </tr>
                    }
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                }
            </table>
            @if (Model._Controller.Equals("Home") || Model._Controller.Equals("DBCheck"))
            {
                <div class="widget-foot">
                    <div class="pull-right" style="margin-left: 5px;">
                        @Html.ActionLink("Добавить", "MsbClientInsert", "Base", new RouteValueDictionary { { "cli_id", "0" }, { "cli_mark", "Codebtor" } }, new Dictionary<string, object> { { "class", "insert pull-right" }, { "Mark", "Созаемщик" }, { "title", "Добавить созаемщика" } })
                    </div>
                    <div class="clearfix"></div>
                </div>}
        </div>
    </div>
</div>
