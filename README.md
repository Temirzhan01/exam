{
              (isSubsidy && stateParticipant == 'newStateSP') &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubStart }}
                    dateTime={finInstrumentData.subStartDateString || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
                <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel>
                  <FormControl
                    type="text"
                    value={finInstrumentData.subInterest || ''}
                    className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
                <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel> смотри мне нужно вот это поле, также снизу
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubStart }}
                    dateTime={finInstrumentData.subStartDateString2 || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
                <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel> это поле
                  <FormControl
                    type="text"
                    value={finInstrumentData.subInterest2 || ''}
                    className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
                <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel> это поле
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubStart }}
                    dateTime={finInstrumentData.subStartDateString3 || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
                <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel> и это поле скрыть
                  <FormControl
                    type="text"
                    value={finInstrumentData.subInterest3 || ''}
                    className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>

              </FormGroup>
            }
            {
              <FormGroup>
                <Col sm={12} className="gap-1">
                  <Button className="btn btn-light btn-block" onClick={() => }>Добавить поля</Button> и тут при нажатии, они должны появляться, типа первые два поля они по дефолту есть, если нажать на кнопку должны появиться остальные две, если нажать еще раз те еще две, максимум можно нажать 2 раза, также еще одну кнопку для скрытия рядом поставить если есть поля которые были скрыты и открыты, скрыть их, по одному нажатия последние 2 добавленые
                </Col>
              </FormGroup>

              Суть какая, есть 2 поля, при нажатии добавить появляются еще точно таких же 2 поля, просто другие параметры будут внутри, если еще раз нажать то еше 2 поля также, максимум  могут бть 6 полей, также кнопка удалить которая будет очищать и обратно скрывать те поля которые были добавлены помимо двух первых дефолтных
            }
