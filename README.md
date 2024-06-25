interface LocalState {
  fieldsSubsidCount: number;
}
Есть тут у меня локальная переменная  fieldsSubsidCount в старом реакте классовом.
И есть кнопка которая реагирует, внутри WWF

          {
            status == 0 &&
            <tr>
              <td width={'50%'}>
                {this.renderButtonIfExists('UploadLoan', {
                  className: 'btn btn-primary btn-block',
                })}
              </td>
              <td width={'50%'}>
                {statusMsg}
                {uploadLoan.loanUploadResponse.msg}
              </td>
            </tr>
          }
Как мне по нажатию данной кнопки передать параметр? той локальной переменной, или же другой вариант

this.state.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount можно ли в модельке хранит, которая на бэк передается ? 
типа чтобы так               (isSubsidy && stateParticipant == 'newStateSP' && this.state.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount >= 2) && ( проверка на это поле было

  addSubsidFields() { и тут усножалась данное поле
    this.setState((prevState: SuperFormaState & LocalState) => ({
      ...prevState,
      fieldsSubsidCount: prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount < 2 ? prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount + 1 : prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount,
    }));
  }

  просто когда я просто добавил в интерфейс поле данное и использовал его то выдавало ошибку.
