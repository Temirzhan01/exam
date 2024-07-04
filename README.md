        <div class="clearfix">
            <div class="col-md-4 margin-top-10"> как эти 2 выпад. списка в ряд поставить и по середине какой нибудь символ свапа, также кнопку Дублировать чуть по ниже поставить ?
                <label class="control-label col-sm-12" title="Вид фин. инструмента">Дублировать от</label>
                @Html.DropDownList("FinInstrument", new CLASB.ViewModels.Directory().FinInsList(Model.BaseClaFinInstrument), new { @class = "form-control FinIns", @id = "FinIns1" })
                <label class="control-label col-sm-12" title="Вид фин. инструмента">Дублировать в</label>
                @Html.DropDownList("FinInstrument", new CLASB.ViewModels.Directory().FinInsList(Model.BaseClaFinInstrument), new { @class = "form-control FinIns", @id = "FinIns2" })
                <input type="button" value="Дублировать" class="btn-xs Dublicate btn-primary" />
            </div>
        </div>
