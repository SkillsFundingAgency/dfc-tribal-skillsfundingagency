// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Tribal" file="CustomModelAttributes.cs">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Custom attributes LangaugePhoneAttribute
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    ///     The language phone attribute, validates a phone number appears to be valid phone number format
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class LanguagePhoneAttribute : DataTypeAttribute, IMetadataAware
    {
        /// <summary>
        ///     The PhoneNumberRegex used to validation the phone number
        /// </summary>
        private static readonly Regex PhoneNumberRegex =
            new Regex(
                @"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        /// <summary>
        ///     The model view name from meta data.
        /// </summary>
        private string modelViewName;

        /// <summary>
        ///     The property name from meta data.
        /// </summary>
        private string propertyName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguagePhoneAttribute" /> class.
        /// </summary>
        public LanguagePhoneAttribute()
            : base(DataType.PhoneNumber)
        {
            ErrorMessage = "The {0} field is not a valid phone number.";
        }

        /// <summary>
        ///     Captures meta data
        /// </summary>
        /// <param name="metadata">
        ///     The metadata object.
        /// </param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            modelViewName = metadata.ContainerType.Name;
            propertyName = metadata.PropertyName;
        }

        /// <summary>
        ///     Checks if the value is valid
        /// </summary>
        /// <param name="value">
        ///     The value to check
        /// </param>
        /// <returns>
        ///     The <see cref="bool" /> is returned indicating if the value is valid or not.
        /// </returns>
        public override bool IsValid(object value)
        {
            // Use the language system to get the validation message
            string fieldName = string.Concat(modelViewName, "_Compare_", propertyName);
            string errorMessage = AppGlobal.Language.GetText(fieldName, ErrorMessage, true);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                // Only set error message if we have some text, otherwise an error is raised by the base object
                ErrorMessage = errorMessage;
            }

            if (value == null)
            {
                return true;
            }

            var valueAsString = value as string;
            return valueAsString != null && PhoneNumberRegex.Match(valueAsString).Length > 0;
        }
    }

    /// <summary>
    ///     Provides an attribute that compares two properties
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class LanguageCompareAttribute : CompareAttribute, IMetadataAware
    {
        /// <summary>
        ///     The model view name from meta data
        /// </summary>
        private string modelViewName;

        /// <summary>
        ///     The property name from meta data
        /// </summary>
        private string propertyName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguageCompareAttribute" /> class.
        ///     Provides an attribute that compares two properties
        /// </summary>
        /// <param name="otherProperty">
        ///     The other property to compare
        /// </param>
        public LanguageCompareAttribute(string otherProperty)
            : base(otherProperty)
        {
        }

        /// <summary>
        ///     Captures meta data
        /// </summary>
        /// <param name="metadata">
        ///     The metadata object.
        /// </param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            modelViewName = metadata.ContainerType.Name;
            propertyName = metadata.PropertyName;
        }

        /// <summary>
        ///     Formats the error message
        /// </summary>
        /// <param name="name">
        ///     The property name
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            // Use the language system to get the validation message
            string fieldName = string.Concat(modelViewName, "_Compare_", propertyName);
            string errorMessage = AppGlobal.Language.GetText(fieldName, ErrorMessage, true);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                // Only set error message if we have some text, otherwise an error is raised by the base object
                ErrorMessage = errorMessage;
            }

            return base.FormatErrorMessage(name);
        }
    }

    /// <summary>
    ///     The language email address attribute.
    /// </summary>
    public class LanguageEmailAddressAttribute : DataTypeAttribute, IClientValidatable
    {
        /// <summary>
        ///     The _regex that validates the email address.
        /// </summary>
        private static readonly Regex EmailRegex =
            new Regex(
                @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguageEmailAddressAttribute" /> class.
        /// </summary>
        public LanguageEmailAddressAttribute()
            : base(DataType.EmailAddress)
        {
            ErrorMessage = "The {0} field is not a valid e-mail address.";
        }

        /// <summary>
        ///     The get client validation rules.
        /// </summary>
        /// <param name="metadata">
        ///     The metadata.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <returns>
        ///     An enumerable validation rule
        /// </returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            // Build up the field name
            string languageFieldName = string.Concat(metadata.ContainerType.Name, "_DataCheckEmailAddress_",
                metadata.PropertyName);
            ErrorMessage = AppGlobal.Language.GetText(languageFieldName, ErrorMessage, true);

            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = "The {0} field is not a valid e-mail address.";
            }

            yield return new ModelClientValidationRule
            {
                ValidationType = "email",
                ErrorMessage = FormatErrorMessage(metadata.PropertyName)
            };
        }

        /// <summary>
        ///     Checks if value is valid
        /// </summary>
        /// <param name="value">
        ///     The value to check.
        /// </param>
        /// <returns>
        ///     Returns true if valid, false if invalid.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var valueAsString = value as string;
            return valueAsString != null && EmailRegex.Match(valueAsString).Length > 0;
        }
    }

    /// <summary>
    ///     Provides a general-purpose attribute that lets you specify localizable strings for types and members of an entity
    ///     partial classes
    /// </summary>
    public class LanguageDisplayAttribute : DisplayNameAttribute, IMetadataAware
    {
        /// <summary>
        ///     The default text.
        /// </summary>
        private readonly string defaultText;

        /// <summary>
        ///     The model view name from meta data.
        /// </summary>
        private string modelViewName;

        /// <summary>
        ///     The property name from meta data.
        /// </summary>
        private string propertyName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguageDisplayAttribute" /> class.
        /// </summary>
        public LanguageDisplayAttribute()
            : base(string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguageDisplayAttribute" /> class.
        /// </summary>
        /// <param name="displayName">
        ///     The field name to display for the property, this becomes the default language text
        /// </param>
        public LanguageDisplayAttribute(string displayName)
            : base(displayName)
        {
            defaultText = defaultText ?? displayName;
        }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                // Use the language system to get the validation message
                string fieldName = string.Concat(modelViewName, "_DisplayName_", propertyName);
                string displayName = AppGlobal.Language.GetText(fieldName, defaultText ?? propertyName, true);
                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    return displayName;
                }
                else
                {
                    return base.DisplayName;
                }
            }
        }

        /// <summary>
        ///     The on metadata created object.
        /// </summary>
        /// <param name="metadata">
        ///     The metadata.
        /// </param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            modelViewName = metadata.ContainerType.Name;
            propertyName = metadata.PropertyName;
        }
    }

    /// <summary>
    ///     Specifies the minimum and maximum length of characters that are allowed in a data field
    /// </summary>
    public class LanguageStringLengthAttribute : StringLengthAttribute, IMetadataAware
    {
        /// <summary>
        ///     The model view name.
        /// </summary>
        private string modelViewName;

        /// <summary>
        ///     The property name.
        /// </summary>
        private string propertyName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguageStringLengthAttribute" /> class.
        /// </summary>
        /// <param name="maximumLength">
        ///     The maximum length of the string
        /// </param>
        public LanguageStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
        }

        /// <summary>
        ///     The metadata relating to the attribute, used internally.
        /// </summary>
        /// <param name="metadata">
        ///     The metadata.
        /// </param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            modelViewName = metadata.ContainerType.Name;
            propertyName = metadata.PropertyName;
        }

        /// <summary>
        ///     Applies formatting to a specified error message
        /// </summary>
        /// <param name="name">The name of the field which caused the validation failure</param>
        /// <returns>A string for the error message</returns>
        public override string FormatErrorMessage(string name)
        {
            string defaultErrorMessage = "{0} should be less than {1} characters";

            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = defaultErrorMessage;
            }

            // Use the language system to get the validation message
            string fieldName = string.Concat(modelViewName, "_StringLength_", propertyName);
            string errorMessage = AppGlobal.Language.GetText(fieldName, ErrorMessage, true);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                // Only set error message if we have some text, otherwise an error is raised by the base object
                ErrorMessage = errorMessage;
            }

            return base.FormatErrorMessage(name);
        }
    }

    /// <summary>
    ///     Specifies that a data field value is required
    /// </summary>
    public class LanguageRequired : RequiredAttribute, IMetadataAware
    {
        /// <summary>
        ///     The model view name.
        /// </summary>
        private string modelViewName;

        /// <summary>
        ///     The property name.
        /// </summary>
        private string propertyName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LanguageRequired" /> class.
        /// </summary>
        public LanguageRequired()
            : base()
        {
        }

        /// <summary>
        ///     The metadata relating to the attribute, used internally.
        /// </summary>
        /// <param name="metadata">
        ///     The metadata.
        /// </param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            modelViewName = metadata.ContainerType.Name;
            propertyName = metadata.PropertyName;
        }

        /// <summary>
        ///     Applies formatting to a specified error message
        /// </summary>
        /// <param name="name">The name of the field which caused the validation failure</param>
        /// <returns>A string for the error message</returns>
        public override string FormatErrorMessage(string name)
        {
            // Use the language system to get the validation message
            string fieldName = string.Concat(modelViewName, "_Required_", propertyName);
            string errorMessage;

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                errorMessage = ErrorMessage;
            }
            else
            {
                errorMessage = ErrorMessageString;
            }

            errorMessage = AppGlobal.Language.GetText(fieldName, errorMessage, true);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                // Only set error message if we have some text, otherwise an error is raised by the base object
                ErrorMessage = errorMessage;
            }

            return base.FormatErrorMessage(name);
        }
    }

    public enum DateFormat
    {
        ShortDate,
        LongDate,
        ShortDateTime,
        LongDateTime
    }

    public class DateDisplayFormat : DisplayFormatAttribute
    {
        public DateFormat format = DateFormat.ShortDate;

        public DateDisplayFormat()
        {
            SetFormat();
        }

        public DateFormat Format
        {
            get { return format; }
            set
            {
                format = value;
                SetFormat();
            }
        }

        private void SetFormat()
        {
            switch (format)
            {
                case DateFormat.ShortDateTime:
                    DataFormatString = "{0:" + Constants.ConfigSettings.ShortDateTimeFormat + "}";
                    break;
                case DateFormat.LongDate:
                    DataFormatString = "{0:" + Constants.ConfigSettings.LongDateFormat + "}";
                    break;
                case DateFormat.LongDateTime:
                    DataFormatString = "{0:" + Constants.ConfigSettings.LongDateTimeFormat + "}";
                    break;
                default:
                    DataFormatString = "{0:" + Constants.ConfigSettings.ShortDateFormat + "}";
                    break;
            }
        }
    }

    public class ProviderPortalUrl : RegularExpressionAttribute
    {
        public const String urlRegularExpression = @"^((ht|f)tp(s?)\:\/\/)?[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\(\)\-‌​\.\?\=\,\'\/\\\+&amp;%\$#_~]*)?$";

        public ProviderPortalUrl()
            : base(urlRegularExpression)
        {
        }
    }

    public class ProviderPortalPostcode : RegularExpressionAttribute
    {
        public ProviderPortalPostcode()
            : base(
                @"^(?i)(GIR 0AA|N1P [0-9][A-Z]{2}|[A-PR-UWYZ]([0-9]{1,2}|([A-HK-Y][0-9]|[A-HK-Y][0-9]([0-9]|[ABEHMNPRV-Y]))|[0-9][A-HJKS-UW]) ?[0-9][ABD-HJLNP-UW-Z]{2})$"
                )
        {
        }
    }

    /// <summary>
    ///     Craig Whale
    ///     The provider portal text field attribute.
    ///     For TFS Item:
    ///     Didn't need a regex pattern as its not strictly a pattern match, each of these invalid chars can be checked for in
    ///     isolation.
    /// </summary>
    public class ProviderPortalTextFieldAttribute : ValidationAttribute
    {
        /// <summary>
        ///     The invalid chars.
        /// </summary>
        private const string InvalidChars = @"£^\`<>[]{}¬";

        /// <summary>
        ///     The is valid method which checks the value for any of the invalid characters defined in the constant.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            char[] characters = value.ToString().ToCharArray();

            return characters.All(character => !InvalidChars.ToCharArray().Contains(character));
        }
    }

    public class ProviderPortalEditCourseOpportunityTextFieldAttribute : ValidationAttribute
    {
        /// <summary>
        ///     The invalid chars.
        /// </summary>
        private const string InvalidChars = @"^\`<>[]{}¬";

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            char[] characters = value.ToString().ToCharArray();

            return characters.All(character => !InvalidChars.ToCharArray().Contains(character));
        }
    }

    public class ProviderPortalRangeAttribute : RangeAttribute, IMetadataAware
    {
        /// <summary>
        ///     The model view name from meta data.
        /// </summary>
        private string modelViewName;

        /// <summary>
        ///     The property name from meta data.
        /// </summary>
        private string propertyName;

        public ProviderPortalRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
        }

        /// <summary>
        ///     Captures meta data
        /// </summary>
        /// <param name="metadata">
        ///     The metadata object.
        /// </param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            modelViewName = metadata.ContainerType.Name;
            propertyName = metadata.PropertyName;
        }

        //public override bool IsValid(object value)
        //{
        //    if (value == null) return false;
        //    if (Minimum is Int32 && (Int32)value < (Int32)Minimum) return false;
        //    if (Maximum is Int32 && (Int32)value > (Int32)Maximum) return false;
        //    return true;
        //}

        /// <summary>
        ///     Applies formatting to a specified error message
        /// </summary>
        /// <param name="name">The name of the field which caused the validation failure</param>
        /// <returns>A string for the error message</returns>
        public override string FormatErrorMessage(string name)
        {
            string defaultErrorMessage = "{0} should be between {1} and {2}";

            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = defaultErrorMessage;
            }

            // Use the language system to get the validation message
            string fieldName = string.Concat(modelViewName, "_Range_", propertyName);
            string errorMessage = AppGlobal.Language.GetText(fieldName, ErrorMessage, true);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                // Only set error message if we have some text, otherwise an error is raised by the base object
                ErrorMessage = errorMessage;
            }

            return base.FormatErrorMessage(name);
        }
    }
}