    var supporttype = new[] { "Субсид", "субсид", "гарантир", "Гарантир" };
    string _stateSupportType = new CLASB.ViewModels.Directory().StateSupportType.SingleOrDefault(x => x.Value == Model.BaseClaFinInstrument[Model.index].StateProgram.StateSupportType).Text;
    Model.FeeRateEndProgramProperties.isVisible = supporttype.Any(x => _stateSupportType.Contains(x)) ? true : false;
