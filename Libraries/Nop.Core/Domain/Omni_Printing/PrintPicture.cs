
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Omni_Printing{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class PrintPicture : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int RequestItemId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual PrintRequestItem RequestItem { get; set; }
    }

}
