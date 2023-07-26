sleep 5 &&
curl -X POST 'http://bbt-consent-vault:8200/v1/secret/data/amorphie-consent' -H "Content-Type: application/json" -H "X-Vault-Token: admin" -d '{ "data": {"pass": "my-password", "username":"my-username","PostgreSql":"Host=localhost:5432;Database=ConsentDb;Username=postgres;Password=postgres"} }'
