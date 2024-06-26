using Microsoft.AspNetCore.Mvc;
using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Azure.Security.KeyVault.Certificates;


public class SecretController : Controller
{

    private readonly string _secretValue;
    private readonly string _certificateValue;



    public SecretController()
    {
        SecretClientOptions options = new SecretClientOptions()
        {
            Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };
        var credential = new DefaultAzureCredential();
        var client = new SecretClient(new Uri("https://pro2keyvault.vault.azure.net/"), credential);
        var Certificateclient = new CertificateClient(new Uri("https://pro2keyvault.vault.azure.net/"), credential);

        KeyVaultSecret secret = client.GetSecret("DBConnectionString");
        KeyVaultCertificateWithPolicy certificateWithPolicy = Certificateclient.GetCertificate("Mycert");

        _secretValue = secret.Value;
        _certificateValue = certificateWithPolicy.Name;
    }

    public IActionResult Index()
    {
        ViewBag.SecretValue = _secretValue;
        ViewBag.certificateWithPolicy = _certificateValue;
        return View();
    }
}
