addSubsidFields() {
  console.log('adding');
  this.setState((prevState: SuperFormaState & LocalState) => ({
    ...prevState,
    fieldsSubsidCount: prevState.fieldsSubsidCount < 2 ? prevState.fieldsSubsidCount + 1 : prevState.fieldsSubsidCount,
  }));
}

deleteSubsidFields() {
  this.setState((prevState: SuperFormaState & LocalState) => ({
    ...prevState,
    fieldsSubsidCount: prevState.fieldsSubsidCount > 0 ? prevState.fieldsSubsidCount - 1 : prevState.fieldsSubsidCount,
  }));
}
