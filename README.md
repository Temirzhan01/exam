            BaseViewModel bv = _BaseViewModel;
            int id = bv.BaseClaFinInstrument.Count;
            bv.BaseClaFinInstrument.Add(bv.BaseClaFinInstrument.SingleOrDefault(f => f.ClaApprovedConditionInfo.CreditLineNumber == clNumber));
            bv.BaseClaFinInstrument[id].ParentCL = clNumber;
            bv.BaseClaFinInstrument[id].ClaApprovedConditionInfo.CreditLineNumber = clNumber + "/" + bv.BaseClaFinInstrument.Where(f => f.ParentCL == clNumber).Count();
            _BaseViewModel = bv;

            Смотри какая проблема, есть список объектов в bv.BaseClaFinInstrument я хочу получить одну из них по селекции, потом заново его же добавить в тот список, потом внести изменения в этот добавленный в список объект(скопированный, то есть последний добавленный) и когда я их вношу, меняются оба объекта, тот который оригинальный и тот который копия последний добавленный. Тут они копируются по ссылке, из за этого? Нельзя как то их разделить, чтобы на 2 разных место в памяти обращались? 
