import React, { Component } from 'react';
import { FormGroup, Col, ControlLabel, FormControl, Button } from 'react-bootstrap';
import DateTimeField from 'react-bootstrap-datetimepicker';

class SubsidyForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      fieldCount: 0,
      finInstrumentData: {
        subStartDateString: '',
        subInterest: '',
        subStartDateString2: '',
        subInterest2: '',
        subStartDateString3: '',
        subInterest3: '',
        subStartDateString4: '',
        subInterest4: '',
        subStartDateString5: '',
        subInterest5: '',
        subStartDateString6: '',
        subInterest6: '',
      }
    };
    this.dateFieldProps = {}; // Замените на ваши реальные пропсы
    this.timeStyleSubStart = {}; // Замените на ваши реальные стили
    this.role = 'assistant'; // или другая роль
    this.isNonRepayment = true; // или другая логика
  }

  handleAddFields = () => {
    if (this.state.fieldCount < 2) {
      this.setState({ fieldCount: this.state.fieldCount + 1 });
    }
  };

  handleRemoveFields = () => {
    if (this.state.fieldCount > 0) {
      this.setState({ fieldCount: this.state.fieldCount - 1 });
    }
  };

  handleFinInstrumentChange = (updatedData) => {
    this.setState({
      finInstrumentData: { ...this.state.finInstrumentData, ...updatedData }
    });
  };

  handleRequestedFloatInput = (updatedData, field) => {
    this.setState({
      finInstrumentData: { ...this.state.finInstrumentData, ...updatedData }
    });
  };

  renderFields = (startIndex) => {
    const fields = [];
    for (let i = 0; i < 2; i++) {
      const index = startIndex + i;
      fields.push(
        <React.Fragment key={index}>
          <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel>
            <DateTimeField
              {...this.dateFieldProps}
              style={{ border: this.timeStyleSubStart }}
              dateTime={this.state.finInstrumentData[`subStartDateString${index + 1}`] || ''}
              onChange={(e) => this.handleFinInstrumentChange({ [`subStartDateString${index + 1}`]: e })}
              inputProps={{ disabled: !(this.role === 'assistant' || (this.role === 'dabo' && this.isNonRepayment)) }} />
          </Col>
          <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel>
            <FormControl
              type="text"
              value={this.state.finInstrumentData[`subInterest${index + 1}`] || ''}
              className={(this.state.finInstrumentData[`subInterest${index + 1}`] && this.state.finInstrumentData.interestRate) ? (parseFloat(this.state.finInstrumentData[`subInterest${index + 1}`]) < this.state.finInstrumentData.interestRate && this.state.finInstrumentData[`subInterest${index + 1}`] !== '0') ? '' : 'input-validation-error' : 'input-validation-error'}
              onChange={(e) => this.handleRequestedFloatInput({ [`subInterest${index + 1}`]: e.target.value }, `subInterest${index + 1}`)}
              disabled={!(this.role === 'assistant' || (this.role === 'dabo' && this.isNonRepayment))}
            />
          </Col>
        </React.Fragment>
      );
    }
    return fields;
  };

  render() {
    const { isSubsidy, stateParticipant } = this.props;

    return (
      <div>
        {(isSubsidy && stateParticipant === 'newStateSP') && (
          <FormGroup>
            {this.renderFields(0)}
            {this.state.fieldCount >= 1 && this.renderFields(2)}
            {this.state.fieldCount >= 2 && this.renderFields(4)}
          </FormGroup>
        )}
        <FormGroup>
          <Col sm={12} className="gap-1">
            <Button className="btn btn-light btn-block" onClick={this.handleAddFields}>Добавить поля</Button>
            {this.state.fieldCount > 0 && <Button className="btn btn-light btn-block" onClick={this.handleRemoveFields}>Удалить последние добавленные поля</Button>}
          </Col>
        </FormGroup>
      </div>
    );
  }
}

export default SubsidyForm;
