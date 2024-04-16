<div class="col-md-12 table object" style="border: 1px solid #4F7199; padding-bottom:10px;">
    <div class="row">
        <div class="col-md-12 FiNumber"></div> // содержание этого нужно 
    </div>

    <div class="col-md-2 pull-right">
        <input type="button" value="Удалить ФИ" class="btn-xs DeleteStartGesvObject btn-danger" />
        @if (Model.BaseClaFinInstrument[Model.index].FinInstrumentKind == "CL" && string.IsNullOrEmpty(Model.BaseClaFinInstrument[Model.index].ParentCL))
        {
        <input type="button" value="Добавить подлимит" class="btn-xs AddUnderLimitFinIns btn-primary" /> //при нажатии этой кнопки, необходимо получить содержание .FiNumber
        }
    </div>
</div>
