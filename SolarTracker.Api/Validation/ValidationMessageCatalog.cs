using System.Globalization;

namespace SolarTracker.Api.Validation;

internal static class ValidationMessageCatalog
{
    private const string SortByMustBeDefinedWhitelistedFieldMessage = "SortBy must be a defined whitelisted field.";
    private const string MaximumPredicateLeavesSupportedTemplate = "A maximum of {0} predicate leaves is supported.";
    private const string FilterNestingMustNotExceedTemplate = "Filter nesting must not exceed {0} levels.";
    private const string CombinerItemsCannotBeNullTemplate = "{0}.items cannot be null.";
    private const string PredicateFieldMustBeDefinedOnWhitelistMessage = "Predicate field must be defined on the whitelist.";
    private const string FieldNotWhitelistedRootScalarTemplate = "Field '{0}' is not a whitelisted root scalar.";
    private const string ValueRequiredForIntegerWhitelistFieldsTemplate = "{0} is required for integer whitelist fields.";
    private const string ValueMustNotBeSetForIntegerWhitelistFieldsTemplate = "{0} must not be set for integer whitelist fields.";
    private const string OperatorNotPermittedForIntegerFieldsTemplate = "Operator '{0}' is not permitted for integer fields.";
    private const string ValueRequiredWhenFilteringStringWhitelistFieldsTemplate =
        "{0} is required when filtering string whitelist fields.";
    private const string ValueMustNotBeSetForStringWhitelistFieldsTemplate = "{0} must not be set for string whitelist fields.";
    private const string OperatorNotPermittedForStringFieldsTemplate = "Operator '{0}' is not permitted for string fields.";
    private const string ValueRequiredWhenFilteringOnFieldTemplate = "{0} is required when filtering on '{1}'.";
    private const string OperatorNotPermittedForFieldTemplate = "Operator '{0}' is not permitted for '{1}'.";
    private const string MovePinsMustDifferMessage = "MoveUpGpioPin and MoveDownGpioPin must differ.";

    internal static string SortByMustBeDefinedWhitelistedField() => SortByMustBeDefinedWhitelistedFieldMessage;

    internal static string MaximumPredicateLeavesSupported(int maxLeafCount) =>
        string.Format(CultureInfo.InvariantCulture, MaximumPredicateLeavesSupportedTemplate, maxLeafCount);

    internal static string FilterNestingMustNotExceed(int maxDepth) =>
        string.Format(CultureInfo.InvariantCulture, FilterNestingMustNotExceedTemplate, maxDepth);

    internal static string CombinerItemsCannotBeNull(string combiner) =>
        string.Format(CultureInfo.InvariantCulture, CombinerItemsCannotBeNullTemplate, combiner);

    internal static string PredicateFieldMustBeDefinedOnWhitelist() => PredicateFieldMustBeDefinedOnWhitelistMessage;

    internal static string FieldNotWhitelistedRootScalar<TField>(TField field) =>
        string.Format(CultureInfo.InvariantCulture, FieldNotWhitelistedRootScalarTemplate, field);

    internal static string ValueRequiredForIntegerWhitelistFields(string valueName) =>
        string.Format(CultureInfo.InvariantCulture, ValueRequiredForIntegerWhitelistFieldsTemplate, valueName);

    internal static string ValueMustNotBeSetForIntegerWhitelistFields(string valueName) =>
        string.Format(CultureInfo.InvariantCulture, ValueMustNotBeSetForIntegerWhitelistFieldsTemplate, valueName);

    internal static string OperatorNotPermittedForIntegerFields<TValue>(TValue value) =>
        string.Format(CultureInfo.InvariantCulture, OperatorNotPermittedForIntegerFieldsTemplate, value);

    internal static string ValueRequiredWhenFilteringStringWhitelistFields(string valueName) =>
        string.Format(CultureInfo.InvariantCulture, ValueRequiredWhenFilteringStringWhitelistFieldsTemplate, valueName);

    internal static string ValueMustNotBeSetForStringWhitelistFields(string valueName) =>
        string.Format(CultureInfo.InvariantCulture, ValueMustNotBeSetForStringWhitelistFieldsTemplate, valueName);

    internal static string OperatorNotPermittedForStringFields<TValue>(TValue value) =>
        string.Format(CultureInfo.InvariantCulture, OperatorNotPermittedForStringFieldsTemplate, value);

    internal static string ValueRequiredWhenFilteringOn<TField>(string valueName, TField field) =>
        string.Format(CultureInfo.InvariantCulture, ValueRequiredWhenFilteringOnFieldTemplate, valueName, field);

    internal static string OperatorNotPermittedForField<TOperator, TField>(TOperator value, TField field) =>
        string.Format(CultureInfo.InvariantCulture, OperatorNotPermittedForFieldTemplate, value, field);

    internal static string MovePinsMustDiffer() => MovePinsMustDifferMessage;
}
