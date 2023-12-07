  SearchClientDataBlock(client: UBG.ClientModel, role: string) {
    const { onlineBankData } = this.state;
    return (
      <div>
        <Col sm={4} className={!client.searchText && 'form-group has-error'}>
          <FormControl
            type="text"
            onChange={(e: any) => this.handleOnChange({ searchText: e.target.value }, role)}
            value={client.searchText || ''}
            disabled={false}
          />
        </Col>
        <Col sm={1}>
          <FormGroup>
            {
              role == 'auth' && this.renderButtonIfExists('SearchAuth', {
                className: 'btn btn-default btn-block'
              })
            }
          </FormGroup>
        </Col>
      </div>
    );
  }

  Смотри у меня есть компонент поле, если в это поле вводят значение 000000000000 или оно содержит его то, нужно чтобы кнопка стало не активной
