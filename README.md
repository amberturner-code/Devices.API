# Devices.API

This is a .NET Core Web API service that exposes 2 endpoints, one to retrieve a list of Devices from an in-memory database, and one to assign a Device. 
The endpoints are testable using Swagger.

It publishes a message using RabbitMQ as a broker to assign a device. It also runs a background service that "listens" for device assignment response messages, 
and then updates the device's status in the database. 

<b>To install RabbitMQ:</b>
https://www.rabbitmq.com/download.html

<b>To run RabbitMQ locally in a Docker container, run these commands in Powershell:</b>

docker run -d --hostname my-rabbit --name some-rabbit -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password rabbitmq:3-management

docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

Navigate to http://localhost:15672 to open the RabbitMQ Management UI and view the queues.

Errors and informational messages for this service are written to a log file here: <b>%appdata%/Devices/logs</b>
