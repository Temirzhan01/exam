export namespace TranshMsb {
export interface FinInstrumentModel {
  subInterest?: string;
  subInt2: string;
  subInt3: string;
  subStartDateString? : string;
  subDate2? : string;
  subDate3? : string;
  fieldsSubsidCount?: number;
}
}

  deleteSubsidFields() { //нужно чтобы при нажатии на данную кнопку, он удалили значения subInt{fieldsSubsidCount} subDate{fieldsSubsidCount} по номеру, то есть последний, как это сделать?
    this.setState((prevState: SuperFormaState & LocalState) => {
      const count = prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount > 0 ? prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount - 1 : prevState.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount;
      return {
        ...prevState,
        finInstrumentInfoData: {
          ...prevState.finInstrumentInfoData,
          finInstrumentData: {
            ...prevState.finInstrumentInfoData.finInstrumentData,
            fieldsSubsidCount: count,
          }
        }
      };
    });
  }

  {
              (isSubsidy && stateParticipant == 'newStateSP') && (
                <FormGroup>
                  <Col sm={6}>
                    <ControlLabel>Дата начала субсидирования:</ControlLabel>
                    <DateTimeField
                      {...this.dateFieldProps}
                      style={{ border: timeStyleSubStart }}
                      dateTime={finInstrumentData.subStartDateString || ''}
                      onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                      inputProps={{
                        disabled:
                          !(role == 'assistant' || (role == 'dabo' && isNonRepayment)),
                      }}
                    />
                  </Col>
                  <Col sm={6}>
                    <ControlLabel>Процент субсидирования:</ControlLabel>
                    <FormControl
                      type="text"
                      value={finInstrumentData.subInterest || ''}
                      className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                      onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                      disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    />
                  </Col>
                </FormGroup>
              )}
            {
              (isSubsidy && stateParticipant == 'newStateSP' && this.state.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount >= 1) && (
                <FormGroup>
                  <Col sm={6}>
                    <ControlLabel>Дата ставки субсидирования:</ControlLabel>
                    <DateTimeField
                      {...this.dateFieldProps}
                      style={{ border: timeStyleSubStart }}
                      dateTime={finInstrumentData.subDate2 || ''}
                      onChange={(e: any) => this.handleFinInstrumentChange({ subDate2: e })}
                      inputProps={{
                        disabled:
                          !(role == 'assistant' || (role == 'dabo' && isNonRepayment)),
                      }}
                    />
                  </Col>
                  <Col sm={6}>
                    <ControlLabel>Процент субсидирования:</ControlLabel>
                    <FormControl
                      type="text"
                      value={finInstrumentData.subInt2 || ''}
                      className={(finInstrumentData.subInt2 && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInt2) < finInstrumentData.interestRate && finInstrumentData.subInt2 != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                      onChange={(e: any) => this.handleRequestedFloatInput({ subInt2: e.target.value }, 'subInterest')}
                      disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    />
                  </Col>
                </FormGroup>
              )}
            {
              (isSubsidy && stateParticipant == 'newStateSP' && this.state.finInstrumentInfoData.finInstrumentData.fieldsSubsidCount >= 2) && (
                <FormGroup>
                  <Col sm={6}>
                    <ControlLabel>Дата ставки субсидирования:</ControlLabel>
                    <DateTimeField
                      {...this.dateFieldProps}
                      style={{ border: timeStyleSubStart }}
                      dateTime={finInstrumentData.subDate3 || ''}
                      onChange={(e: any) => this.handleFinInstrumentChange({ subDate3: e })}
                      inputProps={{
                        disabled:
                          !(role == 'assistant' || (role == 'dabo' && isNonRepayment)),
                      }}
                    />
                  </Col>
                  <Col sm={6}>
                    <ControlLabel>Процент субсидирования:</ControlLabel>
                    <FormControl
                      type="text"
                      value={finInstrumentData.subInt3 || ''}
                      className={(finInstrumentData.subInt3 && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInt3) < finInstrumentData.interestRate && finInstrumentData.subInt3 != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                      onChange={(e: any) => this.handleRequestedFloatInput({ subInt3: e.target.value }, 'subInterest')}
                      disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    />
                  </Col>
                </FormGroup>
              )}
            {
              (isSubsidy && stateParticipant == 'newStateSP') && (
                <FormGroup>
                  <Col sm={6}>
                    <Button className="btn btn-light btn-block" onClick={this.addSubsidFields}>
                      Добавить
                    </Button>
                  </Col>
                  <Col sm={6}>
                    <Button className="btn btn-light btn-block" onClick={this.deleteSubsidFields}>
                      Удалить
                    </Button>
                  </Col>
                </FormGroup>
              )}
