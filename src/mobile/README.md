## Pointing xamarin (android) at a local service:

1. Use http
2. Use 10.0.2.2 instead of localhost
3. Turn off https redirection in the service
4. Configure the service in .vs/config/applicationhost.config
```
<binding protocol="http" bindingInformation="*:62020:127.0.0.1" />
```

The important bit is 127.0.0.1 instead of localhost