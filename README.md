# BlogAPI

## Prerequisites
1. Visual Studio 2019.
2. SqlServer Express 2019 or higher.
3. .Net Core 3.2.
4. Postman v8.11.1 or higher

## Project Installation
1. Clone/Download the repository.
2. Execute the DDL and DML commands in SQL Server.
   ![DDLandDML](https://i.imgur.com/aipEVxE.png)

3. Configure the connection String to your new <em>**ZmgTestDb**</em> in your <em>appsettings.json</em> file.
   ![appsettings](https://i.imgur.com/U6NK7Jz.png)
4. Build the solution.
5. Run the solution.

## How to test
### Swagger
1. Run the project.
2. Open the main page in your explorer. ![](https://i.imgur.com/hpwjqiA.png)
3. Authenticate with one of the following sample credentials
   | UserType |  UserName  | Password |
   | :------- | :--------: | -------: |
   | Editor   |   HireMe   |    dunno |
   | Writer   | LoveZemoga |    dunno |
   | Viewer   |   Viewer   |    dunno |/

   
   ![](https://i.imgur.com/wXdMvnE.png)

4. Use generated JWT token to authenticate to use the Blog specific Methods.
   ![](https://i.imgur.com/oAJ2X1A.png)
   ![](https://i.imgur.com/UcMHOtp.png)
   ![](https://i.imgur.com/Q5X7Q1h.png)

### Postman
1. Import the <em>**Collection**</em> and the <em>**Environment**</em> to your Postman
   ![](https://i.imgur.com/yR7KuzC.png)
   ![](https://i.imgur.com/CQkQ9zf.png)
   ![](https://i.imgur.com/ZPGRJiy.png)
2. Execute any of the requests in the Postman folder **AuthToken** and it will automatically set the token for the specific role for all the Blog operations.
   ![](https://i.imgur.com/gdtMMii.png)
   ![](https://i.imgur.com/Q9iFQaF.png)
3. Execute any of the Blog specific operations.
 ![](https://i.imgur.com/TjzBlx6.png)

###### This project was completed in a total of 25 hours.
