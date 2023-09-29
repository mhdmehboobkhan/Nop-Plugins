using Nop.Web.Framework.Models;

namespace Nop.Plugin.Payments.WireTransfer.Models
{
    public record PaymentInfoModel : BaseNopModel
    {
        public string DescriptionText { get; set; }
    }
}