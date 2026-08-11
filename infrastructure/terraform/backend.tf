terraform {
  backend "azurerm" {
    resource_group_name  = "s268d01rg-uks-sa-poc"
    storage_account_name = "s268d01stsatfstatepoc"
    container_name       = "tfstate"
    key                  = "schoolaccount-collectapi-aca.tfstate"
  }
}