        <div class="row">
            @{
                MBP.Models.MsbClient _model = Model.BaseMsbClient.Where(m => m.CliMark == "Codebtor").FirstOrDefault();
            }
            <ul id="myTab" class="nav nav-tabs">
                @if (_model != null && _model.CliJurFl == 1)
                {
                    <li><a href="#OBDb" data-toggle="tab">Проверки OnlineBank</a></li>
                }
                <li class="active"><a href="#InDb" data-toggle="tab">Во внутренних системах</a></li>
                <li><a href="#OutDb" data-toggle="tab">Во внешних системах</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane fade" id="InDb">
                    @Html.Partial("CodebtorInfo", new MBP.ViewModels.MsbClientViewModel { BaseMsbClient = Model.BaseMsbClient, BaseMsbDbCheck = Model.BaseMsbDbCheck, _Stage = "CodebtorCheck", _Controller = "DBCheck" })
                </div>
                <div class="tab-pane fade" id="OutDb">
                    @Html.Partial("CreditBureau", new MBP.ViewModels.MsbClientViewModel { BaseMsbClient = Model.BaseMsbClient, BaseMsbDbCheck = Model.BaseMsbDbCheck, BaseCreditBureau = Model.BaseCreditBureau, _Stage = "CodebtorCheck", _CliMark = "Codebtor", _Controller = "DBCheck" })
                    @Html.Partial("GCVP", new MBP.ViewModels.MsbClientViewModel { BaseMsbClient = Model.BaseMsbClient, BaseMsbDbCheck = Model.BaseMsbDbCheck, BaseGCVP = Model.BaseGCVP, _Stage = "CodebtorCheck", _CliMark = "Codebtor", _Controller = "DBCheck" })
                </div>
                @if (_model != null && _model.CliJurFl == 1)
                {
                    <div class="tab-pane fade active in" id="OBDb">
                        @Html.Partial("CodebtorInfoOB", new MBP.ViewModels.MsbClientViewModel { BaseMsbClient = Model.BaseMsbClient, BaseMsbDbCheck = Model.BaseMsbDbCheck, _Stage = "CodebtorCheck", _Controller = "DBCheck", isCRM = Model.isCRM })
                    </div>
                }
            </div>
        </div>

        
