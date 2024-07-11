        [RegularExpression(@"^\d+$", ErrorMessage = "'{0}' может принимать только цифровое значение")]
        [DisplayName("Сумма ФИ")]
        public decimal FISum { get; set; }

        данное поле необходимо отобразить с разделением по нулям, читабельным сделать короче 

        <div class="col-md-6">@Html.HtmlText(m => Model.RequestedConditions[Model.index].FISum, "list-alt", Model.FISumPropertiesProperties.readOnly)</div>
