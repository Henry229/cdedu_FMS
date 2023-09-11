using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Stock : BaseEntity
    {
         public Stock() { }

         public Stock(string itemcode, System.DateTime trandate
            , int inqty, int outqty, string source_type, int source_id, int source_seq, string remarks
            , System.DateTime reg_date, string reg_source)
        {
            this.ItemCode = itemcode;
            this.TranDate = trandate;
            this.InQty = inqty;
            this.OutQty = outqty;
            this.Source_Type = source_type;
            this.Source_Id = source_id;
            this.Source_Seq = source_seq;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime TranDate { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int InQty { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int OutQty { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Source_Type { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Source_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Source_Seq { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Remarks { get; set; }


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
            return ItemCode + "/" + TranDate.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
