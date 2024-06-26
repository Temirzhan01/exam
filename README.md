deleteSubsidFields() {
  this.setState((prevState: SuperFormaState & LocalState) => {
    const count = prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount > 0 
      ? prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount - 1 
      : 0;
    
    // Создаем копию finInstrumentData
    const updatedFinInstrumentData = { ...prevState.finInstrumentInfoData.finInstrumentData };

    // В зависимости от значения count очищаем соответствующие поля
    if (count === 1) {
      updatedFinInstrumentData.subInt2 = '';
      updatedFinInstrumentData.subDate2 = '';
    } else if (count === 0) {
      updatedFinInstrumentData.subInt2 = '';
      updatedFinInstrumentData.subDate2 = '';
      updatedFinInstrumentData.subInt3 = '';
      updatedFinInstrumentData.subDate3 = '';
    }

    return {
      ...prevState,
      finInstrumentInfoData: {
        ...prevState.finInstrumentInfoData,
        finInstrumentData: {
          ...updatedFinInstrumentData,
          fieldsSubsidCount: count,
        }
      }
    };
  });
}
