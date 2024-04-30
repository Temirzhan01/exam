            <div class="clearfix margin-top-10" id="GesvObjects">
                @for (int i = 0; i < Model.BaseClaFinInstrument.Count; i++)
                {
                    <div id="ClaGesvs-@i" class="ClaGesv"> Почему это отображается как внизу на странице? 
                       @Html.Partial("ClaGesv", new CLAMB.ViewModels.ClaFinInstrumentInfoViewModel
                       {
                           BaseClaFinInstrument = Model.BaseClaFinInstrument,
                           index = i,
                           BaseClaCommisAccInfo = Model.BaseClaCommisAccInfo
                       })
                    </div>
                }
            </div>


            <div class="clearfix margin-top-10" id="GesvObjects">
                    <div id="ClaGesvs-0"></div>
                    <div id="ClaGesvs-1"></div> 
                    <div class="col-md-12 table object ClaGesvObject" style="border: 1px solid #4F7199; padding-bottom:10px;"></div>
                    <div class="col-md-12 table object ClaGesvObject" style="border: 1px solid #4F7199; padding-bottom:10px;"></div>
           </div> 
