// cppdriver.cpp : Defines the entry point for the console application.
//
#include <stdlib.h>
#include <iostream>
#include "stdafx.h"

#include "mysql_connection.h"
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/prepared_statement.h>
using namespace std;

int main()
{
	sql::Driver *driver;
	sql::Connection *con;
	sql::PreparedStatement *pstmt;
	sql::ResultSet *result;

	try
	{
		driver = get_driver_instance();
		//for demonstration only. never save password in the code!
		con = driver->connect("tcp://seal-db1.mysql.database.azure.com:3306/employees", "sean@seal-db1", "Password1");
		//con = driver->connect("tcp://myserver4demo.mysql.database.azure.com:3306/quickstartdb", "myadmin@myserver4demo", "server_admin_password");
	}
	catch (sql::SQLException e)
	{
		cout << "Could not connect to database. Error message: " << e.what() << endl;
		system("pause");
		exit(1);
	}
		
	//delete
	pstmt = con->prepareStatement("DELETE FROM inventory WHERE name = ?");
	pstmt->setString(1, "orange");
	result = pstmt->executeQuery();
	printf("Row deleted\n");	
	
	delete pstmt;
	delete con;
	delete result;
	system("pause");
	return 0;
}

