No overload matches this call.
  Overload 1 of 2, '(f: (prevState: SuperFormaState & LocalState & IErrorModel & { _local?: any; }, props: any) => SuperFormaState & LocalState & IErrorModel & { ...; }, callback?: () => any): void', gave the following error.
    Type '{ fieldsSubsidCount: number; }' is not assignable to type 'SuperFormaState & LocalState & IErrorModel & { _local?: any; }'.
      Type '{ fieldsSubsidCount: number; }' is missing the following properties from type 'SuperFormaState': requestNumber, error, role, commentInfoData, and 7 more.
  Overload 2 of 2, '(state: SuperFormaState & LocalState & IErrorModel & { _local?: any; }, callback?: () => any): void', gave the following error.
    Argument of type '(prevState: SuperFormaState & LocalState & IErrorModel & { _local?: any; }) => { fieldsSubsidCount: number; }' is not assignable to parameter of type 'SuperFormaState & LocalState & IErrorModel & { _local?: any; }'.
      Type '(prevState: SuperFormaState & LocalState & IErrorModel & { _local?: any; }) => { fieldsSubsidCount: number; }' is not assignable to type 'SuperFormaState'.ts(2769)
index.d.ts(168, 21): The expected type comes from the return type of this signature.
