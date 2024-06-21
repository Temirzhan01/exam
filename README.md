import * as React from 'react';
import {
  Row,
  Col,
  Form,
  FormGroup,
  FormControl,
  ControlLabel,
  Checkbox,
  Button,
  Modal,
} from 'react-bootstrap';

import { SharedServices, TranshMsb, api } from "server";
import { Wizard, WizardStepComponent } from "core/components";
import { SupPubApiesControl } from 'common/components'
import { findIndex, get as _get, times, xor } from 'lodash';
import { DateTimeField } from 'core/ui'
import { Locale } from 'core/utils';
import * as classNames from 'classnames';
import { Typeahead } from 'react-bootstrap-typeahead';
import * as NumberFormat from 'react-number-format';
import { FinInstrumentData } from './FinInstrumentData';
import { connect } from 'tls';
import { Console } from 'console';

declare const PUB_APIES_URL: string;

class NumberValidator {
  code: number;
  message: string;


  constructor(code: number, message: string) {
    this.code = code;
    this.message = message;
  }
}

interface LocalState {
  isSigned: boolean;
  isReadOnly: boolean;
  showModal: boolean;
  showEmptyModal: boolean;
  showPrintModal: boolean;
  showPrintMessage: string;
  pUrl: string;
  fieldsSubsidCount: 2; // это я добавил
}

interface SuperFormaState {
  requestNumber: string;
  error: string;
  role: string;
  commentInfoData: TranshMsb.CommentInfoModel;
  finInstrumentInfoData: TranshMsb.FinInstrumentInfoModel;
  directories: TranshMsb.Directories;
  baseCheckInfoData: SharedServices.BaseCheckInfoModel;
  documentsInfoData: TranshMsb.DocumentInfoModel;
  eddDocList: SharedServices.DocumentModel[];
  clientInfoData: TranshMsb.ClientInfoModel;
  conditionModel: TranshMsb.ConditionModel
}

export class SuperForma extends WizardStepComponent<SuperFormaState & LocalState> {
  private static readonly initialState = {
    role: '',
    requestNumber: '',
    commentInfoData: {},
    finInstrumentInfoData: {},
    directories: {},
    baseCheckInfoData: {},
    documentsInfoData: {},
    clientInfoData: {},
    eddDocList: [],
    isSigned: false,
    isReadOnly: false,
    showModal: false,
    showEmptyModal: false,
    showPrintModal: false,
    showPrintMessage: '',
    error: '',
    pUrl: '',
    conditionModel: {}
  };

  static contextTypes = {
    isLoading: React.PropTypes.bool
  };


  constructor(props, context) {
    super(props, context);
    this.state = Object.assign({}, SuperForma.initialState, this.state);

    this.clearComment = this.clearComment.bind(this);
  }

  render( ) {
    console.log(this.state, 'syate')
    const { conditionModel } = this.state;

    try {
      if(conditionModel.conditions.length  > 0) {
        this.disableButton("Next")
      }
      else if(conditionModel.conditions.length == 0){
        this.enableButton("Next")
      }
    }
    catch (error) {
      this.enableButton("Next")
    }

    
    return (
      <Wizard>
        <Form horizontal>
          {this.renderMainSection()}
        </Form>
      </Wizard>
    );
  }

  renderMainSection() {
    const {
      requestNumber,
      role,
      finInstrumentInfoData,
      directories,
      error,
      clientInfoData,
      conditionModel
    } = this.state;
    let isReadonly: boolean = role != 'assistant';

    isReadonly = isReadonly ? true : finInstrumentInfoData.uploadLoan.status != 0 ? true : false;

    return (
      <div className="form-section">
        <div className="form-row form-row_padded">
          <FormGroup>
            <Col sm={3}><ControlLabel>Наименование клиента:</ControlLabel></Col>
            <Col sm={6}>
              <FormControl
                type="text"
                value={clientInfoData.clientData.fullName || ''}
                disabled={true}
              />
            </Col>
          </FormGroup>
        </div>
        <br />
        <table width={'1500px'}>
          <tbody>
            <tr>
              <td width={'33%'} style={{ verticalAlign: 'top' }}>
                {this.renderCreditLineSection(isReadonly)}
                {this.renderRequestedConditionsSection(isReadonly)}
              </td>
              <td width={'33%'} style={{ verticalAlign: 'top' }}>{this.renderParameterSection(isReadonly)}</td>
              <td width={'33%'} style={{ verticalAlign: 'top' }}>{this.renderPocketDocumentSection(isReadonly)}</td>
            </tr>
          </tbody>
        </table>
        <FormGroup>
          {
            finInstrumentInfoData.stateParticipant == 'kfu' &&
            this.renderKfuErrorSection()}
          {role == 'assistant' && this.renderDataBaseCheckSection()}
          {this.renderSection()}
          {this.renderGesvCommissionSection()}
          {finInstrumentInfoData.finInstrumentData.purposeType == 'Рефинансирование с другого Банка/МФО' && role == 'dabo' &&
            this.renderRegistorSection()}
          {this.renderButtonSection()}
          {this.renderCommentSection()}
        </FormGroup>
        <FormGroup>
          {error && this.renderErrorSection()}
          {conditionModel.conditions && conditionModel.conditions.length > 0 && this.renderErrorSection2()}
        </FormGroup>
      </div>
    )
  }

