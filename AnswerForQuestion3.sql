--Drop the table if it already exists
DROP TABLE IF EXISTS Model;

--Create a table with the following columns and data: Make, Model
--Id column :primary key with auto incremental Id
CREATE TABLE Model (
    Id INT IDENTITY PRIMARY KEY,
    Make VARCHAR(50),
    Model VARCHAR(50)
);

INSERT INTO Model (Make, Model)
VALUES
    ('Toyota', 'Corolla'),
    ('Toyota', 'Camry'),
    ('Nissan', 'Duke'),
    ('Nissan', 'Duke'),
    ('Mazda', '3'),
    ('Mazda', 'CX5'),
    ('Toyota', 'Camry'),
    ('Ford', 'Raptor');

-- Check before removing the duplicates
 Select * from Model 

/*
I am providing two solutions to remove the duplicates
*/

-- Option1: DELETE statement with a subquery 
DELETE FROM Model
WHERE Id NOT IN (
    SELECT MAX(Id)
    FROM Model
    GROUP BY Make, Model
);

-- OR

--Option2: DELETE using join

--For any two rows with the same Make and Model, it finds the one (m1) with the lower ID and deletes it.
--So only the row with the highest ID for each (Make, Model) survives.
DELETE m1
FROM Model m1
JOIN Model m2
  ON m1.Make = m2.Make
 AND m1.Model = m2.Model
 AND m1.Id < m2.Id;

-- Check after removing the duplicates
Select * from Model
