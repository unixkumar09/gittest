# Vans SRMS API Server Setup

These instructions will get a fresh Ubuntu Linux machine up and running with the Vans Stockroom Management System API server.  We are assuming you just installed the OS, so let's make sure we're starting from a good point by running:
```
sudo apt-get update
```

The next step is to choose a password.  Remember it and use it anywhere you see **INSERT_PASSWORD_HERE** in these instructions.  We'll try to warn you when that happens so you don't forget.

OK, let's get started!



## Step 1. Install Postgres
We will use a local instance of [PostgreSQL](https://www.postgresql.org/) for this application.  Let's start by installing the database server:
```
sudo apt-get install postgresql
```

Next we'll login with the default admin user:
```
sudo su - postgres
psql
```
For extra security, you can set a password for the admin user.  This is not required, but it is recommended:

**DON'T FORGET TO REPLACE THE PASSWORD!**
```
ALTER USER postgres PASSWORD 'INSERT_PASSWORD_HERE';
```
Then create a custom user for this app:

**DON'T FORGET TO REPLACE THE PASSWORD!**
```
CREATE ROLE srms WITH PASSWORD 'INSERT_PASSWORD_HERE' LOGIN CREATEDB REPLICATION SUPERUSER;
 ```
 
 You can check that the user/role was created by running:
 ```
 \du
  ``` 
 Next, exit out of psql and switch back to default user
 ```
 \q
 exit
 ```


## Step 2. Install Git
[Git](https://git-scm.com/) will be needed in order to retrieve source code updates.
**INSERT A USERNAME AND EMAIL ADDRESS HERE**
```
sudo apt-get install git
git config --global user.name ""
git config --global user.email ""
```


## Step 3. Install .NET Core SDK [Link to more detailed instructions](https://www.microsoft.com/net/core#linuxubuntu)
This application is written in [.NET Core](https://www.microsoft.com/net/core).  The .NET Core SDK will be needed in order to run it.
```
sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
sudo apt-get update
sudo apt-get install dotnet-dev-1.0.3

```



## Step 4. Install Jenkins
We use the [Jenkins](https://jenkins.io/) Continuous Integration platform to Poll the Github repository for source code changes.  Any updates will be automatically retrieved, built, and published.  Jenkins will also be used to run nightly product updates, reporting, and database backups.

1. Add repo and install. (Again, copy/paste all 4 lines...hit `Enter` after first 3 commands have finished running)
```
wget -q -O - https://pkg.jenkins.io/debian/jenkins-ci.org.key | sudo apt-key add -
sudo sh -c 'echo deb http://pkg.jenkins.io/debian-stable binary/ > /etc/apt/sources.list.d/jenkins.list'
sudo apt-get update
sudo apt-get install jenkins

```
2. Open a browser and go to `localhost:8080`.  You should see an unlock screen that asks you to copy a key from a file or from the system logs.  If you see a login screen instead of this unlock screen, the try navigating to localhost:8080 again.  Here's a shortcut command to open the secret file so you can copy the contents:
```
sudo nano /var/lib/jenkins/secrets/initialAdminPassword
```  

Next, choose the option to install Jenkins with the default plugins, then create a new user and login.  
*Remember the username you chose, we'll use it when creating an SSH key in the next step.*



3. Create SSH keys for jenkins user
**DON'T FORGET TO INSERT THE USERNAME YOU JUST CHOSE FOR THE JENKINS SERVER**
```
sudo su jenkins
ssh-keygen -t rsa -C 'JENKINS_USERNAME'
```
* When prompted, choose the default location for ssh key files.  It should be (/var/lib/jenkins/.ssh/id_rsa).
* When prompted, do not use a passphrsae for ssh key files.
* Exit out of the jenkins user prompt with:
```
exit
```

4. Follow these [Instructions](https://developer.github.com/guides/managing-deploy-keys/#deploy-keys) to add your new SSH key as a Deploy Key in the Github project.

5. Back on the Jenkins server (localhost:8080) create a new job
    * Set the name as SRMS
    * Choose the Freestyle project option
    * Hit OK

6. You should now be on the configure page for your new Job/Project.  Follow these steps to configure the job:
    * In the `General` section, check the `Discard old builds` option
    * In the `Source Code Management` section, choose Git for Source Code Management
    * Add the SSH address for Repository URL (example: git@github.com:Skookum/Vans_SRMS_API.git).  Ignore the big red error message that pops up.
    * Click 'Add' button to create new credentials, then select `Jenkins` for the Credentials Provider
	* For Kind, choose `SSH Username with private key`
	* Enter the username chosen when creating the SSH key
	* Select `From the Jenkins master ~/.ssh` option
	* Click Add to close the Credentials popup window
	* You will then need to select the newly created Credentials in the dropdown
	* Click `Apply` and the red error message should go away if Jenkins is able to successfully authenticate with the Github repo
	* Under `Branches to build`, enter `*/master` if it is not already entered
	* In the Build Triggers section, select `Poll SCM`
	* enter the following cron schedule: `H 2 * * *`
	* In the `Build` section, click the `Add build step` dropdown and select `Execute shell`
	* Enter the following command: `sudo /var/lib/jenkins/srms_build.sh`
	* Click the Save button to exit out of this screen

7. Next we'll create a shell script that will perform the build.  The Jenkins user will need sudo privileges in order to stop/restart the running Kestrel service, as well as to build and publish the website.  Instead of giving the Jenkins user global sudo privilges, we'll create a shell script and give the Jenkins user sudo privileges to just that file.  In your terminal window, enter:
```
sudo nano /var/lib/jenkins/srms_build.sh
```
Paste the following lines into the srms_build.sh file:
 ```
sudo systemctl stop srms.service
sudo dotnet restore
sudo dotnet publish -c Release -o /usr/local/bin/srms
sudo systemctl start srms.service
sleep 1m
curl -v -X PUT --header 'Accept: text/plain' 'http://127.0.0.1/api/Product/Import' -d ''
 ```

Next we'll need to make that file executable.  Run this command:
```
sudo chmod +x /var/lib/jenkins/srms_build.sh
```

8. Give Jenkins sudo privileges for that script file.  We will create a file called `srms` in the `/etc/sudoers.d` directory.  
```
cd /etc/sudoers.d
sudo nano srms
```
paste the following line into that file and save:
```
jenkins ALL = NOPASSWD: /var/lib/jenkins/srms_build.sh, /usr/bin/dotnet, /bin/systemctl
```

Before we run the build, there is one more package that needs to be installed.  Run this to install curl if it's not already on your machine:
```
sudo apt-get install curl
```

## Step 5. Create directory and service for website
The directory will be the location that we publish the website to.  That is where the files will be hosted from.
1. create directory
```
cd /usr/local/bin
sudo mkdir srms
```
2. create service file `sudo nano /etc/systemd/system/srms.service`
3. set contents of the file to: 
**DON'T FORGET TO REPLACE THE PASSWORD!**
```
[Unit]
Description=.NET Core Web Api for SRMS project

[Service]
WorkingDirectory=/usr/local/bin/srms
ExecStart=/usr/bin/dotnet /usr/local/bin/srms/Vans_SRMS_API.dll
Restart=always
RestartSec=10  # Restart service after 10 seconds if dotnet service crashes
SyslogIdentifier=srms-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=SRMSConnection=Server=localhost;Port=5432;Database=SRMS;Userid=srms;Password=INSERT_PASSWORD_HERE;

[Install]
WantedBy=multi-user.target
```
4. enable the new service with: 
```
sudo systemctl enable srms.service
```


## Step 6. Install Nginx (reverse proxy)
.NET Core comes with a Kestrel server that runs on port 5000.  Since Kestrel is not a robust solution, we'll use [Nginx](https://www.nginx.com/) as a reverse proxy to forward all request from port 80 to port 5000.
1. Install Nginx
```
sudo apt-get install nginx
```
2. Configure the reverse proxy. 
Open the config file at: `/etc/nginx/sites-available/default` with your favorite text editor:
```
sudo nano /etc/nginx/sites-available/default
```
then replace the body with the following code:

```
server {
	listen 80;
	location / {
		proxy_pass http://localhost:5000;
		proxy_http_version 1.1;
		proxy_set_header Upgrade $http_upgrade;
		proxy_set_header Connection keep-alive;
		proxy_set_header Host $host;
		proxy_cache_bypass $http_upgrade;
	}
	location /ws {
		proxy_pass http://localhost:5000;
		proxy_http_version 1.1;
		proxy_set_header Upgrade $http_upgrade;
		proxy_set_header Connection "Upgrade";
		proxy_set_header Host $host;
		proxy_cache_bypass $http_upgrade;
	}
}
```

3. Start Nginx and reload the config file
```
sudo service nginx start
sudo nginx -t
```



That's it!  Reboot the computer to make sure all the new settings will take effect, then log back into the Jenkins server.  You should see a button that looks like a clock with a green play button overlayed on it.  Click that button to kick off a build of your project. 

The first build should take several minutes to complete.  If you click on the progress bar for the build (in the Build Executor Status section) you will be taken to a screen that shows the Console Output.  That will allow you to monitor the progress.  

Once the build is done, open your browser and navigate to `localhost/swagger` to see the API documentation screen for the new website.  The last step to get the API server ready for testing is to set a default store number for this server.  This will be the store number for the Vans Retail store that the server will be sent to.  You can set the store number via the Swagger documentation page.  Scroll all the way to the bottom of that screen until you see the Orange PUT route for /api/Store.  Click that route to expand it, enter the desired store number in the storeNumber textbox (ignore the DeviceKey textbox), and click the `Try it out!` button.



## Step 7: Configure computer to never go to sleep
Since this server needs to stay running all the time, we'll need to turn off the sleep settings.  
* Go into the Ubuntu System Settings
* Select Power, then make sure it's set to `Don't suspend`

## Optional settings
* Install any additional firewall or corporate applications 
* Configure the operating system for automatic updates

