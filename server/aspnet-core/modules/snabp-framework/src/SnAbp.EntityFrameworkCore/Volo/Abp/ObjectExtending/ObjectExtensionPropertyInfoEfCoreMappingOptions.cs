using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Volo.Abp;
using Volo.Abp.ObjectExtending;

namespace SnAbp.ObjectExtending
{
    public class ObjectExtensionPropertyInfoEfCoreMappingOptions
    {
        [NotNull]
        public ObjectExtensionPropertyInfo ExtensionProperty { get; }

        [NotNull]
        public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

        [CanBeNull]
        public Action<PropertyBuilder> PropertyBuildAction { get; set; }

        public ObjectExtensionPropertyInfoEfCoreMappingOptions(
            [NotNull] ObjectExtensionPropertyInfo extensionProperty,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction = null)
        {
            ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
            
            PropertyBuildAction = propertyBuildAction;
        }
    }
}