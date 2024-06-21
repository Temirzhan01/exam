            {(isSubsidy && stateParticipant == 'newStateSP') && (
              <FormGroup>
                <Col sm={6}>
                  <ControlLabel>Дата ставки субсидирования:</ControlLabel>
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
                    className={
                      finInstrumentData.subInterest && finInstrumentData.interestRate
                        ? parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate &&
                          finInstrumentData.subInterest != '0'
                          ? ''
                          : 'input-validation-error'
                        : 'input-validation-error'
                    }
                    onChange={(e: any) =>
                      this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')
                    }
                    disabled={
                      !(role == 'assistant' || (role == 'dabo' && isNonRepayment))
                    }
                  />
                </Col>
                {this.state.fieldsSubsidCount >= 1 && (
                  <FormGroup>
                    <Col sm={6}>
                      <ControlLabel>Дата ставки субсидирования:</ControlLabel>
                      <DateTimeField
                        {...this.dateFieldProps}
                        style={{ border: timeStyleSubStart }}
                        dateTime={finInstrumentData.subStartDateString2 || ''}
                        onChange={(e: any) =>
                          this.handleFinInstrumentChange({ subStartDateString2: e })
                        }
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
                        value={finInstrumentData.subInterest2 || ''}
                        className={
                          finInstrumentData.subInterest2 && finInstrumentData.interestRate
                            ? parseFloat(finInstrumentData.subInterest2) <
                                finInstrumentData.interestRate &&
                              finInstrumentData.subInterest2 != '0'
                              ? ''
                              : 'input-validation-error'
                            : 'input-validation-error'
                        }
                        onChange={(e: any) =>
                          this.handleRequestedFloatInput({ subInterest2: e.target.value }, 'subInterest2')
                        }
                        disabled={
                          !(role == 'assistant' || (role == 'dabo' && isNonRepayment))
                        }
                      />
                    </Col>
                  </FormGroup>
                )}
                {this.state.fieldsSubsidCount >= 2 && (
                  <FormGroup>
                    <Col sm={6}>
                      <ControlLabel>Дата ставки субсидирования:</ControlLabel>
                      <DateTimeField
                        {...this.dateFieldProps}
                        style={{ border: timeStyleSubStart }}
                        dateTime={finInstrumentData.subStartDateString3 || ''}
                        onChange={(e: any) =>
                          this.handleFinInstrumentChange({ subStartDateString3: e })
                        }
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
                        value={finInstrumentData.subInterest3 || ''}
                        className={
                          finInstrumentData.subInterest3 && finInstrumentData.interestRate
                            ? parseFloat(finInstrumentData.subInterest3) <
                                finInstrumentData.interestRate &&
                              finInstrumentData.subInterest3 != '0'
                              ? ''
                              : 'input-validation-error'
                            : 'input-validation-error'
                        }
                        onChange={(e: any) =>
                          this.handleRequestedFloatInput({ subInterest3: e.target.value }, 'subInterest3')
                        }
                        disabled={
                          !(role == 'assistant' || (role == 'dabo' && isNonRepayment))
                        }
                      />
                    </Col>
                  </FormGroup>
                )}
              </FormGroup>
            )}
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
  
  addSubsidFields() {
    console.log('adding');
    if (this.state.fieldsSubsidCount < 2) {
      this.setState((prevState) => ({
        fieldsSubsidCount: prevState.fieldsSubsidCount + 1,
      }));
    }
  }

  deleteSubsidFields() {
    if (this.state.fieldsSubsidCount > 0) {
      this.setState((prevState) => ({
        fieldsSubsidCount: prevState.fieldsSubsidCount - 1,
      }));
    }
  }
