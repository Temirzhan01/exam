<Col sm={4} className={!client.searchText || client.searchText.includes('000000000000') ? 'form-group has-error' : 'form-group'}>
  <FormControl type="text" onChange={(e: any) => this.handleOnChange({ searchText: e.target.value }, role)} value={client.searchText || ''} disabled={false} />
  {role === 'auth' && !client.searchText.includes('000000000000') && this.renderButtonIfExists('SearchAuth', { className: 'btn btn-default btn-block' })}
</Col>
