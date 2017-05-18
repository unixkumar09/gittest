sudo systemctl stop srms.service
dotnet restore
dotnet publish -c Release -o /var/lib/jenkins/srms
sudo systemctl start srms.service