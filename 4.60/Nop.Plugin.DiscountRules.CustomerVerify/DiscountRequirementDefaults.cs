
namespace Nop.Plugin.DiscountRules.CustomerVerify
{
    /// <summary>
    /// Represents constants for the discount requirement rule
    /// </summary>
    public static class DiscountRequirementDefaults
    {
        /// <summary>
        /// The system name of the discount requirement rule
        /// </summary>
        public const string SystemName = "DiscountRequirement.MustBeAssignedToCustomer";

        /// <summary>
        /// The key of the settings to save restricted customer
        /// </summary>
        public const string SettingsKey = "DiscountRequirement.MustBeAssignedToCustomer-{0}";

        /// <summary>
        /// The HTML field prefix for discount requirements
        /// </summary>
        public const string HtmlFieldPrefix = "DiscountRulesCustomer{0}";
    }
}
