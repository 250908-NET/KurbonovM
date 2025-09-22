-- SETUP:
    -- Create a database server (docker)
        -- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourNewStr0ngP@ssw0rd!" -p 1434:1433 --name sqlserver_instance -d mcr.microsoft.com/mssql/server:2022-latest
    -- Connect to the server (Azure Data Studio / Database extension)
    -- Test your connection with a simple query (like a select)
    -- Execute the Chinook database (to create Chinook resources in your db)

-- I am using DBeaver to write and test challenge queries  

-- On the Chinook DB, practice writing queries with the following exercises

-- BASIC CHALLENGES
-- List all customers (full name, customer id, and country) who are not in the USA
select CONCAT(FirstName, LastName), CustomerId, Country from [dbo].[Customer] where Country <> 'USA';
-- List all customers from Brazil
select * from Customer where Country='Brazil';   
-- List all sales agents
select * from Employee where Title='Sales Support Agent';
-- Retrieve a list of all countries in billing addresses on invoices
select distinct BillingCountry from Invoice;
-- Retrieve how many invoices there were in 2009, and what was the sales total for that year?
select count(*) from Invoice where YEAR(InvoiceDate) = 2009;
    -- (challenge: find the invoice count sales total for every year using one query)
select year(InvoiceDate) as InvoiceYear, COUNT(Total) as count, sum(Total) as salesTotal  from Invoice group by year(InvoiceDate);
-- how many line items were there for invoice #37

-- how many invoices per country? BillingCountry  # of invoices -
select distinct(BillingCountry) as BillingCountry, count(InvoiceId) from Invoice group by BillingCountry;
-- Retrieve the total sales per country, ordered by the highest total sales first.
select distinct(BillingCountry) Country, sum(Total) TotalSales from Invoice group by BillingCountry order by TotalSales DESC;

-- JOINS CHALLENGES
-- Every Album by Artist

-- All songs of the rock genre

-- Show all invoices of customers from brazil (mailing address not billing)

-- Show all invoices together with the name of the sales agent for each one

-- Which sales agent made the most sales in 2009?

-- How many customers are assigned to each sales agent?

-- Which track was purchased the most ing 20010?

-- Show the top three best selling artists.

-- Which customers have the same initials as at least one other customer?



-- ADVACED CHALLENGES
-- solve these with a mixture of joins, subqueries, CTE, and set operators.
-- solve at least one of them in two different ways, and see if the execution
-- plan for them is the same, or different.

-- 1. which artists did not make any albums at all?

-- 2. which artists did not record any tracks of the Latin genre?

-- 3. which video track has the longest length? (use media type table)

-- 4. find the names of the customers who live in the same city as the
--    boss employee (the one who reports to nobody)

-- 5. how many audio tracks were bought by German customers, and what was
--    the total price paid for them?

-- 6. list the names and countries of the customers supported by an employee
--    who was hired younger than 35.


-- DML exercises

-- 1. insert two new records into the employee table.

-- 2. insert two new records into the tracks table.

-- 3. update customer Aaron Mitchell's name to Robert Walter

-- 4. delete one of the employees you inserted.

-- 5. delete customer Robert Walter.
