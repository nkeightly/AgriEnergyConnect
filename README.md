# Agri-Energy Connect Platform

Agri-Energy Connect Platform is a comprehensive solution designed to bridge the gap between farmers and employees in the agricultural sector. The platform offers various functionalities to streamline product management, provide industry insights through video recommendations, and ensure effective communication between stakeholders.

## Table of Contents

- [Setup Instructions](#setup-instructions)
- [System Functionalities](#system-functionalities)
  - [User Roles](#user-roles)
  - [User Interface Design](#user-interface-design)
- [User Testing Feedback](#user-testing-feedback)
- [Database Access Instructions](#database-access-instructions)
- [Additional Information](#additional-information)

## Setup Instructions

1. **Clone the repository:**
    ```sh
    git clone https://github.com/yourusername/agrienergyconnect.git
    ```

2. **Navigate to the project directory:**
    ```sh
    cd agrienergyconnect
    ```

3. **Update the database connection string in `appsettings.json`.**

4. **Run the following commands to setup the database:**
    ```sh
    dotnet ef database update
    ```

5. **Run the application:**
    ```sh
    dotnet run
    ```

## System Functionalities

### User Roles
- **Farmer:**
  - Add new products: (Name, Category, Production Date, Image)
  - View all saved product information in the form of a Dashboard
  - View monthly video recommendations based on current farming industry trends
  - Log out

- **Employee:**
  - Add new farmer profiles (by adding the additional information needed for the user to be part of the employee's system)
  - View and filter products by dates and category
  - View specific farmer's product information
  - View monthly training videos related to their role
  - Log out

### User Interface Design
- Green color scheme reflecting the farming theme.
- Responsive design for various devices.
- WCAG compliant and accessible.

## User Testing Feedback
1. **Thapelo Malepe:** Found the dashboard easily accessible and appreciated the video and image features.
2. **Kamogelo Malatji:** Praised the unique and memorable logo.
3. **Victory Mathebula:** Liked the application's color scheme.
4. **Una Radzume:** Enjoyed the modern design and smooth animations.

## Database Access Instructions

To view the data stored in the database, use the following SQL queries:

- **View all necessary tables:**
    ```sql
    SELECT * FROM AgriEnergyConnectDb.dbo.AspNetUsers;
    SELECT * FROM AgriEnergyConnectDb.dbo.Products;
    SELECT * FROM AgriEnergyConnectDb.dbo.FarmerProfiles;
    SELECT * FROM AgriEnergyConnectDb.dbo.EmployeeProfiles;
    ```

- **Delete a user by ID:**
- **Replace This <>**
    ```sql
    DELETE FROM AgriEnergyConnectDb.dbo.<Table> WHERE Id = <number>;
    ```

## Additional Information

For more details on setting up the development environment and running the prototype, you can contact me: keightlymabasa@gmail.com.

---

