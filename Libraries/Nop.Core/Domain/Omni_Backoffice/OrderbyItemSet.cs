using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class OrderbyItemSet :BaseEntity
    { public OrderbyItemSet() { }

    //public OrderbyItemSet(int order_id, int itemset_id, int itemset_seq, int qty, int qty_teacher, int qty_newbook
    public OrderbyItemSet(int order_id, int itemset_id, int itemset_seq, int qty, int qty_teacher
            , int course_id, int week, string is_half, string remarks, System.DateTime reg_date, string reg_source) 
        {
            this.Order_Id = order_id;
            this.ItemSet_Id = itemset_id;
            this.Qty = qty;
            this.Qty_Teacher = qty_teacher;
            //this.Qty_NewBook = qty_newbook;
            this.Course_Id = course_id;
            this.Week = week;
            this.is_Half = is_half;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Order_Id { get; set; }

        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int ItemSet_Id { get; set; }


        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Qty_Teacher { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Qty_NewBook { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Course_Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string is_Half { get; set; }

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
            return Order_Id.ToString() + "-" + ItemSet_Id.ToString();
        }
    }
}
