namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class CodeMaster : BaseEntity
    {
        public CodeMaster() { }

        public CodeMaster(string codetype, string typename, string detailcode, string detailname, string string_01, string string_02, string string_03
            , int val_01, int val_02, int val_03, int sortorder, string yn_use, System.DateTime reg_date, string reg_source)
        {
            this.CodeType = codetype;
            this.TypeName = typename;
            this.DetailCode = detailcode;
            this.DetailName = detailname;
            this.String_01 = string_01;
            this.String_02 = string_02;
            this.String_03 = string_03;
            this.Val_01 = val_01;
            this.Val_02 = val_02;
            this.Val_03 = val_03;
            this.SortOrder = sortorder;
            this.YN_Use = yn_use;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string CodeType { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string DetailCode { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string DetailName { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string String_01 { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string String_02 { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string String_03 { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Val_01 { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Val_02 { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Val_03 { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string YN_Use { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime reg_date { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string reg_source { get; set; }



        public override string ToString()
        {
            return DetailName;
        }
    }
}
