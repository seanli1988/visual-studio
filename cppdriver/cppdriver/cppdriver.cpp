// cppdriver.cpp : Defines the entry point for the console application.
//
#include <stdlib.h>
#include <iostream>
#include "stdafx.h"

#include "mysql_connection.h"
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>
using namespace std;

int main()
{

	sql::Driver *driver;
	sql::Connection *con;
	sql::Statement *stmt;
	sql::PreparedStatement *pstmt;

	try
	{
		driver = get_driver_instance();
		//for demonstration only. never save password in the code!
		con = driver->connect("tcp://seal-db1.mysql.database.azure.com", "sean@seal-db1", "Password1");
		con->setSchema("employees");
	}
	catch (sql::SQLException e)
	{
		cout << "Could not connect to database. Error message: " << e.what() << endl;
		system("pause");
		exit(1);
	}

	stmt = con->createStatement();
	stmt->execute("DROP TABLE IF EXISTS inventory");
	cout << "Finished dropping table (if existed)" << endl;
	stmt->execute("CREATE TABLE inventory (id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER);");
	cout << "Finished creating table" << endl;
	delete stmt;

	pstmt = con->prepareStatement("INSERT INTO inventory(name, quantity) VALUES(?,?)");
	pstmt->setString(1, "banana");
	pstmt->setInt(2, 150);
	pstmt->execute();
	cout << "One row inserted." << endl;

	pstmt->setString(1, "orange");
	pstmt->setInt(2, 154);
	pstmt->execute();
	cout << "One row inserted." << endl;

	pstmt->setString(1, "apple");
	pstmt->setInt(2, 100);
	pstmt->execute();
	cout << "One row inserted." << endl;
	delete pstmt;

//	select
	sql::ResultSet *result;
	pstmt = con->prepareStatement("SELECT * FROM inventory;");
	result = pstmt->executeQuery();	
	
	while (result->next())
		printf("Reading from table=(%d, %s, %d)\n", result->getInt(1), result->getString(2).c_str(), result->getInt(3));	
	delete result;
	delete pstmt;

	//update
	pstmt = con->prepareStatement("UPDATE inventory SET quantity = ? WHERE name = ?");
	pstmt->setInt(1, 200);
	pstmt->setString(2, "banana");
	result = pstmt->executeQuery();
	printf("Row updated\n");
	
	//delete
	pstmt = con->prepareStatement("DELETE FROM inventory WHERE name = ?");
	pstmt->setString(1, "orange");
	result = pstmt->executeQuery();
	printf("Row deleted\n");
	
	delete stmt;
	delete con;
	delete result;
	system("pause");
	return 0;
}

