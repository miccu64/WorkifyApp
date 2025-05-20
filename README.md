# WorkifyApp

# Workout Microservices Backend

A modern, event-driven microservices backend for a workout tracking application built with **.NET 9**, using **MassTransit**, **RabbitMQ**, **PostgreSQL**, and **Docker Compose**.

---

## 🧩 Microservices Overview

This system is split into 3 independent, containerized microservices:

### 1. **Auth Service**

* Handles user registration, login, and user deletion
* Uses **JWT** for authentication

### 2. **Workout Service**

* Manages workout plans and exercises
* Shares exercise definitions and user plans

### 3. **ExerciseStat Service**

* Stores and provides statistics for exercises

All services communicate asynchronously via **MassTransit** using **RabbitMQ**.

---

## 🛠️ Technologies Used

* **.NET 9**
* **MassTransit**
* **RabbitMQ**
* **Entity Framework Core**
* **PostgreSQL**
* **JWT**
* **Docker & Docker Compose**
* **xUnit & Moq**

---

## 🚀 Getting Started

To run the entire system locally:

```bash
docker compose -f 'dev-docker-compose.yml' up --build 
```

## 🧪 Testing

Unit tests are written using **xUnit** and **Moq**.
Each service has its own test project under `/tests`.

Run tests with:

```bash
dotnet test
```

---

## 📡 Communication Flow

All services publish and subscribe to domain events via RabbitMQ.
For example:

* `UserDeleted` published by Auth service
* Subscribed by ExerciseStat and Wourkout services to clean up unnecessary user data

---
