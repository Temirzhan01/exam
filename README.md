        [Required(ErrorMessage = "'{0}' обязательное поле для заполнения")]
        [RegularExpression(@"^\d+$", ErrorMessage = "'{0}' может принимать только цифровое значение")]
        [DisplayName("Срок в мес(при овердрафт - в днях)")]
        public string Period { get; set; }


        как поставить на поле ограничение больше нуля? 
