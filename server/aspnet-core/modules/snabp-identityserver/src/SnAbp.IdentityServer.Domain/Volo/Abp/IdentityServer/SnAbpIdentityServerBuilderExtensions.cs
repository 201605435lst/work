using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using SnAbp.Identity;
using SnAbp.IdentityServer.AspNetIdentity;
using Volo.Abp.Security.Claims;

namespace SnAbp.IdentityServer
{
    public static class SnAbpIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddSnAbpIdentityServer(
            this IIdentityServerBuilder builder,
            SnAbpIdentityServerBuilderOptions options = null)
        {
            if (options == null)
            {
                options = new SnAbpIdentityServerBuilderOptions();
            }

            //TODO: AspNet Identity integration lines. Can be extracted to a extension method
            if (options.IntegrateToAspNetIdentity)
            {
                builder.AddAspNetIdentity<IdentityUser>();
                builder.AddProfileService<SnAbpProfileService>();
                builder.AddResourceOwnerValidator<SnAbpResourceOwnerPasswordValidator>();
            }

            builder.Services.Replace(ServiceDescriptor.Transient<IClaimsService, SnAbpClaimsService>());

            if (options.UpdateAbpClaimTypes)
            {
                AbpClaimTypes.UserId = JwtClaimTypes.Subject;
                AbpClaimTypes.UserName = JwtClaimTypes.Name;
                AbpClaimTypes.Role = JwtClaimTypes.Role;
                AbpClaimTypes.Email = JwtClaimTypes.Email;
            }

            if (options.UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap)
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.UserId] = AbpClaimTypes.UserId;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.UserName] = AbpClaimTypes.UserName;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.Role] = AbpClaimTypes.Role;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.Email] = AbpClaimTypes.Email;
            }

            return builder;
        }

        public static IIdentityServerBuilder AddAbpDeveloperSigningCredential(
            this IIdentityServerBuilder builder,
            bool persistKey = true,
            string filename = null,
            IdentityServerConstants.RsaSigningAlgorithm signingAlgorithm = IdentityServerConstants.RsaSigningAlgorithm.RS256)
        {
            if (filename == null)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "tempkey.rsa");
            }

            if (File.Exists(filename))
            {
                var keyFile = File.ReadAllText(filename);

                var json = JObject.Parse(keyFile);
                var keyId = json.GetValue("KeyId").Value<string>();
                var jsonParameters = json.GetValue("Parameters");
                RSAParameters rsaParameters;
                rsaParameters.D = Convert.FromBase64String(jsonParameters["D"].Value<string>());
                rsaParameters.DP = Convert.FromBase64String(jsonParameters["DP"].Value<string>());
                rsaParameters.DQ = Convert.FromBase64String(jsonParameters["DQ"].Value<string>());
                rsaParameters.Exponent = Convert.FromBase64String(jsonParameters["Exponent"].Value<string>());
                rsaParameters.InverseQ = Convert.FromBase64String(jsonParameters["InverseQ"].Value<string>());
                rsaParameters.Modulus = Convert.FromBase64String(jsonParameters["Modulus"].Value<string>());
                rsaParameters.P = Convert.FromBase64String(jsonParameters["P"].Value<string>());
                rsaParameters.Q = Convert.FromBase64String(jsonParameters["Q"].Value<string>());

                return builder.AddSigningCredential(CryptoHelper.CreateRsaSecurityKey(rsaParameters, keyId), signingAlgorithm);
            }
            else
            {
                var key = CryptoHelper.CreateRsaSecurityKey();

                RSAParameters parameters;

                if (key.Rsa != null)
                {
                    parameters = key.Rsa.ExportParameters(includePrivateParameters: true);
                }
                else
                {
                    parameters = key.Parameters;
                }

                var jObject = new JObject
                {
                    {
                        "KeyId", key.KeyId
                    },
                    {
                        "Parameters", new JObject
                        {
                            {"D", Convert.ToBase64String(parameters.D)},
                            {"DP", Convert.ToBase64String(parameters.DP)},
                            {"DQ", Convert.ToBase64String(parameters.DQ)},
                            {"Exponent", Convert.ToBase64String(parameters.Exponent)},
                            {"InverseQ", Convert.ToBase64String(parameters.InverseQ)},
                            {"Modulus", Convert.ToBase64String(parameters.Modulus)},
                            {"P", Convert.ToBase64String(parameters.P)},
                            {"Q", Convert.ToBase64String(parameters.Q)}
                        }
                    }
                };

                if (persistKey)
                {
                    File.WriteAllText(filename, jObject.ToString());
                }
                return builder.AddSigningCredential(key, signingAlgorithm);
            }
        }
    }
}
