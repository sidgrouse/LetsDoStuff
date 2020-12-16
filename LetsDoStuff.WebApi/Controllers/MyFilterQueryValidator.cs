using System;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.OData;
using Microsoft.OData.UriParser;

public class MyFilterQueryValidator : FilterQueryValidator
{
    private static readonly string[] AllowedProperties = { "Tags", "Name" };

    public override void ValidateSingleValuePropertyAccessNode(
        SingleValuePropertyAccessNode propertyAccessNode,
        ODataValidationSettings settings)
    {
        string propertyName = null;
        if (propertyAccessNode != null)
        {
            propertyName = propertyAccessNode.Property.Name;
        }

        if (propertyName != null && !AllowedProperties.Contains(propertyName))
        {
            throw new ODataException(
                string.Format("Filter on {0} not allowed", propertyName));
        }

        base.ValidateSingleValuePropertyAccessNode(propertyAccessNode, settings);
    }

    public MyFilterQueryValidator(DefaultQuerySettings defaultQuerySettings)
                                                : base(defaultQuerySettings)
    {
    }
}
