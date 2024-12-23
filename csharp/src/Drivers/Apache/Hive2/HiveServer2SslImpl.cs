/*
* Licensed to the Apache Software Foundation (ASF) under one or more
* contributor license agreements.  See the NOTICE file distributed with
* this work for additional information regarding copyright ownership.
* The ASF licenses this file to You under the Apache License, Version 2.0
* (the "License"); you may not use this file except in compliance with
* the License.  You may obtain a copy of the License at
*
*    http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;

namespace Apache.Arrow.Adbc.Drivers.Apache.Hive2
{
    static class HiveServer2SslImpl
    {
        static internal Dictionary<string, string> ValidateTlsOptions(IReadOnlyDictionary<string, string> Properties)
        {
            Dictionary<string, string> TlsProperties = new Dictionary<string, string>();
            Properties.TryGetValue(TlsOptions.IsSslEnabled, out string? isSslEnabled);
            if (!bool.TryParse(isSslEnabled, out bool isSslEnabledBool) || !isSslEnabledBool)
            {
                TlsProperties[TlsOptions.IsSslEnabled] = "False";
                return TlsProperties;
            }
            TlsProperties[TlsOptions.IsSslEnabled] = "True";
            Properties.TryGetValue(TlsOptions.AllowHostnameMismatch, out string? allowHostnameMismatch);
            TlsProperties[TlsOptions.AllowHostnameMismatch] = !bool.TryParse(allowHostnameMismatch, out bool allowHostnameMismatchBool) || !allowHostnameMismatchBool ? "False" : "True";
            Properties.TryGetValue(TlsOptions.AllowSelfSigned, out string? allowSelfSigned);
            TlsProperties[TlsOptions.AllowSelfSigned] = !bool.TryParse(allowSelfSigned, out bool allowSelfSignedBool) || !allowSelfSignedBool ? "False" : "True";
            if (allowSelfSignedBool)
            {
                Properties.TryGetValue(TlsOptions.TrustedCertificatePath, out string? trustedCertificatePath);
                if (trustedCertificatePath == null) return TlsProperties;
                TlsProperties[TlsOptions.TrustedCertificatePath] = trustedCertificatePath != "" && File.Exists(trustedCertificatePath) ? trustedCertificatePath : throw new FileNotFoundException("Trusted certificate path is invalid or file does not exist.");
            }
            return TlsProperties;
        }

        static internal HttpClientHandler NewHttpClientHandler(Dictionary<string, string> TlsProperties)
        {
            HttpClientHandler httpClientHandler = new();
            TlsProperties.TryGetValue(TlsOptions.IsSslEnabled, out string? isSSl);
            if (Convert.ToBoolean(isSSl))
            {
                TlsProperties.TryGetValue(TlsOptions.AllowSelfSigned, out string? allowSelfSigned);
                TlsProperties.TryGetValue(TlsOptions.AllowHostnameMismatch, out string? allowHostnameMismatch);
                TlsProperties.TryGetValue(TlsOptions.TrustedCertificatePath, out string? trustedCertificatePath);
                httpClientHandler.ServerCertificateCustomValidationCallback = (request, certificate, chain, policyErrors) =>
                {
                    if (policyErrors == SslPolicyErrors.None) return true;
                    if (string.IsNullOrEmpty(trustedCertificatePath))
                    {
                        return
                            (!policyErrors.HasFlag(SslPolicyErrors.RemoteCertificateChainErrors) || Convert.ToBoolean(allowSelfSigned))
                        && (!policyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNameMismatch) || Convert.ToBoolean(allowHostnameMismatch));
                    }
                    if (certificate == null)
                        return false;
                    X509Certificate2 customCertificate = new X509Certificate2(trustedCertificatePath);
                    X509Chain chain2 = new X509Chain();
                    chain2.ChainPolicy.ExtraStore.Add(customCertificate);

                    // "tell the X509Chain class that I do trust this root certs and it should check just the certs in the chain and nothing else"
                    chain2.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

                    // This setup does not have revocation information
                    chain2.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                    // Build the chain and verify
                    return chain2.Build(certificate);
                };
            }

            return httpClientHandler;
        }
    }
}
