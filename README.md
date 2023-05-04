# My Companion ![MIT License](https://img.shields.io/badge/Version-v1.1.0-green.svg)

Our Major Project.

### Authors

- [@AnkitRana](https://www.github.com/Ankit161001)

## Documentation

Two Resource Groups each for the Terraform and the ASP.NET Project.

## Resource Group (for Terraform) (major-project-tf)

![image](https://user-images.githubusercontent.com/61089784/234833399-e991ddb0-76f5-42e2-b5c1-744df6be8fe0.png)
> The Terraform Resource Group

We have to create only the storage account with the Storage Account and the VM. The Storage Account has a Container which will store the state file after terraform is applied. And the VNet, Subnet, Disk, NSG, NIC, Public IP and either Default or created automatically:

- Storage Account (majorprojecttf)
  - Container (testcont01): for the terraform state file.
- Virtual Machine (VM-majorproject-tf)
- VNet (created on VM creation)
- Public IP (created on VM creation)
- Network Security Group (created on VM creation)
- NIC (created on VM creation)
- VM Disk (created on VM creation)

![image](https://user-images.githubusercontent.com/61089784/234834702-becd5e1d-d059-4e98-99c7-4359c2e1d155.png)
> The container inside the storage account

![image](https://user-images.githubusercontent.com/61089784/234834857-f30e8fba-4a00-4eac-9e7e-a1446a9f7805.png)

![image](https://user-images.githubusercontent.com/61089784/234834963-d6bac07d-a5a7-4e91-a65f-3554e2d02663.png)
> the tfstate file after terraform has been run

## Azure DevOps

We add two new Agent Pools.

## MajorTFLinuxPool

Self Hosted Agent with access to all pipelines.

We create a new agent for this pool. We use the VM “VM-majorproject-tf” for this agent.

[Steps to make a Self-Hosted Agent](https://www.coachdevops.com/2023/01/how-to-setup-self-hosted-linux-agent-in.html)

> **A new service connection can be created but not necessary.**

## Virtual Machine 1 i.e. VM-majorproject-tf

> **Access Tokens after creation must be kept safe**

VM should have “zip” installed:
```
sudo apt install zip
```
> **Pipeline will fail without “zip” as it is required to install terraform in the Linux Agent.**

## The Pipeline Code

> **The Terraform Extension must be installed in the Azure DevOps Portal for convenience.**

```
trigger:
- main

pool: MajorTFLinuxPool

steps:
- task: TerraformInstaller@1
  inputs:
    terraformVersion: 'latest'
- task: TerraformTaskV4@4
  inputs:
    provider: 'azurerm'
    command: 'init'
    backendServiceArm: 'Azure for Students(a586d4b4-1f57-461e-9567-09269d497bc8)'
    backendAzureRmResourceGroupName: 'major-project-tf'
    backendAzureRmStorageAccountName: 'majorprojecttf'
    backendAzureRmContainerName: 'testcont01'
    backendAzureRmKey: 'tf/tf.tfstate'
- task: TerraformTaskV4@4
  inputs:
    provider: 'azurerm'
    command: 'validate'
- task: TerraformTaskV4@4
  inputs:
    provider: 'azurerm'
    command: 'apply'
    environmentServiceNameAzureRM: 'Azure for Students(a586d4b4-1f57-461e-9567-09269d497bc8)'
```

## The Terraform Code

```
terraform {
  backend "azurerm" {
  }
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }
}

# Configure the Microsoft Azure Provider
provider "azurerm" {
  features {
  }
}

data "azurerm_client_config" "current" {}

# Create a resource group
resource "azurerm_resource_group" "rsgrp" {
  name     = "major-project"
  location = "Central India"
}

# The MS Sql Database Server
resource "azurerm_mssql_server" "sqlserver" {
  name                         = "major-sqlserver"
  resource_group_name          = azurerm_resource_group.rsgrp.name
  location                     = azurerm_resource_group.rsgrp.location
  version                      = "12.0"
  administrator_login          = "major-admin"
  administrator_login_password = "Amity@123"
  minimum_tls_version          = "1.2"
}

# The MS Sql Database
resource "azurerm_mssql_database" "sqldb" {
  name           = "major-db"
  server_id      = azurerm_mssql_server.sqlserver.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 2
  sku_name       = "Basic"
  zone_redundant = false
}

# Create a virtual network within the resource group
resource "azurerm_virtual_network" "vnet" {
  name                = "major-network"
  resource_group_name = azurerm_resource_group.rsgrp.name
  location            = azurerm_resource_group.rsgrp.location
  address_space       = ["10.0.0.0/16"]
}

# The subnet for the VM
resource "azurerm_subnet" "subnet" {
  name                 = "major-subnet"
  resource_group_name  = azurerm_resource_group.rsgrp.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.2.0/24"]
}

# The Network Interface for the VM
resource "azurerm_network_interface" "netint" {
  name                = "major-nic"
  location            = azurerm_resource_group.rsgrp.location
  resource_group_name = azurerm_resource_group.rsgrp.name

  ip_configuration {
    name                          = "major-ip"
    subnet_id                     = azurerm_subnet.subnet.id
    private_ip_address_allocation = "Dynamic"
  }
}

# The Virtual Machine for the self-hosted agent
resource "azurerm_linux_virtual_machine" "major-agent" {
  name                = "major-agent"
  resource_group_name = azurerm_resource_group.rsgrp.name
  location            = azurerm_resource_group.rsgrp.location
  size                = "Standard_B2s"
  admin_username      = "ankit"
  admin_password      = "Ankit@123456"
  disable_password_authentication = false
  network_interface_ids = [
    azurerm_network_interface.netint.id,
  ]

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "UbuntuServer"
    sku       = "16.04-LTS"
    version   = "latest"
  }
}

resource "azurerm_app_service_plan" "majorplan" {
  name                = "major-appserviceplan"
  location            = azurerm_resource_group.rsgrp.location
  resource_group_name = azurerm_resource_group.rsgrp.name

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_app_service" "majorservice" {
  name                = "major-app-service"
  location            = azurerm_resource_group.rsgrp.location
  resource_group_name = azurerm_resource_group.rsgrp.name
  app_service_plan_id = azurerm_app_service_plan.majorplan.id

  site_config {
    dotnet_framework_version = "v6.0"
  }
}
```

## The Backend Code in Terraform

```
backend "azurerm" {
}
```

Before we add this section of code in the terraform file, each time we run the pipeline a new resource is added, but not updated as it should be.

This is because the state file is not present for reference.

This is solved by using the “backend” block, as it stores the state file for reference in the assigned Container inside the assigned Storage Account.

## Resource Group for the ASP.NET Project (major-project)

These are the resources created by the Terraform code when it is run through the pipeline.

![image](https://user-images.githubusercontent.com/61089784/234836762-836471c0-39c5-4c1a-a08d-9f6bca30930a.png)

## The SQL Database Server (major-sqlserver)

> **A new Networking Firewall Rule is created to allow all traffic to access the database.**

![image](https://user-images.githubusercontent.com/61089784/234836974-80458da8-c83d-44df-a7d1-47d0dd3491eb.png)

