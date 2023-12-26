sleep 5 &&
curl -X POST 'http://bbt-consent-vault:8200/v1/secret/data/amorphie-consent' -H "Content-Type: application/json" -H "X-Vault-Token: admin" -d '{ "data": {"HHSCode": "0125", "HHSForwardingAddress":"http://localhost:4900/public/OpenBankingAuthorize?riza_no={0}", "PostgreSql":"Host=localhost:5432;Database=ConsentDb;Username=postgres;Password=postgres", "ServiceURLs:PaymentServiceURL": "PaymentServiceURLAddress", "ServiceURLs:AccountServiceURL": "AccountServiceURLAddress","ServiceURLs:TokenServiceURL": "TokenServiceURLAddress","ServiceURLs:ContractServiceURL": "ContractServiceURLAddress"} }'