# TLS et certificats

## explications

https://www.techrepublic.com/blog/data-center/ssl-tls-certificates-what-you-need-to-know/
https://tiptopsecurity.com/how-does-https-work-ssl-tls-explained/
https://security.stackexchange.com/questions/20803/how-does-ssl-tls-work

## créer une certificate authority

https://deliciousbrains.com/ssl-certificate-authority-for-local-https-development/

## notes sur la générationde clés

https://docs.docker.com/engine/security/https/#create-a-ca-server-and-client-keys-with-openssl

## Ubuntu : installer un Certificat

convert myCA.pem to crt :
```
openssl x509 -in foo.pem -inform PEM -out foo.crt
```
Add certificate :
```	
sudo cp foo.crt /usr/local/share/ca-certificates/foo.crt
sudo update-ca-certificates
```

Remove	
```
Remove your CA.
sudo update-ca-certificates --fresh
```
