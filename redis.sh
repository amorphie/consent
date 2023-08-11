redis-server --daemonize yes && sleep 3
redis-cli MSET config-amorphie-contract-db 'User ID=postgres;Password=postgres;Host=Localhost;Port=5432;Database=contract;'
redis-cli MSET config-error '"en" '{"Errors.NotFound":"The requested item was not found.","Errors.ValidationError":"Validation failed for the provided data."}'
redis-cli save 
redis-cli shutdown 
redis-server