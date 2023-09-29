
namespace Nop.Plugin.DiscountRules.ShippingMethods
{
    /// <summary>
    /// Represents constants for the discount requirement rule
    /// </summary>
    public static class DiscountRequirementDefaults
    {
        /// <summary>
        /// The system name of the discount requirement rule
        /// </summary>
        public const string SystemName = "DiscountRequirement.MustBeAssignedToShippingMethod";

        /// <summary>
        /// The key of the settings to save restricted shipping methods
        /// </summary>
        public const string SettingsKey = "DiscountRequirement.MustBeAssignedToShippingMethod-{0}";

        /// <summary>
        /// The HTML field prefix for discount requirements
        /// </summary>
        public const string HtmlFieldPrefix = "DiscountRulesMustBeAssignedToShippingMethod{0}";
    }
}
