﻿using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using RoastedBytes.Nop.Plugin.Misc.ExtendedVendor.Enums;

namespace RoastedBytes.Nop.Plugin.Misc.ExtendedVendor.Models
{
    public class VendorPayoutListModel : BaseNopEntityModel
    {
       
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.Vendor")]
        public int VendorId
        {
            get;
            set;
        }
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.Order")]
        public int OrderId
        {
            get;
            set;
        }
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.VendorOrderTotal")]
        public decimal VendorOrderTotal
        {
            get;
            set;
        }
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.CommissionPercentage")]
        public decimal CommissionPercentage
        {
            get;
            set;
        }
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.PayoutStatus")]
        public PayoutStatus PayoutStatus
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.PayoutStatus")]
        public string PayoutStatusName
        {
            get;
            set;
        }
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.PayoutDate")]
        public DateTime? PayoutDate
        {
            get;
            set;
        }

        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.Remarks")]
        public string Remarks
        {
            get;
            set;
        }
        
        [NopResourceDisplayName("Plugin.Misc.ExtendedVendor.Fields.ShippingCharge")]
        public decimal ShippingCharge
        {
            get;
            set;
        }
    }
}
