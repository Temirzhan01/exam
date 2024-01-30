Это метод в котором нужно выполнить мапппинг
        public async Task<IEnumerable<Models.Document>> GetDocumentsFromHDD(string branchNumber)
        {
            using (GetFinToolDocsMSBSoapClient client = new GetFinToolDocsMSBSoapClient()) 
            {
                var hddDocs = await client.GetFinToolDocsAsync(branchNumber);
                return _mapper.Map<Models.Document[]>(hddDocs.Body.GetFinToolDocsResult.Documents);
            }
        }

Это модель в которую надо преобразовать
namespace SPM3._0Service.Models
{
    public class Document
    {
        public string DocSection { get; set; }
        public string DocType { get; set; }
        public string OrigType { get; set; }
        public string ScanRequire { get; set; }
        public string DocLink { get; set; }
        public string ReqNumber { get; set; }
        public string DocNumber { get; set; }
        public string DocDescription { get; set; }
        public string IssueDate { get; set; }
    }
}

а это из которой 
          public ServiceReference.Document[] Documents
        {
            get
            {
                return this.DocumentsField;
            }
            set
            {
                this.DocumentsField = value;
            }
        }

        public partial class Document : object
{
    
    private string DocSectionField;
    
    private string DocSectionCodeField;
    
    private string DocTypeField;
    
    private string DocTypeCodeField;
    
    private string OrigTypeField;
    
    private string ScanRequireField;
    
    private string IssueDateField;
    
    private string DocNumberField;
    
    private string IssuedByField;
    
    private string DocLinkField;
    
    private string ReqNumberField;
    
    private string DocDescriptionField;
    
    private string NameField;
    
    private string SizeField;
    
    private string CreatedField;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
    public string DocSection
    {
        get
        {
            return this.DocSectionField;
        }
        set
        {
            this.DocSectionField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
    public string DocSectionCode
    {
        get
        {
            return this.DocSectionCodeField;
        }
        set
        {
            this.DocSectionCodeField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
    public string DocType
    {
        get
        {
            return this.DocTypeField;
        }
        set
        {
            this.DocTypeField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
    public string DocTypeCode
    {
        get
        {
            return this.DocTypeCodeField;
        }
        set
        {
            this.DocTypeCodeField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
    public string OrigType
    {
        get
        {
            return this.OrigTypeField;
        }
        set
        {
            this.OrigTypeField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
    public string ScanRequire
    {
        get
        {
            return this.ScanRequireField;
        }
        set
        {
            this.ScanRequireField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
    public string IssueDate
    {
        get
        {
            return this.IssueDateField;
        }
        set
        {
            this.IssueDateField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
    public string DocNumber
    {
        get
        {
            return this.DocNumberField;
        }
        set
        {
            this.DocNumberField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
    public string IssuedBy
    {
        get
        {
            return this.IssuedByField;
        }
        set
        {
            this.IssuedByField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
    public string DocLink
    {
        get
        {
            return this.DocLinkField;
        }
        set
        {
            this.DocLinkField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
    public string ReqNumber
    {
        get
        {
            return this.ReqNumberField;
        }
        set
        {
            this.ReqNumberField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=11)]
    public string DocDescription
    {
        get
        {
            return this.DocDescriptionField;
        }
        set
        {
            this.DocDescriptionField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=12)]
    public string Name
    {
        get
        {
            return this.NameField;
        }
        set
        {
            this.NameField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
    public string Size
    {
        get
        {
            return this.SizeField;
        }
        set
        {
            this.SizeField = value;
        }
    }
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=14)]
    public string Created
    {
        get
        {
            return this.CreatedField;
        }
        set
        {
            this.CreatedField = value;
        }
    }
}
