# Self_Signed_Certificate
Создание самоподписанного сертификата

### Quick start
#### Run *.exe file
#### Get your certificate from Result directory


### Use command line to set settings
-h || -help

#### Self_Signed_Certificate.exe -s D:\Temp -n Cert -f my_new_certificate -p 123qwe -y 1

where:
  1. -s is path to directory where certificate will save
  2. -n Name of certificate
  3. -f The file name of certificate
  4. -p Password key to certificate
  5. -y How long certificate will work
 
if you don't set some setting:

-s Environment.CurrentDirectory\Result

-n SelfSignedCertificate

-f SelfSignedCertificate

-p P@55w0rd

-y 5
