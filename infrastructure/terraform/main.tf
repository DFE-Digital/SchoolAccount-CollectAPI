data "azurerm_app_configuration" "app_config" {
  name                = "s268d01appcs-sa-shared"
  resource_group_name = "s268d01rg-uks-sa-shared"
}


data "azurerm_key_vault" "kv" {
  name                = "s268d01kvs-sa-shared"
  resource_group_name = "s268d01rg-uks-sa-shared"
}

data "azurerm_key_vault_secret" "ai_key" {
  name         = "AppInsightsInstrumentationKey"
  key_vault_id = data.azurerm_key_vault.kv.id
}


resource "azurerm_container_app" "app" {
  name                         = "schoolaccount-collectapi-app"
  resource_group_name          = "s268d01rg-uks-sa-poc"
  container_app_environment_id = data.azurerm_container_app_environment.env.id
  revision_mode                = "Single"

  identity {
    type = "SystemAssigned"
  }

  template {
    container {
      name   = "schoolaccount-collectapi-app"
      image  = "ghcr.io/dfe-digital/schoolaccount-collectapi:latest"
      cpu    = 0.25
      memory = "0.5Gi"
      #Env Var (Terraform)
      env {
        name  = "AzureAppConfiguration__Endpoint"
        value = data.azurerm_app_configuration.app_config.endpoint
      }

      #Secrets Key Vault
      env {
        # temporary build in Terraform from App Insight key from KV secrets
        name  = "APPLICATIONINSIGHTS_CONNECTION_STRING"
        value = "InstrumentationKey=${data.azurerm_key_vault_secret.ai_key.value};IngestionEndpoint=https://uksouth-0.in.applicationinsights.azure.com/"
      }
    }
  }

  ingress {
    external_enabled = true
    target_port      = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }
}

data "azurerm_container_app_environment" "env" {
  name                = "s268d01ace-sa-poc"
  resource_group_name = "s268d01rg-uks-sa-poc"
}

resource "azurerm_role_assignment" "app_config_reader" {
  scope                = data.azurerm_app_configuration.app_config.id
  role_definition_name = "App Configuration Data Reader"
  principal_id         = azurerm_container_app.app.identity[0].principal_id
  depends_on = [
    azurerm_container_app.app
  ]
  skip_service_principal_aad_check = true
}

resource "azurerm_key_vault_access_policy" "aca_access" {
  key_vault_id = data.azurerm_key_vault.kv.id

  tenant_id = azurerm_container_app.app.identity[0].tenant_id
  object_id = azurerm_container_app.app.identity[0].principal_id

  secret_permissions = [
    "Get",
    "List"
  ]

  depends_on = [
    azurerm_container_app.app
  ]
}