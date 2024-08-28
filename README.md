        [RegularExpression(@"^[\d\s]+$", ErrorMessage = "'{0}' может принимать только цифровое значение")]
        [DisplayName("Сумма ФИ")]
        public decimal FISum { get; set; }

                            <div class="col-md-6 FiSumClass">@Html.HtmlText(m => Model.RequestedConditions[Model.index].FISum, "list-alt", Model.FISumPropertiesProperties.readOnly, "Введите сумму", "Requested")</div>

        The field Сумма ФИ must be a number.
