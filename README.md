<div class="clearfix">
    <div class="col-md-4 margin-top-10">
        <label class="control-label col-sm-12" title="Вид фин. инструмента">Дублировать от</label>
        <div class="dropdown-container">
            @Html.DropDownList("FinInstrument", new CLASB.ViewModels.Directory().FinInsList(Model.BaseClaFinInstrument), new { @class = "form-control FinIns", @id = "FinIns1" })
            <span class="swap-symbol">⇄</span>
            @Html.DropDownList("FinInstrument", new CLASB.ViewModels.Directory().FinInsList(Model.BaseClaFinInstrument), new { @class = "form-control FinIns", @id = "FinIns2" })
        </div>
        <input type="button" value="Дублировать" class="btn-xs Dublicate btn-primary duplicate-button" />
    </div>
</div>

.dropdown-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: 10px;
}

.swap-symbol {
    margin: 0 10px;
    font-size: 20px;
}

.duplicate-button {
    display: block;
    margin-top: 20px;
    width: 100%;
}
