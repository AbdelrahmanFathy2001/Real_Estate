
# 2M Store

2M Store is an e-commerce web application designed for users to browse and purchase various products. The application is built using modern web technologies to provide a seamless shopping experience.

## Features

- **Product Catalog:** Browse a wide range of products.
- **Search Functionality:** Easily find products using search.
- **Shopping Cart:** Add and manage items in your cart.
- **Order Checkout:** Secure and simple checkout process.
- **User Authentication:** Create an account, log in, and manage your orders.

## Project Structure

The 2M Store project follows a standard web application structure:

```
Solution: 2M-Store

├── Project.API
│   ├── Connected Services
│   ├── Dependencies
│   ├── Properties
│   ├── wwwroot                     # Static files and assets
│   ├── Controllers                 # API Controllers
│   ├── Errors                      # Error handling logic
│   ├── Extensions                  # Extension methods
│   ├── Helpers                     # Utility and helper functions
│   ├── Mapping                     # Mapping profiles (e.g., AutoMapper)
│   ├── Middlewares                 # Custom middleware
│   ├── Templates                   # HTML or Email templates
│   ├── Validations                 # Input validations
│   ├── appsettings.json            # Configuration file
│   ├── GlobalUsings.cs             # Global using directives
│   └── Program.cs                  # Entry point of the application
│
├── Project.BLL (Business Logic Layer)
│   ├── Dependencies
│   ├── Dtos                        # Data transfer objects
│   ├── Interfaces                  # Interface definitions
│   ├── Repositories                # Repository patterns
│   ├── Services                    # Business logic services
│   ├── Settings                    # Configuration settings
│   └── Specifications              # Specifications for business rules
│
├── Project.DAL (Data Access Layer)
│   ├── Dependencies
│   ├── AppMetaData                 # Metadata-related classes
│   ├── Data                        # Database context or unit of work
│   ├── Entities                    # Database entities/models
│   ├── Helper                      # Helper classes for DAL operations
│   ├── Migrations                  # Database migrations
│   └── Seed                        # Data seeding scripts
│
├── Project.MVC (Model-View-Controller)
│   ├── Connected Services
│   ├── Dependencies
│   ├── Properties
│   ├── wwwroot                     # Static files and assets for the web application
│   ├── Controllers                 # MVC Controllers
│   ├── Models                      # ViewModels and domain models
│   ├── Views                       # Razor views for UI
│   ├── appsettings.json            # Configuration file
│   ├── GlobalUsings.cs             # Global using directives
│   ├── libman.json                 # Library manager configuration
│   └── Program.cs                  # Entry point of the MVC application
```

## Technologies Used

### Frontend:
- Angular (or any frontend framework being used)
- HTML5, CSS3, JavaScript (ES6+)
- Bootstrap for responsive design

### Backend:
- Asp .Net web API and MVC
- SQL Server for database management
- Entity Framwork 

### Tools:
- Git for version control
- Postman for API testing

## Installation

To run the project locally, follow these steps:

1. Clone the repository:

```bash
git clone https://github.com/Mahmoudoosman19/2M-Store.git
```

2. Navigate to the project directory:

```bash
cd 2M-Store
```

3. Install the dependencies:

```bash
npm install
```

4. Start the backend server:

```bash
cd backend
npm start
```

5. Start the frontend server:

```bash
cd ../src
npm start
```

6. Open your browser and go to `http://localhost:3000` for the frontend, and `http://localhost:5000` for the backend.

## Usage

1. Sign up or log in to your account.
2. Browse the products and add them to your shopping cart.
3. Proceed to checkout to finalize your order.

## Contributing

We welcome contributions to improve the 2M Store. Please follow these steps to contribute:

1. Fork the repository.
2. Create a new feature branch:

```bash
git checkout -b feature/your-feature-name
```

3. Commit your changes:

```bash
git commit -m 'Add your feature'
```

4. Push to your branch:

```bash
git push origin feature/your-feature-name
```

5. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For any questions or issues, please contact:

**Mahmoud Osman Zaki**  
[GitHub Profile](https://github.com/Mahmoudoosman19)