  renderRequestedConditionsSection(isReadOnly) {
    const { finInstrumentInfoData, directories, role } = this.state;
    const { currentCreditLines, finInstrumentData, stateParticipant, isGuarantee, isSubsidy } = finInstrumentInfoData;
    const { isNonRepayment, errorKfu, isPrepaymentPenalty, additionalFund } = finInstrumentData;

    let timeStyle: string = '';
    let timeStyleSubEnd: string = '';
    let timeStyleSubStart: string = '';
    let timeStyleGarantEnd: string = '';

    let hide = false;

    let today = new Date();
    if (finInstrumentData.credCommitteDateString && finInstrumentData.fundDateString) {
      let dateParts = finInstrumentData.fundDateString.split(".");
      let datePartsKK = finInstrumentData.credCommitteDateString.split(".");
      let fundDate = new Date(+dateParts[2], +dateParts[1] - 1, +dateParts[0]);
      let credComitteDate = new Date(+datePartsKK[2], +datePartsKK[1] - 1, +datePartsKK[0]);
      timeStyle = (finInstrumentData.credCommitteDateString != 'Invalid date' && finInstrumentData.fundDateString != 'Invalid date') ? (fundDate > credComitteDate && fundDate <= today && credComitteDate <= today) ? '' : '2px solid #F78181' : '2px solid #F78181';

    }

    if ((stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) {
      let dateParts = finInstrumentData.endDateString.split(".");
      let transhDate = new Date(+dateParts[2], +dateParts[1] - 1, +dateParts[0]);
      let datePartsCL = currentCreditLines.endDateString.split(".");
      let CLEndDate = new Date(+datePartsCL[2], +datePartsCL[1] - 1, +datePartsCL[0]);

      if ((isSubsidy && !finInstrumentData.subEndDateString) && (isGuarantee && !finInstrumentData.garantEndDateString)) {
        hide = true;
      }

      if (isSubsidy && finInstrumentData.subEndDateString) {
        dateParts = finInstrumentData.subEndDateString.split(".");
        let subsidyDate = new Date(+dateParts[2], +dateParts[1] - 1, +dateParts[0]);
        timeStyleSubEnd = (finInstrumentData.subEndDateString != 'Invalid date') ? (CLEndDate > subsidyDate && subsidyDate > today) ? '' : '2px solid #F78181' : '2px solid #F78181'
        if (transhDate < subsidyDate) hide = true;
      }

      if (isSubsidy && finInstrumentData.subStartDateString) {
        dateParts = finInstrumentData.subStartDateString.split(".");
        timeStyleSubStart = (finInstrumentData.subStartDateString != 'Invalid date') ? (finInstrumentData.subStartDateString == new Date().toLocaleDateString()) ? '' : '2px solid #F78181' : '2px solid #F78181'

      }

      if (isGuarantee && finInstrumentData.garantEndDateString) {
        dateParts = finInstrumentData.garantEndDateString.split(".");
        let garantDate = new Date(+dateParts[2], +dateParts[1] - 1, +dateParts[0]);
        timeStyleGarantEnd = (finInstrumentData.garantEndDateString != 'Invalid date') ? (garantDate > today) ? '' : '2px solid #F78181' : '2px solid #F78181'

        if (transhDate < garantDate) hide = true;
      }

      if (isSubsidy && !finInstrumentData.subEndDateString) {
        hide = true;
      }

      if (isGuarantee && !finInstrumentData.garantEndDateString) {
        hide = true;
      }

    }

    return (
      <Col sm={12}>
        <div className="form-section__header">
          <h3 className="form-section__title">{'Запрашиваемые условия по траншу'}</h3>
        </div>
        <div className="form-section__body">
          <div className="form-row form-row_padded">
            {
              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>№ решения Фонда:</ControlLabel></Col>
                <Col sm={5}>
                  <FormControl
                    type="text"
                    style={{ border: finInstrumentData.fundNumber ? '' : '2px solid #F78181' }}
                    value={finInstrumentData.fundNumber || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ fundNumber: e.target.value })}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
              </FormGroup>
            }
            {

              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Дата решения Фонда:</ControlLabel></Col>
                <Col sm={5}>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{
                      border: timeStyle
                    }}
                    dateTime={finInstrumentData.fundDateString || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ fundDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
                <Col >
                  <Button onClick={() => this.addAdditionalFund()}>+</Button>
                </Col>
              </FormGroup>
            }
            {
              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) &&
              this.renderAddFundBlock(additionalFund)
            }
            {
              stateParticipant == 'kfu' &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Номер вклада КФУ:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.numberKfu || ''}
                    disabled={true}
                  />
                </Col>
              </FormGroup>
            }
            {
              stateParticipant == 'kfu' &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Дата завершения вклада КФУ:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.endDateKfuString || ''}
                    disabled={true}
                  />
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>Номер транша:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.number || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Сумма транша:</ControlLabel></Col>
              <Col sm={6}>
                <NumberFormat
                  value={finInstrumentData.amountString || ''}
                  displayType={'text'}
                  thousandSeparator={' '}
                  renderText={value => <FormControl type="text" value={value} disabled={true} />}
                />
              </Col>
            </FormGroup>
            {
              stateParticipant != 'kfu' && !isGuarantee &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Сумма изъятия со счета наличными:</ControlLabel></Col>
                <Col sm={6}>
                  <NumberFormat
                    value={finInstrumentData.amountFromAccountString || ''}
                    displayType={'text'}
                    thousandSeparator={' '}
                    renderText={value => <FormControl
                      className={finInstrumentData.amountFromAccountString ? '' : 'input-validation-error'}
                      disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))} type="text"
                      onChange={(e: any) => this.handleFinInstrumentChange({ amountFromAccountString: e.target.value })}
                      value={value} />}
                  />
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>Валюта транша:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.currency || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            {
              finInstrumentData.termType == 'M' &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Срок транша, в мес.:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.term || ''}
                    disabled={true}
                  />
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>Дата завершения транша (для выгрузки в КБ и кред.рег):</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.endDateString || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Число месяца для погашения займа:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.dayMonthRepayment || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Номер счета для зачисления транша (текущий счет):</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.accountCreditCurrent || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Номер счета для погашения транша:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.accountCredit || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Номер карточного счета для погашения:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.cardNumberTransfer || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Заявление на страхование заемщика от НС:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.insuranceNumber || ''}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Ставка вознаграждения:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.interestRate}
                  className={!(finInstrumentData.interestRate < 1) ? '' : 'input-validation-error'}
                  onChange={(e: any) => this.handleRequestedFloatInput({ interestRate: e.target.value }, 'interestRate')}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                />
              </Col>
            </FormGroup>
            {
              (stateParticipant == 'kfu' && errorKfu.code == 13) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Ставка вознаграждения (после окончания срока участия в гос. программе):</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.interestRateState}
                    className={!(finInstrumentData.interestRateState < 1) ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ interestRateState: e.target.value }, 'interestRateState')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
              </FormGroup>
            }
            {
              stateParticipant != 'kfu' &&
              <FormGroup>
                <Col sm={6}><ControlLabel style={{ color: 'red' }}>Наличие в решении УО условий по поддержанию кредитовых оборотов:</ControlLabel></Col>
                <Col sm={6}>
                  <Checkbox
                    checked={finInstrumentData.presenceDecision}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    onChange={(e: any) => this.handleFinInstrumentChange({ presenceDecision: e.target.checked })}>
                  </Checkbox>
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>ГЭСВ:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.gesv || ''}
                  onChange={(e: any) => this.handleRequestedFloatInput({ gesv: e.target.value }, 'gesv')}
                  disabled={true}
                />
              </Col>
            </FormGroup>
            {isPrepaymentPenalty &&
              <FormGroup>
                <Col sm={6}><ControlLabel style={{ color: 'red' }}>Штраф за досрочное погашение:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.prepaymentPenalty}
                    className={finInstrumentData.prepaymentPenalty != null ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ prepaymentPenalty: e.target.value }, 'prepaymentPenalty')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>Штраф за просрочку:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.penalty || 0}
                  className={!(finInstrumentData.penalty < 0.1) ? '' : 'input-validation-error'}
                  onChange={(e: any) => this.handleRequestedFloatInput({ penalty: e.target.value }, 'penalty')}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                />
              </Col>
            </FormGroup>
            {((stateParticipant == 'kfu' && errorKfu.code == 13) || ((stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP') && !hide)) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Штраф за просрочку в случае выхода из программы:</ControlLabel></Col>
                <Col sm={6}>
                  <NumberFormat

                    value={finInstrumentData.penaltyState || 0}
                    displayType={'text'}
                    renderText={value => <FormControl
                      className={!(finInstrumentData.penaltyState < 0.1) ? '' : 'input-validation-error'}
                      onChange={(e: any) => this.handleRequestedFloatInput({ penaltyState: e.target.value }, 'penaltyState')}
                      disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                      value={value} />}
                  />
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>Метод погашения:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  componentClass="select"
                  value={finInstrumentData.redemptionMethod}
                  onChange={(e: any) => this.handleFinInstrumentChange({ redemptionMethod: e.target.value })}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                >
                  {SharedServices.RedemptionOption.RefRus().map((g: any) => <option key={g.value} value={g.value}>{g.name}</option>)}
                </FormControl>
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Льготный период по ОД:</ControlLabel></Col>
              <Col sm={6}>
                <NumberFormat
                  value={finInstrumentData.gracePeriod || ''}
                  displayType="text"
                  renderText={value => <FormControl
                    className={((+finInstrumentData.gracePeriodInterest < finInstrumentData.term) ? '' : 'input-validation-error') || ((finInstrumentData.gracePeriod) ? '' : 'input-validation-error')}
                    disabled={!(role == 'assistant' || role == 'dabo' && isNonRepayment) || finInstrumentData.redemptionOrder.key == 'КР9_ПОГАШ_КР.05' ? true : false} type="text"
                    onChange={(e: any) => this.handleRequestedNumberInput({ gracePeriod: e.target.value }, 'gracePeriod')}
                    value={value} />}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Льготный период по процентам:</ControlLabel></Col>
              <Col sm={6}>
                <NumberFormat
                  value={finInstrumentData.gracePeriodInterest || ''}
                  displayType="text"
                  renderText={value => <FormControl
                    className={((+finInstrumentData.gracePeriodInterest < finInstrumentData.term) ? '' : 'input-validation-error') || ((finInstrumentData.gracePeriodInterest) ? '' : 'input-validation-error')}
                    disabled={!(role == 'assistant' || role == 'dabo' && isNonRepayment) || finInstrumentData.repaymentRemuneration.key == 'КР10_ПОГАШ_%%.05' ? true : false} type="text"
                    onChange={(e: any) => this.handleRequestedNumberInput({ gracePeriodInterest: e.target.value }, 'gracePeriodInterest')}
                    value={value} />}
                />
              </Col>
            </FormGroup>
            {
              (+finInstrumentData.gracePeriodInterest > 0) &&
              <FormGroup>
                <Col sm={6}><ControlLabel style={{ color: 'red' }}>Равномерное распределение отсроченных сумм:</ControlLabel></Col>
                <Col sm={6}>
                  <Checkbox
                    checked={finInstrumentData.isEquallyDivided}
                    disabled={isReadOnly}
                    onChange={(e: any) => this.handleFinInstrumentChange({ isEquallyDivided: e.target.checked })}>
                  </Checkbox>
                </Col>
              </FormGroup>
            }
            <FormGroup>
              <Col sm={6}><ControlLabel>Целевое назначение:</ControlLabel></Col>
              <Col sm={6}>
                <Typeahead
                  isInvalid={finInstrumentData.aimFinance.key ? false : true}
                  labelKey={option => `${option.value}`}
                  defaultSelected={finInstrumentData.aimFinance.key ? directories.aimFinanceOption.slice(directories.aimFinanceOption.findIndex(x => x.key == finInstrumentData.aimFinance.key), directories.aimFinanceOption.findIndex(x => x.key == finInstrumentData.aimFinance.key) + 1) : []}
                  options={
                    directories.aimFinanceOption.map(q => ({ key: q.key, value: q.value }))
                  }
                  onChange={(e: any) => this.handleUnderSectionInputs(e, directories.aimFinanceOption, 'aimFinance')}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Подробная цель финансирования (из заявки Клиента):</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.detailedPurpose || ''}
                  onChange={(e: any) => this.handleFinInstrumentChange({ detailedPurpose: e.target.value })}
                  className={(finInstrumentData.detailedPurpose) ? '' : 'input-validation-error'}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Подробная цель финансирования (из заявки Клиента) на казахском языке:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.detailedPurposeQaz || ''}
                  className={(finInstrumentData.detailedPurposeQaz) ? '' : 'input-validation-error'}
                  onChange={(e: any) => this.handleFinInstrumentChange({ detailedPurposeQaz: e.target.value })}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Тип кредитного комитета:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  componentClass="select"
                  value={finInstrumentData.typeCreditCommitee.key || ''}
                  onChange={(e: any) => this.handleParameterChange(e.target, directories.typeCreditCommiteeOption, 'typeCreditCommitee')}
                  className={(finInstrumentData.typeCreditCommitee.key) ? '' : 'input-validation-error'}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                >
                  <option key={0} value={''}>{'---------Выбор---------'}</option>
                  {directories.typeCreditCommiteeOption.map((g: SharedServices.ReferenceResponse) => <option key={g.key} value={g.key}>{g.value}</option>)}
                </FormControl>
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Номер Решения КК:</ControlLabel></Col>
              <Col sm={6}>
                <FormControl
                  type="text"
                  value={finInstrumentData.credCommitteeDecision || ''}
                  className={(finInstrumentData.credCommitteeDecision) ? '' : 'input-validation-error'}
                  onChange={(e: any) => this.handleFinInstrumentChange({ credCommitteeDecision: e.target.value })}
                  disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                />
              </Col>
            </FormGroup>
            <FormGroup>
              <Col sm={6}><ControlLabel>Дата Решения КК:</ControlLabel></Col>
              <Col sm={6}>
                <DateTimeField
                  {...this.dateFieldProps}
                  style={{ border: timeStyle }}
                  dateTime={finInstrumentData.credCommitteDateString || ''}
                  onChange={(e: any) => this.handleFinInstrumentChange({ credCommitteDateString: e })}
                  inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
              </Col>
            </FormGroup>
            {
              (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP' || stateParticipant == 'newStateSP') &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Виды государственной поддержкии проекта:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    componentClass="select"
                    value={finInstrumentInfoData.stateSupportProgram || ''}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    className={(finInstrumentInfoData.stateSupportProgram) ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleStateSupport({ stateSupportProgram: e.target.value })}
                  >
                    <option value="">Выберите....</option>
                    {directories.stateSupportList.map((g: SharedServices.StateSupportModel) => <option key={g.colvirId} value={g.colvirId}>{g.value}</option>)}
                  </FormControl>
                </Col>
              </FormGroup>
            }
            {
              (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP' || stateParticipant == 'newStateSP') &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Место реализации проекта:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    componentClass="select"
                    value={finInstrumentData.projectLocation || ''}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    className={(finInstrumentData.projectLocation) ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleFinInstrumentChange({ projectLocation: e.target.value })}
                  >
                    <option value="">Выберите....</option>
                    {directories.projectLocationOption.map((g: SharedServices.ReferenceResponse) => <option key={g.key} value={g.key}>{g.value}</option>)}
                  </FormControl>
                </Col>
              </FormGroup>
            }
            {
              ((stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP' || stateParticipant == 'newStateSP') && isSubsidy) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Номер счета Фонда:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    componentClass="select"
                    value={finInstrumentData.fundAccNumber || ''}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                    className={(finInstrumentData.fundAccNumber) ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleFinInstrumentChange({ fundAccNumber: e.target.value })}
                  >
                    <option value="">Выберите....</option>
                    {directories.fundAccNumberList.map((g: SharedServices.StateSupportModel) => <option key={g.id} value={g.id}>{g.value}</option>)}
                  </FormControl>
                </Col>
              </FormGroup>
            }
            {
              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Номер договора субсидирования:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.subAgreementNumber || ''}
                    className={(finInstrumentData.subAgreementNumber) ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subAgreementNumber: e.target.value })}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
              </FormGroup>
            }
            {
              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.subInterest || ''}
                    className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
              </FormGroup>
            }
            {
              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Дата начала субсидирования:</ControlLabel></Col>
                <Col sm={6}>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubStart }}
                    dateTime={finInstrumentData.subStartDateString || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
              </FormGroup>
            }
            {
              (isSubsidy && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Дата окончания субсидирования:</ControlLabel></Col>
                <Col sm={6}>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubEnd }}
                    dateTime={finInstrumentData.subEndDateString || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subEndDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
              </FormGroup>
            }
            {
              (isGuarantee && (stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP')) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Дата окончания гарантирования:</ControlLabel></Col>
                <Col sm={6}>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleGarantEnd }}
                    dateTime={finInstrumentData.garantEndDateString || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ garantEndDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) }} />
                </Col>
              </FormGroup>
            }
            {
              ((stateParticipant == 'state' || stateParticipant == 'newState' || stateParticipant == 'priorityProjects' || stateParticipant == 'newStateSP') && !hide) &&
              <FormGroup>
                <Col sm={6}><ControlLabel>Ставка вознаграждения в случае прекращения участия заемщика в Программе:</ControlLabel></Col>
                <Col sm={6}>
                  <FormControl
                    type="text"
                    value={finInstrumentData.terminationRate || ''}
                    className={(finInstrumentData.terminationRate) ? '' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ terminationRate: e.target.value }, 'terminationRate')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment))}
                  />
                </Col>
              </FormGroup>
            }
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
                <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubStart }}
                    dateTime={finInstrumentData.subStartDateString2 || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) || this.state.fieldsSubsidCount >= 4 }} />  /> // эти условия не работают, они сразу появляются, хотя должны появлиться при условии 
                </Col>
                <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel>
                  <FormControl
                    type="text"
                    value={finInstrumentData.subInterest2 || ''}
                    className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment)) || this.state.fieldsSubsidCount >= 4}  /> // эти условия не работают, они сразу появляются, хотя должны появлиться при условии 
                  />
                </Col>
                <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel>
                  <DateTimeField
                    {...this.dateFieldProps}
                    style={{ border: timeStyleSubStart }}
                    dateTime={finInstrumentData.subStartDateString3 || ''}
                    onChange={(e: any) => this.handleFinInstrumentChange({ subStartDateString: e })}
                    inputProps={{ disabled: !(role == 'assistant' || (role == 'dabo' && isNonRepayment)) || this.state.fieldsSubsidCount >= 6  }} />  /> // эти условия не работают, они сразу появляются, хотя должны появлиться при условии 
                </Col>
                <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel>
                  <FormControl
                    type="text"
                    value={finInstrumentData.subInterest3 || ''}
                    className={(finInstrumentData.subInterest && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData.subInterest) < finInstrumentData.interestRate && finInstrumentData.subInterest != '0') ? '' : 'input-validation-error' : 'input-validation-error'}
                    onChange={(e: any) => this.handleRequestedFloatInput({ subInterest: e.target.value }, 'subInterest')}
                    disabled={!(role == 'assistant' || (role == 'dabo' && isNonRepayment)) || this.state.fieldsSubsidCount >= 6  }  /> // эти условия не работают, они сразу появляются, хотя должны появлиться при условии 
                  />
                </Col>
              </FormGroup>
            }
            {
              <FormGroup>
                <Col sm={6}>
                  <Button className="btn btn-light btn-block" onClick={this.AddSubsidFields}>Добавить</Button>
                </Col>
                <Col sm={6}>
                  <Button className="btn btn-light btn-block" onClick={this.DeleteSubsidFields}>Удалить</Button>
                </Col>
              </FormGroup>
            }
          </div>
        </div>
      </Col>
    )
  }
 // эти нижние 2 метода добавил также
  async AddSubsidFields() {
    console.log("adding"); // в консоль выходит успешно
    if (this.state.fieldsSubsidCount <= 4) {
      this.state.fieldsSubsidCount += 2;
    }
  }

  async DeleteSubsidFields() {
    if (this.state.fieldsSubsidCount >= 4) {
      this.state.fieldsSubsidCount -= 2;
    }
  }
}
