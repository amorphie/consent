sleep 5 &&
curl -X POST 'http://bbt-consent-vault:8200/v1/secret/data/amorphie-consent' -H "Content-Type: application/json" -H "X-Vault-Token: admin" -d '{ "data": {"HHSCode": "0125", "HHSForwardingAddress":"http://localhost:4900/public/OpenBankingAuthorize?riza_no={0}","PaymentServiceURL":"http://svtstr3app01.ebt.bank/fora/DigitalServices/EftService.svc","AccountServiceURL":"https://nonprod-apisix.burgan.com.tr/test-fora/DigitalServices/AccountService.svc","CustomerServiceURL":"https://nonprod-apisix-gw.apps.nonprod.ebt.bank/preprod-asgard/api/v1", "PostgreSql":"Host=localhost:5432;Database=ConsentDb;Username=postgres;Password=postgres"} }'