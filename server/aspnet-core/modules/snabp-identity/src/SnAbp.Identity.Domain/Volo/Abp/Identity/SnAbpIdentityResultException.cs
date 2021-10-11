using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Volo.Abp.ExceptionHandling;
using SnAbp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp;

namespace SnAbp.Identity
{
    [Serializable]
    public class SnAbpIdentityResultException : BusinessException, ILocalizeErrorMessage
    {
        public IdentityResult IdentityResult { get; }

        public SnAbpIdentityResultException([NotNull] IdentityResult identityResult)
            : base(
                code: $"Identity.{identityResult.Errors.First().Code}",
                message: identityResult.Errors.Select(err => err.Description).JoinAsString(", "))
        {
            IdentityResult = Check.NotNull(identityResult, nameof(identityResult));
        }

        public SnAbpIdentityResultException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public virtual string LocalizeMessage(LocalizationContext context)
        {
            return IdentityResult.LocalizeErrors(context.LocalizerFactory.Create<IdentityResource>());
        }
    }
}