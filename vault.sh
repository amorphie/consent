sleep 5 &&
curl -X POST 'http://bbt-template-vault:8200/v1/secret/data/amorphie-template' -H "Content-Type: application/json" -H "X-Vault-Token: admin" -d '{ "data": {"pass": "my-password", "username":"my-username"} }'
