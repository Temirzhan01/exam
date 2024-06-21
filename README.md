import React, { useState } from 'react';
import { FormGroup, Col, ControlLabel, FormControl, Button } from 'react-bootstrap';
import DateTimeField from 'react-bootstrap-datetimepicker';

const SubsidyForm = () => {
  const [fieldCount, setFieldCount] = useState(0);
  const [finInstrumentData, setFinInstrumentData] = useState({
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
  });

  const dateFieldProps = {}; // Замените на ваши реальные пропсы
  const timeStyleSubStart = {}; // Замените на ваши реальные стили
  const role = 'assistant'; // или другая роль
  const isNonRepayment = true; // или другая логика

  const handleAddFields = () => {
    if (fieldCount < 2) setFieldCount(fieldCount + 1);
  };

  const handleRemoveFields = () => {
    if (fieldCount > 0) setFieldCount(fieldCount - 1);
  };

  const handleFinInstrumentChange = (updatedData) => {
    setFinInstrumentData({ ...finInstrumentData, ...updatedData });
  };

  const handleRequestedFloatInput = (updatedData, field) => {
    setFinInstrumentData({ ...finInstrumentData, ...updatedData });
  };

  const renderFields = (startIndex) => {
    const fields = [];
    for (let i = 0; i < 2; i++) {
      const index = startIndex + i;
      fields.push(
        <React.Fragment key={index}>
          <Col sm={6}><ControlLabel>Дата ставки субсидирования:</ControlLabel>
            <DateTimeField
              {...dateFieldProps}
              style={{ border: timeStyleSubStart }}
              dateTime={finInstrumentData[`subStartDateString${index + 1}`] || ''}
              onChange={(e) => handleFinInstrumentChange({ [`subStartDateString${index + 1}`]: e })}
              inputProps={{ disabled: !(role === 'assistant' || (role === 'dabo' && isNonRepayment)) }} />
          </Col>
          <Col sm={6}><ControlLabel>Процент субсидирования:</ControlLabel>
            <FormControl
              type="text"
              value={finInstrumentData[`subInterest${index + 1}`] || ''}
              className={(finInstrumentData[`subInterest${index + 1}`] && finInstrumentData.interestRate) ? (parseFloat(finInstrumentData[`subInterest${index + 1}`]) < finInstrumentData.interestRate && finInstrumentData[`subInterest${index + 1}`] !== '0') ? '' : 'input-validation-error' : 'input-validation-error'}
              onChange={(e) => handleRequestedFloatInput({ [`subInterest${index + 1}`]: e.target.value }, `subInterest${index + 1}`)}
              disabled={!(role === 'assistant' || (role === 'dabo' && isNonRepayment))}
            />
          </Col>
        </React.Fragment>
      );
    }
    return fields;
  };

  return (
    <div>
      {(isSubsidy && stateParticipant === 'newStateSP') && (
        <FormGroup>
          {renderFields(0)}
          {fieldCount >= 1 && renderFields(2)}
          {fieldCount >= 2 && renderFields(4)}
        </FormGroup>
      )}
      <FormGroup>
        <Col sm={12} className="gap-1">
          <Button className="btn btn-light btn-block" onClick={handleAddFields}>Добавить поля</Button>
          {fieldCount > 0 && <Button className="btn btn-light btn-block" onClick={handleRemoveFields}>Удалить последние добавленные поля</Button>}
        </Col>
      </FormGroup>
    </div>
  );
};

export default SubsidyForm;
