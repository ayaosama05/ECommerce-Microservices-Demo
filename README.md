# ECommerce-Microservices-Demo
Demo Application using .NET Core API 

<!-- ABOUT THE PROJECT -->
## About The Project

Small Microservices E-Commerce Application using .NET API with the following structure:

* ECommerce.Api.Gateway
* ECommerce.Api.Customers
* ECommerce.Api.Orders
* ECommerce.Api.Products
* ECommerce.Api.Search
* ECommerce.Api.Tests
* docker-compose
  
 ### Built With

<img height="50" src="https://user-images.githubusercontent.com/25181517/121405754-b4f48f80-c95d-11eb-8893-fc325bde617f.png"> <img height="50" src="https://user-images.githubusercontent.com/25181517/192107858-fe19f043-c502-4009-8c47-476fc89718ad.png"><img height="50" src="https://user-images.githubusercontent.com/25181517/192109061-e138ca71-337c-4019-8d42-4792fdaa7128.png">
 <img height="50" src="https://user-images.githubusercontent.com/25181517/117207330-263ba280-adf4-11eb-9b97-0ac5b40bc3be.png">

### API End Points

Run End-Points through [Ocelot API Gateway](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html/) / Using Docker

|      End Point           |    Verb       |                        URL                                   |               Result                |
| ------------------------ | ------------- | -------------------------------------------------------------| ----------------------------------- |
| Get All Products         |     GET       |      http://localhost:48199/gateway/products                 |    Get List Of Products             |
| Get Product By ID        |     GET       |      http://localhost:48199/gateway/products/{id}            |    Get Product Info                 |
| Get All Customers        |     GET       |      http://localhost:48199/gateway/customers                |    Get List Of Customers            |
| Get Customer By ID       |     GET       |      http://localhost:48199/gateway/customers/{id}           |    Get Customer Info                |
| Get orders By CustomerID |     GET       |      http://localhost:48199/gateway/orders/{customerId}      |    Get Customer Orders              |
| Search By CustomerID     |     POST      |      http://localhost:48199/gateway/search                   |    Get Customer Info + his orders   |

> To be solved: `Search End-Point Not working Using Docker` 



